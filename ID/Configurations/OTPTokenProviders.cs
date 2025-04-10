using Microsoft.AspNetCore.Identity;

namespace ID.Configurations
{
    public class EmailLoginTotpTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser> where TUser : IdentityUser
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            return "EmailLoginTokenProvieder:" + purpose + ":" + user.Email;
        }
    }

    public class PhoneLoginTotpTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser> where TUser : IdentityUser
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            return "PhoneLoginTokenProvieder:" + purpose + ":" + user.PhoneNumber;
        }
    }

    public class LoginTotpTokenProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser> where TUser : IdentityUser
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            return "LoginTokenProvieder:" + purpose + ":" + user.PhoneNumber;
        }
    }

    public static class OtpTokenProviders
    {
        public static IdentityBuilder AddTotpTokenProviders(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            builder.AddTokenProvider("EmailLoginTokenProvieder", typeof(EmailLoginTotpTokenProvider<>).MakeGenericType(userType));
            builder.AddTokenProvider("PhoneLoginTokenProvieder", typeof(PhoneLoginTotpTokenProvider<>).MakeGenericType(userType));
            builder.AddTokenProvider("LoginTokenProvieder", typeof(LoginTotpTokenProvider<>).MakeGenericType(userType));
            return builder;
        }
    }
}
