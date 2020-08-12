using ImageManagement.Services;
using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Enums;
using KerykeionCmsCore.Options;
using KerykeionStringExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    public class UserService<TUser> : IUserService
        where TUser : KerykeionUser
    {
        protected readonly UserManager<TUser> UserManager;
        protected readonly EntitiesService EntitiesService;

        private readonly ImagesService _imageService;
        private readonly KerykeionCmsOptions _options;

        public UserService(UserManager<TUser> userManager,
            EntitiesService entitiesService,
            ImagesService imagesService,
            IOptions<KerykeionCmsOptions> options)
        {
            UserManager = userManager;
            EntitiesService = entitiesService;

            _imageService = imagesService;
            _options = options.Value;
        }

        public async Task<KerykeionDbResult> CreateAsync(string username, string email, string password, IFormFile file, string language)
        {
            var user = ActivateUser(username, email);
            if (user == null)
            {
                return KerykeionDbResult.Fail();
            }

            if (file == null && _options.User.RequireProfileImage)
            {
                return KerykeionDbResult.Fail(new KerykeionDbError { Message = "Een profiel foto is verplicht." });
            }

            if (file != null)
            {
                if (!await AddProfileImageToUserAndCheckCompletion(file, user))
                {
                    return KerykeionDbResult.Fail(new KerykeionDbError { Message = "Het uploaden van de profiel foto is mislukt." });
                }
            }

            if (string.IsNullOrEmpty(language))
            {
                user.Language = _options.Pages.Language;
            }
            else
            {
                if (Enum.TryParse<KerykeionCmsLanguage>(language, out _))
                {
                    user.Language = Enum.Parse<KerykeionCmsLanguage>(language);
                }
                else
                {
                    user.Language = _options.Pages.Language;
                }
            }

            var result = await UserManager.CreateAsync(user, password);
            return KerykeionDbResultAfterTryCreateUser(result, user);
        }

        public async Task<KerykeionDbResult> CreateAsync(string username, string email)
        {
            var user = ActivateUser(username, email);
            if (user == null)
            {
                return KerykeionDbResult.Fail();
            }

            var result = await UserManager.CreateAsync(user);
            return KerykeionDbResultAfterTryCreateUser(result, user);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(object user)
        {
             return await UserManager.GenerateEmailConfirmationTokenAsync(user as TUser);
        }

        public async Task<Guid> GetUserIdAsync(object user)
        {
            var id = await UserManager.GetUserIdAsync(user as TUser);
            return Guid.Parse(id);
        }

        public async Task<IEnumerable<dynamic>> FindUsersByNameAsync(string searchValue)
        {
            var users = await UserManager.Users.Include(u => u.ProfileImage).ToListAsync();
            return users.Where(u => u.UserName.CompleteTrimAndUpper().Contains(searchValue.CompleteTrimAndUpper()));
        }

        public async Task<dynamic> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await UserManager.GetUserAsync(claimsPrincipal);
        }

        public IdentityOptions IdentityOptions()
        {
            return UserManager.Options;
        }

        public async Task<string> GetUserNameAsync(dynamic user)
        {
            return await UserManager.GetUserNameAsync(user);
        }

        public async Task<string> GetPhoneNumberAsync(dynamic user)
        {
            return await UserManager.GetPhoneNumberAsync(user);
        }

        public string GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            return UserManager.GetUserId(claimsPrincipal);
        }

        public async Task<IdentityResult> SetPhoneNumberAsync(dynamic user, string phoneNumber)
        {
            return await UserManager.SetPhoneNumberAsync(user, phoneNumber);
        }

        public async Task<IdentityResult> AddToRoleAsync(dynamic user, string role)
        {
            return await UserManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> AddToRolesAsync(dynamic user, IEnumerable<string> roles)
        {
            return await UserManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(dynamic user, string role)
        {
            return await UserManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<List<string>> GetRolesAsync(dynamic user)
        {
            return await UserManager.GetRolesAsync(user);
        }

        public async Task<dynamic> FindByIdAsync(string id)
        {
            return await UserManager.Users.Include(u => u.ProfileImage).FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));
        }

        public async Task<IdentityResult> DeleteAsync(dynamic user)
        {
            return await UserManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(dynamic user, string code)
        {
            return await UserManager.ConfirmEmailAsync(user, code);
        }

        public async Task<IdentityResult> SetUserNameAsync(dynamic user, string username)
        {
            return await UserManager.SetUserNameAsync(user, username);
        }

        public async Task<IdentityResult> ChangeEmailAsync(dynamic user, string email, string code)
        {
            return await UserManager.ChangeEmailAsync(user, email, code);
        }

        public async Task<KerykeionDbResult> AddLoginAsync(dynamic user, ExternalLoginInfo info)
        {
            IdentityResult result = await UserManager.AddLoginAsync(user, info);
            if (result.Succeeded)
            {
                return KerykeionDbResult.Success(user);
            }

            return KerykeionDbResultFilledWithErrors(result.Errors);
        }

        private TUser ActivateUser(string name, string email)
        {
            var user = Activator.CreateInstance<TUser>();
            user.Email = email;
            user.UserName = name;
            return user;
        }

        private KerykeionDbResult KerykeionDbResultAfterTryCreateUser(IdentityResult result, TUser user)
        {
            if (result.Succeeded)
            {
                return KerykeionDbResult.Success(user);
            }

            return KerykeionDbResultFilledWithErrors(result.Errors);
        }

        private KerykeionDbResult KerykeionDbResultFilledWithErrors(IEnumerable<IdentityError> identityErrors)
        {
            var errors = new List<KerykeionDbError>();
            foreach (var error in identityErrors)
            {
                errors.Add(new KerykeionDbError { Message = error.Description });
            }
            return KerykeionDbResult.Fail(errors.ToArray());
        }

        public async Task<dynamic> FindByEmailAsync(string email)
        {
            return await UserManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ResetPasswordAsync(dynamic user, string code, string password)
        {
            return await UserManager.ResetPasswordAsync(user, code, password);
        }

        public async Task<bool> IsEmailConfirmedAsync(dynamic user)
        {
            return await UserManager.IsEmailConfirmedAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(dynamic user)
        {
            return await UserManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string> GetEmailAsync(dynamic user)
        {
            return await UserManager.GetEmailAsync(user);
        }

        public async Task<string> GenerateChangeEmailTokenAsync(dynamic user, string newEmail)
        {
            return await UserManager.GenerateChangeEmailTokenAsync(user, newEmail);
        }

        public async Task<bool> HasPasswordAsync(dynamic user)
        {
            return await UserManager.HasPasswordAsync(user);
        }

        public async Task<IdentityResult> AddPasswordAsync(dynamic user, string newPassword)
        {
            return await UserManager.AddPasswordAsync(user, newPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(dynamic user, string oldPassword, string newPassword)
        {
            return await UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<string> GetAuthenticatorKeyAsync(dynamic user)
        {
            return await UserManager.GetAuthenticatorKeyAsync(user);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(dynamic user)
        {
            return await UserManager.GetTwoFactorEnabledAsync(user);
        }

        public async Task<int> CountRecoveryCodesAsync(dynamic user)
        {
            return await UserManager.CountRecoveryCodesAsync(user);
        }

        public async Task<bool> VerifyTwoFactorTokenAsync(dynamic user, string authenticatorTokenProvider, string verificationCode)
        {
            return await UserManager.VerifyTwoFactorTokenAsync(user, authenticatorTokenProvider, verificationCode);
        }

        public async Task<IdentityResult> SetTwoFactorEnabledAsync(dynamic user, bool enabled)
        {
            return await UserManager.SetTwoFactorEnabledAsync(user, enabled);
        }

        public async Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(dynamic user, int number)
        {
            return await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, number);
        }

        public async Task<IdentityResult> ResetAuthenticatorKeyAsync(dynamic user)
        {
            return await UserManager.ResetAuthenticatorKeyAsync(user);
        }

        public async Task<string> GetUserProfileImageUrl(dynamic user)
        {
            var currentUser = await GetUserProfileImageIncluded(user);
            return currentUser?.ProfileImage?.Url ?? FolderConstants.DefaultUserImagePath;
        }

        public async Task<KerykeionDbResult> AddProfileImage(dynamic user, IFormFile profileImage)
        {
            if (!(await GetUserProfileImageIncluded(user) is TUser currentUser))
            {
                return KerykeionDbResult.Fail();
            }

            var errors = new List<KerykeionDbError>();

            if (currentUser.ProfileImage != null)
            {
                //var delImgResult = await EntitiesService.DeleteAsync(currentUser.ProfileImage, FolderConstants.UserProfileImagesFolderName);
                //if (!delImgResult.Successfull)
                //{
                //    return delImgResult;
                //}
            }

            if (await AddProfileImageToUserAndCheckCompletion(profileImage, currentUser))
            {
                var updateUserResult = await UserManager.UpdateAsync(currentUser);
                if (updateUserResult.Succeeded)
                {
                    return KerykeionDbResult.Success();
                }
                foreach (var error in updateUserResult.Errors)
                {
                    errors.Add(new KerykeionDbError { Message = error.Description });
                }
            }

            errors.Add(new KerykeionDbError { Message = "Het uploaden van de afbeelding is mislukt." });
            return KerykeionDbResult.Fail(errors.ToArray());
        }

        public async Task<dynamic> GetUserProfileImageIncluded(dynamic user)
        {
            Guid userId = user.Id;
            return await UserManager.Users.Include(u => u.ProfileImage).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(dynamic user)
        {
            return await UserManager.GetLoginsAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(dynamic user, string password)
        {
            return await UserManager.CheckPasswordAsync(user, password);
        }

        private async Task<bool> AddProfileImageToUserAndCheckCompletion(IFormFile file, TUser user)
        {
            var imageResult = await _imageService.SaveImage(file, FolderConstants.UserProfileImagesFolderName);
            if (imageResult.Success)
            {
                Image image = new Image
                {
                    Url = $"/{FolderConstants.UserProfileImagesFolderName}/{imageResult.ImgUrl}",
                    Name = $"{user.UserName}-Profile-Image"
                };

                var addImgResult = await EntitiesService.CreateAsync(image);
                if (addImgResult.Successfull)
                {
                    user.ProfileImage = image;
                    return true;
                }
            }
            return false;
        }

        public async Task<string> GetLanguageAsync(dynamic user)
        {
            var thisUser = await UserManager.FindByIdAsync(user.Id.ToString());
            if (thisUser != null)
            {
                return thisUser.Language.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<KerykeionDbResult> SetLanguageAsync(dynamic user, string language)
        {
            KerykeionUser thisUser = await UserManager.FindByIdAsync(user.Id.ToString());
            if (thisUser != null)
            {
                user.Language = Enum.Parse<KerykeionCmsLanguage>(language);
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return KerykeionDbResult.Success();
                }
            }

            return KerykeionDbResult.Fail();

        }
    }
}
