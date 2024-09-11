using AutoMapper;
using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos.UserDtos;
using JobPortal.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Yeni bir kullanıcı kaydı yapar.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var serviceResult = await _userService.RegisterUserAsync(userRegisterDto);
            var response = new ResponseModel
            {
                Message = serviceResult.Message,
                Data = serviceResult.Data,
                StatusCode = (int)serviceResult.ResultCode
            };

            if (serviceResult.IsSuccess())
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Kullanıcıyı giriş yapar.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var serviceResult = await _userService.LoginUserAsync(userLoginDto);
            var response = new ResponseModel
            {
                Message = serviceResult.Message,
                Data = serviceResult.Data,
                StatusCode = (int)serviceResult.ResultCode
            };

            if (serviceResult.IsSuccess())
            {
                return Ok(response);
            }
            return Unauthorized(response);
        }

        /// <summary>
        /// Kullanıcının şifresini yeniler.
        /// </summary>
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var serviceResult = await _userService.ResetPasswordAsync(resetPasswordDto);
            var response = new ResponseModel
            {
                Message = serviceResult.Message,
                Data = serviceResult.Data,
                StatusCode = (int)serviceResult.ResultCode
            };

            if (serviceResult.IsSuccess())
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Tüm kullanıcıları getirir.
        /// </summary>
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> ListUsers()
        {
            var serviceResult = await _userService.GetAllUsersAsync();
            var response = new ResponseModel
            {
                Message = serviceResult.Message,
                Data = serviceResult.Data,
                StatusCode = (int)serviceResult.ResultCode
            };

            return Ok(response);
        }

        /// <summary>
        /// Belirtilen kullanıcıyı siler.
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var serviceResult = await _userService.DeleteUserAsync(id);
            var response = new ResponseModel
            {
                Message = serviceResult.Message,
                Data = serviceResult.Data,
                StatusCode = (int)serviceResult.ResultCode
            };

            if (serviceResult.IsSuccess())
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
