using AutoMapper;
using JobPortal.Business.Classes;
using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos.CompanyDtos;
using JobPortal.Data.GenericRepository;
using JobPortal.Data.Models;
using JobPortal.Data.UnitOfWork;
using Moq;

namespace JobPortal.Tests
{
    public class CompanyServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGenericRepository<Company>> _companyRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICompanyService _companyService;

        public CompanyServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _companyRepoMock = new Mock<IGenericRepository<Company>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.GenericRepository<Company>()).Returns(_companyRepoMock.Object);

            // ICompanyService'i interface üzerinden çağırıyoruz
            _companyService = new CompanyService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateCompanyAsync_ShouldReturnSuccess_WhenCompanyIsCreated()
        {
            // Arrange
            var companyCreateDto = new CompanyCreateDto
            {
                Name = "Test Company",
                Address = "Test Address",
                PhoneNumber = "123456789",
                JobPostingLimit = 10
            };

            var company = new Company();
            _mapperMock.Setup(m => m.Map<Company>(companyCreateDto)).Returns(company);
            _companyRepoMock.Setup(r => r.AddAsync(It.IsAny<Company>())).Returns(Task.CompletedTask);

            // Act
            var result = await _companyService.CreateCompanyAsync(companyCreateDto);

            // Assert
            Assert.True(result.IsSuccess());
            Assert.Equal("Company created successfully.", result.Message);
        }

        [Fact]
        public async Task UpdateCompanyAsync_ShouldReturnFail_WhenCompanyNotFound()
        {
            // Arrange
            var companyUpdateDto = new CompanyUpdateDto
            {
                Name = "Updated Company",
                Address = "Updated Address",
                PhoneNumber = "987654321",
                JobPostingLimit = 15
            };

            _companyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Company)null);

            // Act
            var result = await _companyService.UpdateCompanyAsync(1, companyUpdateDto);

            // Assert
            Assert.False(result.IsSuccess());
            Assert.Equal("Company not found.", result.Message);
        }

        [Fact]
        public async Task UpdateCompanyAsync_ShouldReturnSuccess_WhenCompanyIsUpdated()
        {
            // Arrange
            var companyUpdateDto = new CompanyUpdateDto
            {
                Name = "Updated Company",
                Address = "Updated Address",
                PhoneNumber = "987654321",
                JobPostingLimit = 15
            };

            var existingCompany = new Company { Name = "Old Company" };
            _companyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingCompany);
            _mapperMock.Setup(m => m.Map(companyUpdateDto, existingCompany));

            // Act
            var result = await _companyService.UpdateCompanyAsync(1, companyUpdateDto);

            // Assert
            Assert.True(result.IsSuccess());
            Assert.Equal("Company updated successfully.", result.Message);
        }

        [Fact]
        public async Task DeleteCompanyAsync_ShouldReturnFail_WhenCompanyNotFound()
        {
            // Arrange
            _companyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Company)null);

            // Act
            var result = await _companyService.DeleteCompanyAsync(1);

            // Assert
            Assert.False(result.IsSuccess());
            Assert.Equal("Company not found.", result.Message);
        }

        [Fact]
        public async Task DeleteCompanyAsync_ShouldReturnSuccess_WhenCompanyIsDeleted()
        {
            // Arrange
            var existingCompany = new Company { Name = "Company to Delete" };
            _companyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingCompany);
            _companyRepoMock.Setup(r => r.Delete(existingCompany));

            // Act
            var result = await _companyService.DeleteCompanyAsync(1);

            // Assert
            Assert.True(result.IsSuccess());
            Assert.Equal("Company deleted successfully.", result.Message);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ShouldReturnFail_WhenCompanyNotFound()
        {
            // Arrange
            _companyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Company)null);

            // Act
            var result = await _companyService.GetCompanyByIdAsync(1);

            // Assert
            Assert.False(result.IsSuccess());
            Assert.Equal("Company not found.", result.Message);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ShouldReturnSuccess_WhenCompanyIsFound()
        {
            // Arrange
            var existingCompany = new Company { Name = "Test Company" };
            _companyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingCompany);

            var companyDto = new CompanyDto { Name = "Test Company" };
            _mapperMock.Setup(m => m.Map<CompanyDto>(existingCompany)).Returns(companyDto);

            // Act
            var result = await _companyService.GetCompanyByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess());
            Assert.Equal("Company retrieved successfully.", result.Message);
        }

        [Fact]
        public async Task GetAllCompaniesAsync_ShouldReturnSuccess_WhenCompaniesAreFound()
        {
            // Arrange
            var companies = new List<Company>
        {
            new Company { Name = "Company 1" },
            new Company { Name = "Company 2" }
        };

            _companyRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(companies);

            var companyDtos = new List<CompanyDto>
        {
            new CompanyDto { Name = "Company 1" },
            new CompanyDto { Name = "Company 2" }
        };

            _mapperMock.Setup(m => m.Map<IEnumerable<CompanyDto>>(companies)).Returns(companyDtos);

            // Act
            var result = await _companyService.GetAllCompaniesAsync();

            // Assert
            Assert.True(result.IsSuccess());
            Assert.Equal("Companies retrieved successfully.", result.Message);
        }
    }
}