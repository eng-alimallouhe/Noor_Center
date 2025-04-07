using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Domain.Entities.Users;
using LMS.Domain.Enums.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Repositories.Users;
using LMS.Infrastructure.Specifications;
using System.Security.Cryptography;

namespace LMS.Application.Services.Users.Authentication
{
    public class CodeService
    {
        private readonly string templatesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<OtpCode> _codeRepository;

        public CodeService(
            IEmailService emailService,
            IUserRepository userRepository,
            IRepository<OtpCode> otpCodeRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _codeRepository = otpCodeRepository;
        }

        /// <summary>
        /// Sends a verification OTP code to the specified email.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="type">The purpose of the OTP code (e.g., SignUp, ResetPassword).</param>
        /// <returns>A result containing the generated OTP code if successful, otherwise an error message.</returns>
        /// <remarks>
        /// This method checks if the email exists in the system, generates a new verification code,
        /// and sends it via email using the appropriate HTML template based on the <paramref name="type"/>.
        /// If there is an existing OTP code, it will be deleted and replaced with the new one.
        /// The generated OTP code will be hashed and stored in the repository with an expiration time of 10 minutes.
        /// </remarks>
        public async Task<Result<OtpCode>> SendCodeAsync(string email, Purpose type)
        {
            try
            {
                // Construct the template path based on the OTP purpose
                string templatePath = Path.Combine(templatesDirectory, $"{type.ToString()}.html");

                // Check if the email template exists
                if (!File.Exists(templatePath))
                {
                    return Result<OtpCode>.Failure("Template not found.");
                }

                // Retrieve the user based on the provided email, including their OTP code
                var user = await _userRepository.GetByCriteriaAsync(new Specification<User>(
                    criteria: user => user.Email.ToLower() == email.ToLower(),
                    includes: [user => user.OtpCode]
                ));

                // Return failure if the user does not exist
                if (user == null)
                {
                    return Result<OtpCode>.Failure("User not found.");
                }

                var otpcode = user.OtpCode;

                // Delete the existing OTP code if it exists
                if (otpcode != null)
                {
                    try
                    {
                        await _codeRepository.DeleteHardlyAsync(otpcode.OtpCodeId);
                    }
                    catch (Exception ex)
                    {
                        return Result<OtpCode>.Failure($"Error: {ex.Message}");
                    }
                }

                var username = user.UserName;

                // Generate a new OTP code
                var code = GeneratedCode();

                // Read the email template
                string emailTemplate = await File.ReadAllTextAsync(templatePath);

                // Replace placeholders with actual values
                emailTemplate = emailTemplate.Replace("{{name}}", username)
                                              .Replace("{{code}}", code);

                try
                {
                    // Send the OTP code via email
                    await _emailService.SendEmailAsync(email, "Verify Email", emailTemplate);
                }
                catch (Exception ex)
                {
                    return Result<OtpCode>.Failure($"Error while sending the email: {ex.Message}");
                }

                // Create a new OTP code entity
                var otpCode = new OtpCode()
                {
                    HashedValue = code,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(10),
                    IsUsed = false,
                    FailedAttempts = 0,
                    UserId = user.UserId,
                    CodeType = CodeType.SignUp,
                    User = user
                };
                try
                {
                    // Save the OTP code to the database
                    await _codeRepository.AddAsync(otpCode);
                }
                catch (Exception ex)
                {
                    return Result<OtpCode>.Failure($"Error: {ex.Message}");
                }

                return Result<OtpCode>.Success("Verification code sent to email", otpCode);
            }
            catch (Exception ex)
            {
                return Result<OtpCode>.Failure($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// Verifies the provided OTP code for the given email.
        /// </summary>
        /// <param name="otpCodeDTO">DTO containing the email and the OTP code to verify.</param>
        /// <returns>A result indicating whether the OTP verification was successful.</returns>
        /// <remarks>
        /// This method checks the following conditions:
        /// 1. If the user exists in the system based on the provided email.
        /// 2. If the OTP code exists, has not been used, and has not expired.
        /// 3. If the number of failed attempts is less than 3; otherwise, the OTP is discarded.
        /// 4. If the entered OTP code matches the stored hashed value.
        /// Upon successful verification, the OTP code will be marked as used and deleted.
        /// </remarks>
        public async Task<Result<OtpCode>> VerifyOtpCodeAsync(OtpCodeDTO otpCodeDTO)
        {
            string email = otpCodeDTO.Email;
            string code = otpCodeDTO.Code;

            // Retrieve the user along with their OTP code based on the provided email
            var user = await _userRepository.GetByCriteriaAsync(new Specification<User>(
                user => user.Email.ToLower() == email.ToLower(),
                includes: [user => user.OtpCode]
            ));

            // Return failure if the user does not exist
            if (user == null)
            {
                return Result<OtpCode>.Failure("User not found.");
            }

            var otpCode = user.OtpCode;

            // Return failure if no OTP code is found
            if (otpCode == null)
            {
                return Result<OtpCode>.Failure("OTP code not found.");
            }

            // Check if the OTP code has already been used
            if (otpCode.IsUsed)
            {
                return Result<OtpCode>.Failure("This OTP code has already been used.");
            }

            // If the OTP code has expired or reached the maximum allowed attempts, delete it
            if (otpCode.ExpiredAt < DateTime.UtcNow || otpCode.FailedAttempts >= 3)
            {
                try
                {
                    await _codeRepository.DeleteHardlyAsync(otpCode.OtpCodeId);
                }
                catch (Exception ex)
                {
                    return Result<OtpCode>.Failure($"Error: {ex.Message}");
                }
                return Result<OtpCode>.Failure("Invalid code.");
            }

            // Verify the OTP code against the hashed value
            if (code != otpCode.HashedValue)
            {
                otpCode.FailedAttempts += 1;
                var remainingAttempts = 3 - otpCode.FailedAttempts;
                try
                {
                    await _codeRepository.UpdateAsync(otpCode);
                }
                catch (Exception ex)
                {
                    return Result<OtpCode>.Failure($"Error: {ex.Message}");
                }
                return Result<OtpCode>.Failure($"Incorrect OTP code. You have {remainingAttempts} attempts remaining.");
            }

            //delete OTP code after successful verification
            try
            {
                await _codeRepository.DeleteHardlyAsync(otpCode.OtpCodeId);
            }
            catch (Exception ex)
            {
                return Result<OtpCode>.Failure($"Error: {ex.Message}");
            }

            return Result<OtpCode>.Success("OTP code verified successfully.", otpCode);
        }

        //generate the otp code contain 6-digits:
        private string GeneratedCode()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[6];
                rng.GetBytes(randomBytes);

                int code = BitConverter.ToInt32(randomBytes, 0) % 1000000;
                return Math.Abs(code).ToString("D6");
            }
        }

    }

    public enum Purpose
    {
        AccountVerificationTemplate,
        PasswordResetTemplate,
        RegistrationVerificationTemplate
    }
}
