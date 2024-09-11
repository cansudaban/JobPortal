using AutoMapper;
using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos.UserDtos;
using JobPortal.Common.Helpers;
using JobPortal.Common.ServiceResultManager;
using JobPortal.Data.GenericRepository;
using JobPortal.Data.Models;
using JobPortal.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobPortal.Business.Classes
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userRepo = _unitOfWork.GenericRepository<User>();
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResult> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("PasswordsDoNotMatch"));
            }

            var existingUser = await _userRepo.GetAsync(u => u.Email == userRegisterDto.Email && !u.IsDeleted);
            if (existingUser != null)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("EmailAlreadyExists"));
            }

            var hashedPassword = PasswordHelper.HashPassword(userRegisterDto.Password);

            var user = new User
            {
                Name = userRegisterDto.Name,
                Email = userRegisterDto.Email,
                Password = hashedPassword
            };

            await _userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("UserRegisteredSuccess"), userDto);
        }

        public async Task<ServiceResult> LoginUserAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepo.GetAsync(u => u.Email == userLoginDto.Email && !u.IsDeleted);
            if (user == null || !PasswordHelper.VerifyPassword(userLoginDto.Password, user.Password))
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("InvalidEmailOrPassword"));
            }

            var userDto = _mapper.Map<UserDto>(user);
            var token = GenerateJwtToken(user);
            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("LoginSuccess"), new { User = userDto, Token = token });

        }

        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("PasswordsDoNotMatch"));
            }

            var user = await _userRepo.GetAsync(u => u.Email == resetPasswordDto.Email && !u.IsDeleted);
            if (user == null)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("UserNotFound"));
            }

            user.Password = PasswordHelper.HashPassword(resetPasswordDto.NewPassword);
            _userRepo.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("PasswordResetSuccess"));
        }

        public async Task<ServiceResult> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("UsersRetrievedSuccess"), userDtos);
        }

        public async Task<ServiceResult> DeleteUserAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("UserNotFound"));
            }

            _userRepo.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("UserDeletedSuccess"));
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:TokenExpiryInMinutes"])),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
