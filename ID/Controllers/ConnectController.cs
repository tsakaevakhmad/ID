﻿using ID.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly IConfiguration _configuration;

        public ConnectController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IOpenIddictApplicationManager applicationManager, 
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationManager = applicationManager;
            _authorizationManager = authorizationManager;
            _scopeManager = scopeManager;
            _configuration = configuration;
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

            if (request.IsRefreshTokenGrantType())
                return await IsRefreshToken(request);

            if (request.IsDeviceCodeGrantType())
                return await IsDeviceCodeGrantType(request);

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
            {
                var parameters = Base64UrlEncoder.Encode(string.Join("&", request.GetParameters().Select(param =>
                    $"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value.ToString())}")));
                
                var redirectUrl = $"/?params={parameters}";

                return Redirect(redirectUrl);
            }

            var claims = new List<Claim>
            {
                new Claim(OpenIddictConstants.Claims.Subject, User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value)
            };

            var identity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await AddScopes(principal, request);

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> Verify()
        {
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return Unauthorized(new { error = "Authentication failed" });

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return BadRequest(new { error = "User not found" });

            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Id));
            principal.SetScopes(result.Principal.GetScopes());
            principal.SetResources(_scopeManager.ListResourcesAsync(principal.GetScopes()).ToBlockingEnumerable().ToArray());

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> IsPass(OpenIddictRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            await AddScopes(principal, request);
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            claimsIdentity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Id.ToString()));
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> IsClientCredentials(OpenIddictRequest request)
        {
            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId);
            var principal = new ClaimsPrincipal(identity);
            await AddScopes(principal, request);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<IActionResult> IsRefreshToken(OpenIddictRequest request)
        {
            try
            {
                return SignIn(await GenerateUserClaims(request, "refresh_token"), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<IActionResult> IsCode(OpenIddictRequest request)
        {
            try
            {
                return SignIn(await GenerateUserClaims(request, "code"), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<IActionResult> IsDeviceCodeGrantType(OpenIddictRequest request)
        {
            var principal = await GenerateUserClaims(request, "device_code");
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private async Task<ClaimsPrincipal> GenerateUserClaims(OpenIddictRequest request, string requestType)
        {
            var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            if (principal == null)
                throw new Exception($"Invalid {requestType}");

            var userId = principal.FindFirst(OpenIddictConstants.Claims.Subject)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Invalid user");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var newPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var identity = (ClaimsIdentity)newPrincipal.Identity;
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, userId));
            var scopes = request.GetScopes();
            newPrincipal.SetScopes(scopes);
            return newPrincipal;
        }

        private async Task AddScopes(ClaimsPrincipal principal, OpenIddictRequest request)
        {
            var client = await _applicationManager.FindByClientIdAsync(request.ClientId);
            var scopes = (await _applicationManager.GetPermissionsAsync(client))
                .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope));
            scopes = scopes.Select(x => x[OpenIddictConstants.Permissions.Prefixes.Scope.Length..]);
            var vScopes = new List<string>();
            foreach (var scope in request.Scope.Split(" "))
                if (scopes.Contains(scope))
                    vScopes.Add(scope);
            principal.SetScopes(vScopes);
        }
    }
}
