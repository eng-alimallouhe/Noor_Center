using AutoMapper;
using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Domain.Entities.Financial;
using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Specifications;

namespace LMS.Application.Services.Users.Authentication
{
    public class RegisterService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<LoyaltyLevel> _levelRepository;
        private readonly CodeService _codeService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RegisterService(
            IRepository<Customer> customerRepository,
            IRepository<Role> roleRepository,
            CodeService codeService,
            IUserRepository userRepository,
            IRepository<LoyaltyLevel> levelRepository,
            IMapper mapper) 
        {
            _customerRepository = customerRepository;
            _roleRepository = roleRepository;
            _levelRepository = levelRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _codeService = codeService;
        }



        public async Task<Result<string>> VerifyEmail(string email)
        {
            // Check if the email already exists in the database
            var result = await _customerRepository.GetByCriteriaAsync(new Specification<Customer>(
                criteria: c => c.Email.ToLower() == email.ToLower()
            ));

            if (result != null)
            {
                return Result<string>.Failure("Email already exists.");
            }

            // If the email is available, return success
            return Result<string>.Success("Email is available.", email);
        }


        public async Task<Result<string>> VerifyUserName(string username)
        {
            // Check if the username already exists in the database
            var result = await _customerRepository.GetByCriteriaAsync(new Specification<Customer>(
                criteria: c => c.UserName.ToLower().Trim() == username.ToLower().Trim()
            ));
            if (result != null)
            {
                return Result<string>.Failure("Username already exists.");
            }

            // If the username is available, return success
            return Result<string>.Success("Username is available.", username);
        }

        /// <summary>
        /// Registers a new customer and creates a temporary account.
        /// </summary>
        /// <param name="register">The registration data of the user.</param>
        /// <returns>A result containing the created customer if successful, otherwise an error message.</returns>
        /// <remarks>
        /// This method checks if the user already exists based on email or username.
        /// If the user does not exist, it assigns the default role (Customer) and loyalty level (Bronze),
        /// hashes the password, creates the customer, and stores it temporarily until verification.
        /// </remarks>
        public async Task<Result<RegisterRequestDTO>> Register(RegisterRequestDTO register)
        {
            // Check if the user is already registered by email or username
            var existingCustomer = await _customerRepository.GetByCriteriaAsync(new Specification<Customer>(
                criteria: c => c.Email.ToLower() == register.Email.ToLower() ||
                                c.UserName == register.UserName
            ));

            if (existingCustomer != null)
            {
                return Result<RegisterRequestDTO>.Failure("User already exists.");
            }

            // Retrieve the default role for customers
            var role = await _roleRepository.GetByCriteriaAsync(new Specification<Role>(
                criteria: r => r.RoleType.ToLower() == "customer"
            ));

            if (role == null)
            {
                return Result<RegisterRequestDTO>.Failure("Default customer role not found.");
            }

            // Retrieve the default loyalty level
            var level = await _levelRepository.GetByCriteriaAsync(new Specification<LoyaltyLevel>(
                criteria: l => l.LevelName.ToLower() == "Newbie Nester".ToLower()
            ));

            if (level == null)
            {
                return Result<RegisterRequestDTO>.Failure("Initial loyalty level not found.");
            }

            // Map DTO to Customer entity
            var newCustomer = _mapper.Map<Customer>(register);
            newCustomer.RoleId = role.RoleId;
            newCustomer.Role = role;
            newCustomer.LevelId = level.LevelId;
            newCustomer.Level = level;

            // Hash the password before storing it
            newCustomer.Password = BCrypt.Net.BCrypt.HashPassword(register.Password);

            // Save the temporary customer
            try
            {
                await _customerRepository.AddAsync(newCustomer);
                await _codeService.SendCodeAsync(register.Email, Purpose.RegistrationVerificationTemplate);
            }
            catch (Exception ex)
            {
                return Result<RegisterRequestDTO>.Failure($"An error occurred while creating the user: {ex.Message}");
            }

            return Result<RegisterRequestDTO>.Success("Verification code has been sent successfully.", register);
        }



        /// <summary>
        /// Verifies the registration of a customer using the OTP code.
        /// </summary>
        /// <param name="codeDTO">The DTO containing the user's email and OTP code.</param>
        /// <returns>A result indicating whether the verification was successful or failed.</returns>
        /// <remarks>
        /// This method verifies the provided OTP code. If the code is expired or the maximum number of 
        /// failed attempts is reached, the temporary account is deleted for privacy reasons. 
        /// If the code is valid, the account is marked as verified.
        /// </remarks>
        public async Task<Result<User>> VerifyRegister(OtpCodeDTO codeDTO)
        {
            // Retrieve user by email
            var user = await _userRepository.GetByCriteriaAsync(new Specification<User>(
                criteria: us => us.Email.ToLower() == codeDTO.Email.ToLower(),
                includes: [us => us.Role, us => us.Notifications]
            ));

            if (user == null)
            {
                return Result<User>.Failure("User not found.");
            }

            // Verify OTP code
            var verifyResult = await _codeService.VerifyOtpCodeAsync(codeDTO);

            if (!verifyResult.IsSuccess)
            {
                // If code is invalid or expired, delete the temporary account
                if (verifyResult.Message.Contains("Invalid code", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        await _customerRepository.DeleteHardlyAsync(user.UserId);
                        return Result<User>.Failure("The verification code is invalid. Please request a new one.");
                    }
                    catch (Exception ex)
                    {
                        return Result<User>.Failure($"An error occurred while deleting the account: {ex.Message}");
                    }
                }

                // If the code is incorrect but there are remaining attempts
                return Result<User>.Failure(verifyResult.Message);
            }

            // Mark account as verified
            user.IsVerified = true;

            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"An error occurred while verifying the user: {ex.Message}");
            }

            return Result<User>.Success("User verified successfully.", user);
        }
    }
}