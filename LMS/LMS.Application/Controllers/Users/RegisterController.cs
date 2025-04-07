using AutoMapper;
using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Application.DTOs.ResponseDTOs;
using LMS.Application.Services.Users.Authentication;
using LMS.Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Application.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly RegisterService _registerService;
        private readonly IMapper _mapper;

        public RegisterController(
            TokenService tokenService,
            IMapper mapper,
            RegisterService registerService)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _registerService = registerService;
        }

        [HttpGet("verify-email/{email}")]
        public async Task<ActionResult<Result<string>>> VerifyEmail(string email)
        {
            var result = await _registerService.VerifyEmail(email);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("verify-username/{username}")]
        public async Task<ActionResult<Result<string>>> VerifyUserName(string username)
        {
            var result = await _registerService.VerifyUserName(username);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<RegisterRequestDTO>>> Register(RegisterRequestDTO register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _registerService.Register(register);

            if (! registerResult.IsSuccess)
            {
                return BadRequest(registerResult);
            }
            return Ok(registerResult);
        }

        [HttpPost("verify-register")]
        public async Task<ActionResult<AuthenticationResponseDTO>> VerifyRegister(OtpCodeDTO otpCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifyResult = await _registerService.VerifyRegister(otpCode);

            if (!verifyResult.IsSuccess)
            {
                if (verifyResult.Message.Contains("please request", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized(Result<AuthenticationResponseDTO>.Failure("the code is invalid, re-enter you information"));
                }

                return BadRequest(verifyResult);
            }

            if (verifyResult.Entity == null)
            {
                return BadRequest(Result<User>.Failure("invalid information"));
            }

            var accessToken = _tokenService.GenerateAccessToken(verifyResult.Entity);
            var refreshTokenResult = await _tokenService.GenerateRefreshTokenAsync(verifyResult.Entity);

            if (! refreshTokenResult.IsSuccess || refreshTokenResult.Entity == null)
            {
                return Unauthorized(refreshTokenResult);
            }

            var refreshtoken = refreshTokenResult.Entity;

            var user = _mapper.Map<UserResponseDTO>(verifyResult.Entity);

            return Ok(new AuthenticationResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshtoken,
                    User = user
                });
        }
    }
}