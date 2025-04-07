using AutoMapper;
using LMS.Application.common;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Application.DTOs.ResponseDTOs;
using LMS.Application.Services.Users.Authentication;
using LMS.Domain.Entities.Users;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Interfaces;
using LMS.Infrastructure.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Application.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly AccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(
            IUserRepository userRepository,
            TokenService tokenService,
            IMapper mapper,
            AccountService accountService)
        {

            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponseDTO>> Login(LogInRequestDTO logIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.LogIn(logIn);

            if (!result.IsSuccess)
            {
                if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(result);
                }
                if (result.Message.Contains("account is locked", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized(result);
                }
                if (result.Message.Contains("try again.", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(result);
                }
                return Unauthorized(result);
            }
            else
            {
                var token = _tokenService.GenerateAccessToken(result.Entity!);
                var refreshTokenResult = await _tokenService.GenerateRefreshTokenAsync(result.Entity!);

                if (refreshTokenResult.IsSuccess)
                {
                    var user = _mapper.Map<UserResponseDTO>(result.Entity!);

                    return Ok(new AuthenticationResponseDTO
                    {
                        AccessToken = token,
                        RefreshToken = refreshTokenResult.Entity!,
                        User = user
                    });
                }
                else
                {
                    return BadRequest(refreshTokenResult);
                }
            }
        }

        
        [HttpPost("password-reset-code")]
        public async Task<ActionResult<Result<ResetPasswordRequestDTO>>> RequestResetPassword(ResetPasswordRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requestResult = await _accountService.RequestPasswordCodeAsync(request);

            if (!requestResult.IsSuccess)
            {
                if (requestResult.Message.Contains("account is locked", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized(requestResult);
                }

                if (requestResult.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(requestResult);
                }

                return BadRequest(requestResult);
            }
            return Ok(Result<ResetPasswordRequestDTO>.Success("code send successfully", request)); 
        }

        [HttpPost("password-reset")]
        public async Task<ActionResult<AuthenticationResponseDTO>> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (! ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resetResult = await _accountService.ResetPasswordAsync(resetPasswordDTO);

            if (! resetResult.IsSuccess)
            {
                if (resetResult.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                {
                    return NotFound(resetResult);
                }
               
                if (resetResult.Message.Contains("invalid code", StringComparison.OrdinalIgnoreCase))
                {
                    resetResult.Message = "the code is invalid, re-enter you information";
                    
                    return Unauthorized(resetResult);
                }

                return BadRequest(resetResult);
            }

            var token = _tokenService.GenerateAccessToken(resetResult.Entity!);
            
            var refreshTokenResult = await _tokenService.GenerateRefreshTokenAsync(resetResult.Entity!);
            
            if (refreshTokenResult.IsSuccess)
            {
                var user = _mapper.Map<UserResponseDTO>(resetResult.Entity!);
                
                return Ok(new AuthenticationResponseDTO
                {
                    AccessToken = token,
                    RefreshToken = refreshTokenResult.Entity!,
                    User = user
                });
            }

            else
            {
                return BadRequest(refreshTokenResult);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticationResponseDTO>> RefreshToken(AuthorizationRequestDTO authorization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _tokenService.RefreshAuthorization(authorization);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }
            
           

            else
            {
                var response = result.Entity!;
                return Ok(response);
            }
        }
    }
}