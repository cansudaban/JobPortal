using AutoMapper;
using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos.CompanyDtos;
using JobPortal.Common.ServiceResultManager;
using JobPortal.Data.GenericRepository;
using JobPortal.Data.Models;
using JobPortal.Data.UnitOfWork;

namespace JobPortal.Business.Classes
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Company> _companyRepo;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _companyRepo = _unitOfWork.GenericRepository<Company>();
            _mapper = mapper;
        }

        public async Task<ServiceResult> CreateCompanyAsync(CompanyCreateDto companyCreateDto)
        {
            var company = _mapper.Map<Company>(companyCreateDto);

            await _companyRepo.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();

            return Result.ReturnAsSuccess("Company created successfully.");
        }

        public async Task<ServiceResult> UpdateCompanyAsync(int id, CompanyUpdateDto companyUpdateDto)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null)
            {
                return Result.ReturnAsFail("Company not found.");
            }

            _mapper.Map(companyUpdateDto, company);
            _companyRepo.Update(company);
            await _unitOfWork.SaveChangesAsync();

            return Result.ReturnAsSuccess("Company updated successfully.");
        }

        public async Task<ServiceResult> DeleteCompanyAsync(int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null)
            {
                return Result.ReturnAsFail("Company not found.");
            }

            _companyRepo.Delete(company);
            await _unitOfWork.SaveChangesAsync();

            return Result.ReturnAsSuccess("Company deleted successfully.");
        }

        public async Task<ServiceResult> GetCompanyByIdAsync(int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null)
            {
                return Result.ReturnAsFail("Company not found.");
            }

            var companyDto = _mapper.Map<CompanyDto>(company);
            return Result.ReturnAsSuccess("Company retrieved successfully.", companyDto);
        }

        public async Task<ServiceResult> GetAllCompaniesAsync()
        {
            var companies = await _companyRepo.GetAllAsync();
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Result.ReturnAsSuccess("Companies retrieved successfully.", companyDtos);
        }
    }
}
