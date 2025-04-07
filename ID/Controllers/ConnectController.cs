using ID.Domain.Entity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.Server.AspNetCore;
using System.Linq;
using System.Security.Claims;

namespace ID.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictAuthorizationManager _authorizationManager;

        public ConnectController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IOpenIddictApplicationManager applicationManager, 
            IOpenIddictAuthorizationManager authorizationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationManager = applicationManager;
            _authorizationManager = authorizationManager;
        }

        [HttpPost]
        public async Task<IActionResult> Token()
        {
            var request = HttpContext.GetOpenIddictServerRequest();

            if (request.IsPasswordGrantType())
                return await IsPass(request);

            if (request.IsAuthorizationCodeGrantType())
                return await IsCode(request);

            if (request.IsClientCredentialsGrantType())
                return await IsClientCredentials(request);

            return BadRequest(new { error = "Invalid grant type" });
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Authorize()
        {
            
            var request = HttpContext.GetOpenIddictServerRequest() ??
                  throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
            
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
            if (application == null)
                return BadRequest(new { error = "Invalid client_id" });

            if (!User.Identity.IsAuthenticated)
                return Challenge();

            var claims = new List<Claim>
            {
                new Claim(OpenIddictConstants.Claims.Subject, User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                new Claim(OpenIddictConstants.Claims.Email, User.FindFirstValue(ClaimTypes.Email))
            };

            var identity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            principal.SetScopes(request.GetScopes());

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> IsPass(OpenIddictRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            claimsIdentity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Id.ToString()));

            var client = await _applicationManager.FindByClientIdAsync(request.ClientId);
            var scopes = (await _applicationManager.GetPermissionsAsync(client))
                .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope));
            scopes = scopes.Select(x => x[OpenIddictConstants.Permissions.Prefixes.Scope.Length..]);
            var vScopes = new List<string>();
            foreach (var scope in request.Scope.Split(" "))
                if (scopes.Contains(scope))
                    vScopes.Add(scope);
            principal.SetScopes(vScopes);

            await _signInManager.SignInAsync(user, isPersistent: true);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> IsClientCredentials(OpenIddictRequest request)
        {
            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId);
            var principal = new ClaimsPrincipal(identity);

            var client = await _applicationManager.FindByClientIdAsync(request.ClientId);
            var scopes = (await _applicationManager.GetPermissionsAsync(client))
                .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope));
            scopes = scopes.Select(x => x[OpenIddictConstants.Permissions.Prefixes.Scope.Length..]);
            var vScopes = new List<string>();
            foreach (var scope in request.Scope.Split(" "))
                if (scopes.Contains(scope))
                    vScopes.Add(scope);
            principal.SetScopes(vScopes);

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> IsCode(OpenIddictRequest request)
        {
            var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            if (principal == null)
            {
                return BadRequest(new { error = "Invalid authorization code" });
            }

            var userId = principal.FindFirst(OpenIddictConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { error = "Invalid user" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new { error = "User not found" });
            }

            // Создаем новый `ClaimsPrincipal` для токенов
            var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

            var identity = (ClaimsIdentity)newPrincipal.Identity;
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, userId));

            // Устанавливаем разрешенные скоупы
            var scopes = request.GetScopes();
            newPrincipal.SetScopes(scopes);

            // Генерируем `access_token` и `id_token`
            return SignIn(newPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}
