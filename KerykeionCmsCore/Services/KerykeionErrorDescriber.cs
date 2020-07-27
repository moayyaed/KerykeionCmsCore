using KerykeionCmsCore.Constants;
using Microsoft.AspNetCore.Identity;

namespace KerykeionCmsCore.Services
{
    public class KerykeionErrorDescriber : IdentityErrorDescriber
    {
        private readonly KerykeionTranslationsService _translationsService;
        public KerykeionErrorDescriber(KerykeionTranslationsService translationsService)
        {
            _translationsService = translationsService;
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.DefaultError, "Identity system error")
            };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.ConcurrencyFailure, "Concurrency failure.")
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.PasswordMismatch, "Password mismatch.")
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.InvalidToken, "Invalid token.")
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.RecoveryCodeRedemptionFailed, "Recovery code was not redeemed.")
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.LoginAlreadyAssociated, "An external login is already associated with this account.")
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.InvalidUserName, $"User name '{userName}' is invalid.", userName)
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.InvalidEmail, $"Email '{email}' is invalid.", email)
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.DuplicateUserName, $"User name '{userName}' is already taken.", userName)
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.InvalidRoleName, $"Role name '{role}' is invalid.", role)
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.DuplicateRoleName, $"Role name '{role}' is already taken.", role)
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.UserAlreadyHasPassword, "User already has a password.")
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.UserLockoutNotEnabled, "User lockout is not enabled.")
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.UserAlreadyInRole, $"The user is already in '{role}' role.", role)
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.UserNotInRole, $"The user is not in '{role}' role.", role)
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.PasswordTooShort, $"The password must be at least '{length}' characters long.", length.ToString())
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.PasswordRequiresAtleastAmountUniqueChars, $"The password must contain '{uniqueChars}' unique characters.", uniqueChars.ToString())
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.PasswordRequiresNonAlphanumeric, "Passwords must have at least one non alphanumeric character.")
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.PasswordRequiresDigit, "Passwords must have at least one digit ('0'-'9').")
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.PasswordRequiresLower, "Passwords must have at least one lowercase ('a'-'z').")
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = _translationsService.TranslateError(ErrorDescriberConstants.PasswordRequiresUpper, "Passwords must have at least one uppercase ('A'-'Z').")
            };
        }
    }
}
