using KerykeionCmsCore.Classes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    public class SignInService<TUser> : ISignInService
        where TUser : KerykeionUser
    {
        protected readonly SignInManager<TUser> SignInManager;

        public SignInService(SignInManager<TUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null)
        {
            return SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return await SignInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public async Task ForgetTwoFactorClientAsync()
        {
            await SignInManager.ForgetTwoFactorClientAsync();
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return await SignInManager.GetExternalAuthenticationSchemesAsync();
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            return await SignInManager.GetExternalLoginInfoAsync();
        }

        public async Task<dynamic> GetTwoFactorAuthenticationUserAsync()
        {
            return await SignInManager.GetTwoFactorAuthenticationUserAsync();
        }

        public bool IsSignedIn(ClaimsPrincipal claimsPrincipal)
        {
            return SignInManager.IsSignedIn(claimsPrincipal);
        }

        public async Task<bool> IsTwoFactorClientRememberedAsync(dynamic user)
        {
            return await SignInManager.IsTwoFactorClientRememberedAsync(user);
        }

        public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool rememberMe, bool lockoutOnFailure)
        {
            return await SignInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure);
        }

        public async Task RefreshSignInAsync(dynamic user)
        {
            await SignInManager.RefreshSignInAsync(user);
        }

        public async Task SignInAsync(object user, bool isPersistent, string authenticationMethod = null)
        {
            await SignInManager.SignInAsync(user as TUser, isPersistent, authenticationMethod);
        }

        public async Task SignOutAsync()
        {
            await SignInManager.SignOutAsync();
        }

        public async Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string authenticatorCode, bool rememberMe, bool rememberMachine)
        {
            return await SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, rememberMachine);
        }

        public async Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode)
        {
            return await SignInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
        }
    }
}
