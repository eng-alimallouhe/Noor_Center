using AutoMapper;
using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Application.DTOs.ResponseDTOs;
using LMS.Domain.Entities.Users;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Specifications;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Application.Services.Users.Authentication
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TokenReaderService _tokenReaderService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;   
        private readonly IUserRepository _userRepository;

        public TokenService(
            IConfiguration configuration,
            IUserRepository userRepository,
            IMapper mapper,
            TokenReaderService tokenReaderService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenReaderService = tokenReaderService;
            _mapper = mapper;
            _refreshTokenRepository = refreshTokenRepository;
        }

        /// <summary>
        /// Generates an access token (JWT) for a given user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>A JWT access token as a string.</returns>
        /// <remarks>
        /// This method creates a JWT token containing user-related claims such as UserId, Email, and Role.
        /// The token is signed using HMAC SHA256 and expires after 30 minutes.
        /// </remarks>
        public string GenerateAccessToken(User user)
        {
            // Create a security key from the configured secret key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

            // Generate signing credentials using HMAC SHA256 algorithm
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define user claims to be included in the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), // User ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // User email
                new Claim(ClaimTypes.Role, user.Role.RoleType), // User role
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier for the token
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Token expiration time
                signingCredentials: credentials
            );

            // Serialize and return the token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates a refresh token for a given user.
        /// </summary>
        /// <param name="user">The user for whom the refresh token is generated.</param>
        /// <returns>A result indicating success or failure of refresh token generation.</returns>
        /// <remarks>
        /// This method creates a cryptographically secure random refresh token, stores it in the database, 
        /// and sets its expiration date to 7 days.
        /// </remarks>
        public async Task<Result<string>> GenerateRefreshTokenAsync(User user)
        {
            // Ensure the user exists before generating a token
            if (user == null)
            {
                return Result<string>.Failure("User not found");
            }

            // Create a 64-byte secure random token
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Create a new refresh token object
            var token = new RefreshToken
            {
                UserId = user.UserId,
                Token = Convert.ToBase64String(randomBytes), // Convert to Base64 string
                Expiration = DateTime.UtcNow.AddDays(7), // Token valid for 7 days
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                // Remove any previous refresh token for this user
                await _refreshTokenRepository.DeleteAsync(user.UserId);
            }
            catch
            {
                Console.WriteLine("Error");
            }

            try
            {
                // Store the new refresh token in the database
                await _refreshTokenRepository.AddAsync(token);
            }
            catch
            {
                return Result<string>.Failure("can't add the token");
            }

            return Result<string>.Success("Refresh token generated successfully", token.Token);
        }

        /// <summary>
        /// Checks if the user has a valid refresh token.
        /// </summary>
        /// <param name="user">The user whose refresh token needs verification.</param>
        /// <returns>A result indicating whether the refresh token is valid or expired.</returns>
        /// <remarks>
        /// If the refresh token is expired, a new refresh token is generated.
        /// </remarks>
        public async Task<Result<AuthenticationResponseDTO>> RefreshAuthorization(AuthorizationRequestDTO authorization)
        {
            var userId = _tokenReaderService.GetUserId(authorization.AccessToken);

            if (userId == null)
            {
                return Result<AuthenticationResponseDTO>.Failure("access token is unvalid");
            }

            var user = await _userRepository.GetByCriteriaAsync(new Specification<User>(
                criteria: us => us.UserId == Guid.Parse(userId),
                includes: [us => us.Role, us => us.Notifications]
            ));

            if (user == null)
            {
                return Result<AuthenticationResponseDTO>.Failure("user not founded");
            }

            var refreshToken = await _refreshTokenRepository.GetByUserIdAsync(Guid.Parse(userId));

            if (refreshToken == null)
            {
                return Result<AuthenticationResponseDTO>.Failure("Refresh token not found");
            }

            if (refreshToken.Token != authorization.RefreshToken)
            {
                return Result<AuthenticationResponseDTO>.Failure("refresh token is invalid");
            }

            if (refreshToken.IsRevoked)
            {
                return Result<AuthenticationResponseDTO>.Failure("refresh token is revoked");
            }

            if (refreshToken.Expiration <= DateTime.UtcNow)
            {
                return Result<AuthenticationResponseDTO>.Failure("refresh token is expired");
            }

            var newAccessToken = GenerateAccessToken(user);

            var refreshTokenResult = await GenerateRefreshTokenAsync(user);

            if (refreshTokenResult.IsSuccess)
            {
                var userResponse = _mapper.Map<UserResponseDTO>(user);
                return Result<AuthenticationResponseDTO>.Success("Refresh token generated successfully", new AuthenticationResponseDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = refreshTokenResult.Entity!,
                    User = userResponse
                });
            }
            else
            {
                return Result<AuthenticationResponseDTO>.Failure("Error generating refresh token");
            }
        }
    }
}