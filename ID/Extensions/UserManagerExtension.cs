using ID.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace ID.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<string> GeneratePhoneNumberLoginTokenAsync(this UserManager<User> userManager, User user)
        {
            return await userManager.GenerateUserTokenAsync(user, "PhoneLoginTokenProvieder", "phone-auth");
        }

        public static async Task<bool> VerifyPhoneNumberLoginTokenAsync(this UserManager<User> userManager, User user, string token)
        {
            return await userManager.VerifyUserTokenAsync(user, "PhoneLoginTokenProvieder", "phone-auth", token);
        }

        public static async Task<string> GenerateEmailLoginTokenAsync(this UserManager<User> userManager, User user)
        {
            return await userManager.GenerateUserTokenAsync(user, "EmailLoginTokenProvieder", "email-auth");
        }

        public static async Task<bool> VerifyEmailLoginTokenAsync(this UserManager<User> userManager, User user, string token)
        {
            return await userManager.VerifyUserTokenAsync(user, "EmailLoginTokenProvieder", "email-auth", token);
        }

        public static async Task<string> GenerateLoginTokenAsync(this UserManager<User> userManager, User user)
        {
            return await userManager.GenerateUserTokenAsync(user, "LoginTokenProvieder", "id-auth");
        }

        public static async Task<bool> VerifyLoginTokenAsync(this UserManager<User> userManager, User user, string token)
        {
            return await userManager.VerifyUserTokenAsync(user, "LoginTokenProvieder", "id-auth", token);
        }
    }
}
