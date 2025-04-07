using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Domain.Entities.Users;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Specifications;

namespace LMS.Application.Services.Users.Authentication
{
    public class AccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly CodeService _codeService;

        public AccountService(
            IUserRepository userRepository,
            CodeService codeService)
        {
            _userRepository = userRepository;
            _codeService = codeService;
        }


        /// <summary>
        /// Authenticates a user and manages failed login attempts.
        /// </summary>
        /// <param name="logIn">The login request containing the user's email and password.</param>
        /// <returns>A result containing the authenticated user if successful, otherwise an error message.</returns>
        /// <remarks>
        /// This method verifies the provided email and password. If the email does not exist, 
        /// an error is returned. If the password is incorrect, the failed login attempts increase.
        /// After 5 failed attempts, the user account is locked. Upon successful authentication, 
        /// the failed attempts counter is reset.
        /// </remarks>
        public async Task<Result<User>> LogIn(LogInRequestDTO logIn)
        {
            // Retrieve user by email
            var user = await _userRepository.GetByCriteriaAsync(new Specification<User>(
                criteria: us => us.Email.ToLower().Trim() == logIn.Email.ToLower().Trim(),
                includes: [us => us.Role, us => us.Notifications]
            ));

            if (user == null)
            {
                return Result<User>.Failure("User not found.");
            }

            if (user.IsDeleted)
            {
                return Result<User>.Failure("User not found.");
            }

            if (user.IsLocked && user.LockedUntil > DateTime.UtcNow)
            {
                return Result<User>.Failure($"account is locked and will be accessable in {user.LockedUntil}");
            }

            // Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(logIn.Password, user.Password);

            if (!isPasswordValid)
            {
                // Lock the account if failed attempts exceed the limit
                if (user.FailedLoginAttempts >= 5)
                {
                    user.IsLocked = true;

                    user.LockedUntil = DateTime.UtcNow.AddMinutes(15);

                    try
                    {
                        await _userRepository.UpdateAsync(user);
                        return Result<User>.Failure("User account is locked for 15 minutes due to multiple failed login attempts.");
                    }
                    catch (Exception ex)
                    {
                        return Result<User>.Failure($"Error while locking account: {ex.Message}");
                    }
                }

                // Increment failed login attempts
                user.FailedLoginAttempts += 1;
                
                try
                {
                    await _userRepository.UpdateAsync(user);
                }

                catch (Exception ex)
                {
                    return Result<User>.Failure($"Error updating failed login attempts: {ex.Message}");
                }

                return Result<User>.Failure("Invalid password. Please try again.");
            }

            // Reset failed attempts on successful login
            user.FailedLoginAttempts = 0;
            user.LastLogIn = DateTime.UtcNow;
            user.IsLocked = false;
            user.LockedUntil = null;

            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Error resetting failed attempts: {ex.Message}");
            }

            return Result<User>.Success("Login successful.", user);
        }

        /// <summary>
        /// Sends a password reset code to the user's registered email.
        /// </summary>
        /// <param name="resetPassword">The DTO containing the user's email for password reset.</param>
        /// <returns>A result indicating whether the reset code was successfully sent.</returns>
        /// <remarks>
        /// This method checks if the provided email exists in the system. If found, 
        /// a reset password code is generated and sent via email. 
        /// If the email does not exist, an error message is returned.
        /// </remarks>
        public async Task<Result<User>> RequestPasswordCodeAsync(ResetPasswordRequestDTO resetPassword)
        {
            try
            {
                // Retrieve user by email
                var user = await _userRepository.GetByCriteriaAsync(new Specification<User>(
                    criteria: us => us.Email.ToLower().Trim() == resetPassword.Email.ToLower().Trim()
                ));

                if (user == null)
                {
                    return Result<User>.Failure("User not found.");
                }

                if (user.IsDeleted)
                {
                    return Result<User>.Failure("User not found.");
                }

                // Check if the user is locked
                if(user.IsLocked && user.LockedUntil < DateTime.UtcNow)
                {
                    return Result<User>.Failure($"Account is locked and will be accessible in {user.LockedUntil}");
                }

                // Send reset password code
                var result = await _codeService.SendCodeAsync(resetPassword.Email, Purpose.PasswordResetTemplate);

                if (!result.IsSuccess)
                {
                    return Result<User>.Failure($"Failed to send reset code: {result.Message}");
                }

                return Result<User>.Success("Code sent successfully.", user);
            }
            catch (Exception ex)
            {   
                return Result<User>.Failure($"An error occurred: {ex.Message}");
            }
        }
   
        public async Task<Result<User>> ResetPasswordAsync(ResetPasswordDTO reset)
        {
            // Retrieve user by email
            var user = await _userRepository.GetByCriteriaAsync(new Specification<User>(
                criteria: us => us.Email.ToLower().Trim() == reset.Email.ToLower().Trim(),
                includes: [us => us.Role, us => us.Notifications]
            ));

            if (user == null)
            {
                return Result<User>.Failure("User not found.");
            }        


            var verifyResult = await _codeService.VerifyOtpCodeAsync(new OtpCodeDTO
            {
                Email = reset.Email,
                Code = reset.Code
            });

            if (!verifyResult.IsSuccess)
            {
                // If code is invalid or expired, delete the temporary account
                if (verifyResult.Message.Contains("invalid code", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        return Result<User>.Failure("The verification code is invalid. Please request a new one.");
                    }
                    catch (Exception ex)
                    {
                        return Result<User>.Failure($"An error occurred while deleting the account: {ex.Message}");
                    }
                }
                // If the code is incorrect but there are remaining attempts
                return Result<User>.Failure("Incorrect code. Please try again.");
            }

            //check if the new password and new password is the same password: 
            var isSamePassword = BCrypt.Net.BCrypt.Verify(reset.Password, user.Password);

            if (isSamePassword)
            {
                return Result<User>.Failure("use another password");
            }

            //if not the same password encrypt the new password then asign it to the targate account: 
            user.Password = BCrypt.Net.BCrypt.HashPassword(reset.Password);

            //try to update the account: 
            try
            {
                await _userRepository.UpdateAsync(user);
                return Result<User>.Success("the password updated successfully", user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"An error occurred while updating the account: {ex.Message}");
            }
        }
    }
}
