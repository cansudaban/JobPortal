using AutoMapper;
using JobPortal.Business.Classes;
using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos.UserDtos;
using JobPortal.Common.Helpers;
using JobPortal.Data.GenericRepository;
using JobPortal.Data.Models;
using JobPortal.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Moq;

namespace JobPortal.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGenericRepository<User>> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IUserService _userService;
        private readonly Mock<IConfiguration> _configMock;

        public UserServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepoMock = new Mock<IGenericRepository<User>>();
            _mapperMock = new Mock<IMapper>();
            _configMock = new Mock<IConfiguration>();

            _configMock.Setup(c => c["JwtSettings:SecretKey"]).Returns("YourVeryLongSuperSecretKey1234567890123456");
            _configMock.Setup(c => c["JwtSettings:Issuer"]).Returns("JobPortal");
            _configMock.Setup(c => c["JwtSettings:Audience"]).Returns("JobPortalAudience");
            _configMock.Setup(c => c["JwtSettings:TokenExpiryInMinutes"]).Returns("60");

            _unitOfWorkMock.Setup(u => u.GenericRepository<User>()).Returns(_userRepoMock.Object);

            _userService = new UserService(_unitOfWorkMock.Object, _mapperMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFail_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Email = "test@example.com",
                Password = "Test123!",
                ConfirmPassword = "DifferentPassword"
            };

            // Act
            var result = await _userService.RegisterUserAsync(userRegisterDto);

            // Assert
            Assert.False(result.IsSuccess());
            Assert.Equal("Passwords do not match.", result.Message);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFail_WhenUserAlreadyExists()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Email = "test@example.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            };

            var existingUser = new User { Email = "test@example.com" };
            _userRepoMock.Setup(r => r.GetAsync(u => u.Email == userRegisterDto.Email && !u.IsDeleted))
                         .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.RegisterUserAsync(userRegisterDto);

            // Assert
            Assert.False(result.IsSuccess());
            Assert.Equal("A user with this email already exists.", result.Message);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnSuccess_WhenUserIsNew()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Email = "test@example.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            };

            _userRepoMock.Setup(r => r.GetAsync(u => u.Email == userRegisterDto.Email && !u.IsDeleted))
                         .ReturnsAsync((User)null);

            var user = new User { Email = userRegisterDto.Email };
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            var userDto = new UserDto { Email = userRegisterDto.Email };
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

            // Act
            var result = await _userService.RegisterUserAsync(userRegisterDto);

            // Assert
            Assert.True(result.IsSuccess());
            Assert.Equal("User registered successfully.", result.Message);
        }

        [Fact]
        public async Task LoginUserAsync_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "test@example.com",
                Password = "Test123!"
            };

            _userRepoMock.Setup(r => r.GetAsync(u => u.Email == userLoginDto.Email && !u.IsDeleted))
                         .ReturnsAsync((User)null);

            // Act
            var result = await _userService.LoginUserAsync(userLoginDto);

            // Assert
            Assert.False(result.IsSuccess());
            Assert.Equal("Invalid email or password.", result.Message);
        }

        [Fact]
        public async Task LoginUserAsync_ShouldReturnSuccess_WhenUserFoundAndPasswordMatches()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "test@example.com",
                Password = "Test123!"
            };

            var user = new User { Email = userLoginDto.Email, Password = PasswordHelper.HashPassword("Test123!") };
            _userRepoMock.Setup(r => r.GetAsync(u => u.Email == userLoginDto.Email && !u.IsDeleted))
                         .ReturnsAsync(user);

            var userDto = new UserDto { Email = userLoginDto.Email };
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _userService.LoginUserAsync(userLoginDto);

            // Assert
            Assert.True(result.IsSuccess());
            Assert.Equal("Login successful.", result.Message);
            Assert.NotNull(result.Data);
        }
    }
}