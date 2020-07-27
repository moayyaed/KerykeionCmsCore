using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    public interface ISignInService
    {
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        Task SignInAsync(object user, bool isPersistent, string authenticationMethod = null);
        Task<SignInResult> PasswordSignInAsync(string username, string password, bool rememberMe, bool lockoutOnFailure);
        bool IsSignedIn(ClaimsPrincipal claimsPrincipal);
        Task SignOutAsync();
        Task RefreshSignInAsync(dynamic user);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
        Task<dynamic> GetTwoFactorAuthenticationUserAsync();
        Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string authenticatorCode, bool rememberMe, bool rememberMachine);
        Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode);
        Task<bool> IsTwoFactorClientRememberedAsync(dynamic user);
        Task ForgetTwoFactorClientAsync();
    }
}
