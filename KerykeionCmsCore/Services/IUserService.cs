using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="file"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> CreateAsync(string username, string email, string password, IFormFile file, string language);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> CreateAsync(string username, string email);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Guid> GetUserIdAsync(object user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GenerateEmailConfirmationTokenAsync(object user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> FindUsersByNameAsync(string searchValue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<dynamic> FindByIdAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        Task<dynamic> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IdentityOptions IdentityOptions();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetUserNameAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetPhoneNumberAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        string GetUserId(ClaimsPrincipal claimsPrincipal);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<IdentityResult> SetPhoneNumberAsync(dynamic user, string phoneNumber);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<dynamic> FindByEmailAsync(string email);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> AddToRoleAsync(dynamic user, string role);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> IsEmailConfirmedAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<IdentityResult> AddToRolesAsync(dynamic user, IEnumerable<string> roles);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<IdentityResult> RemoveFromRoleAsync(dynamic user, string role);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<List<string>> GetRolesAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> DeleteAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetUserProfileImageUrl(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetEmailAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<IdentityResult> ConfirmEmailAsync(dynamic user, string code);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IdentityResult> SetUserNameAsync(dynamic user, string email);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IList<UserLoginInfo>> GetLoginsAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetAuthenticatorKeyAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> GetTwoFactorEnabledAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GeneratePasswordResetTokenAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<IdentityResult> ChangeEmailAsync(dynamic user, string email, string code);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> CountRecoveryCodesAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> CheckPasswordAsync(dynamic user, string password);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> AddLoginAsync(dynamic user, ExternalLoginInfo info);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> ResetPasswordAsync(dynamic user, string code, string password);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newEmail"></param>
        /// <returns></returns>
        Task<string> GenerateChangeEmailTokenAsync(dynamic user, string newEmail);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> HasPasswordAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<IdentityResult> AddPasswordAsync(dynamic user, string newPassword);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<IdentityResult> ChangePasswordAsync(dynamic user, string oldPassword, string newPassword);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticatorTokenProvider"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Task<bool> VerifyTwoFactorTokenAsync(dynamic user, string authenticatorTokenProvider, string verificationCode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        Task<IdentityResult> SetTwoFactorEnabledAsync(dynamic user, bool enabled);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(dynamic user, int number);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> ResetAuthenticatorKeyAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="profileImage"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> AddProfileImage(dynamic user, IFormFile profileImage);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<dynamic> GetUserProfileImageIncluded(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetLanguageAsync(dynamic user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<KerykeionDbResult> SetLanguageAsync(dynamic user, string language);
    }
}
