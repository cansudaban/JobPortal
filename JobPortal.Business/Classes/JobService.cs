using AutoMapper;
using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos.CompanyDtos;
using JobPortal.Common.Dtos.JobDtos;
using JobPortal.Common.ServiceResultManager;
using JobPortal.Common.Services;
using JobPortal.Common.Helpers;
using JobPortal.Data.GenericRepository;
using JobPortal.Data.Models;
using JobPortal.Data.UnitOfWork;

namespace JobPortal.Business.Classes
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Job> _jobRepo;
        private readonly ICompanyService _companyService;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly Nest.IElasticClient _elasticClient;

        public JobService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICompanyService companyService,
            ICacheService cacheService,
            ElasticSearchService elasticSearchService)
        {
            _unitOfWork = unitOfWork;
            _jobRepo = _unitOfWork.GenericRepository<Data.Models.Job>();
            _companyService = companyService;
            _mapper = mapper;
            _elasticClient = elasticSearchService.Client;
            _cacheService = cacheService;
        }

        public async Task<ServiceResult> CreateJobAsync(JobCreateDto jobCreateDto)
        {
            var companyResult = await _companyService.GetCompanyByIdAsync(jobCreateDto.CompanyId);
            if (!companyResult.IsSuccess())
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("CompanyNotFound"));
            }

            var company = (CompanyDto)companyResult.Data;

            var limitCheckResult = await CheckJobPostingLimit(jobCreateDto.CompanyId, company.JobPostingLimit);
            if (!limitCheckResult.IsSuccess())
            {
                return limitCheckResult;
            }

            var qualityScore = await CalculateJobQualityScoreAsync(jobCreateDto.EmploymentType, jobCreateDto.Salary, jobCreateDto.Benefits, jobCreateDto.Description);

            var job = _mapper.Map<Job>(jobCreateDto);
            job.QualityScore = qualityScore;

            await _jobRepo.AddAsync(job);
            await _unitOfWork.SaveChangesAsync();

            var indexResponse = await _elasticClient.IndexDocumentAsync(job);
            if (!indexResponse.IsValid)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("JobCreatedFailedElasticsearch"));
            }

            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("JobCreatedSuccessfully"));
        }

        public async Task<ServiceResult> UpdateJobAsync(int id, JobUpdateDto jobUpdateDto)
        {
            var job = await _jobRepo.GetByIdAsync(id);
            if (job == null)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("JobNotFound"));
            }

            var qualityScore = await CalculateJobQualityScoreAsync(jobUpdateDto.EmploymentType, jobUpdateDto.Salary, jobUpdateDto.Benefits, jobUpdateDto.Description);

            _mapper.Map(jobUpdateDto, job);
            job.QualityScore = qualityScore;

            _jobRepo.Update(job);
            await _unitOfWork.SaveChangesAsync();

            var updateResponse = await _elasticClient.IndexDocumentAsync(job);
            if (!updateResponse.IsValid)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("JobUpdatedFailedElasticsearch"));
            }

            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("JobUpdatedSuccessfully"));
        }

        public async Task<ServiceResult> DeleteJobAsync(int id)
        {
            var job = await _jobRepo.GetByIdAsync(id);
            if (job == null)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("JobNotFound"));
            }

            _jobRepo.Delete(job);
            await _unitOfWork.SaveChangesAsync();

            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("JobDeletedSuccessfully"));
        }

        public async Task<ServiceResult> GetJobByIdAsync(int id)
        {
            var job = await _jobRepo.GetByIdAsync(id);
            if (job == null)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("JobNotFound"));
            }

            var jobDto = _mapper.Map<JobDto>(job);
            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("JobRetrievedSuccessfully"), jobDto);
        }

        public async Task<ServiceResult> GetAllJobsAsync()
        {
            var jobs = await _jobRepo.GetAllAsync();
            var jobDtos = _mapper.Map<IEnumerable<JobDto>>(jobs);

            return Result.ReturnAsSuccess(ResourceHelper.GetMessage("JobsRetrievedSuccessfully"), jobDtos);
        }

        public async Task<ServiceResult> SearchJobsAsync(string query)
        {
            var searchResponse = await _elasticClient.SearchAsync<JobDto>(s => s
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(f => f
                            .Field(p => p.Position)
                            .Field(p => p.Description)
                            .Field(p => p.EmploymentType)
                        )
                        .Query(query)
                    )
                )
            );

            if (searchResponse.IsValid && searchResponse.Documents != null)
            {
                return Result.ReturnAsSuccess(ResourceHelper.GetMessage("JobsFound"), searchResponse.Documents);
            }

            return Result.ReturnAsFail(ResourceHelper.GetMessage("NoJobsFound"));
        }

        private async Task<int> CalculateJobQualityScoreAsync(string employmentType, decimal salary, string benefits, string description)
        {
            int score = 0;

            // Çalışma türü belirtilmişse 1 puan
            if (!string.IsNullOrEmpty(employmentType))
            {
                score += 1;
            }

            // Ücret bilgisi belirtilmişse 1 puan
            if (salary > 0)
            {
                score += 1;
            }

            // Yan haklar belirtilmişse 1 puan
            if (!string.IsNullOrEmpty(benefits))
            {
                score += 1;
            }

            // Sakıncalı kelime kontrolü (2 puan)
            var restrictedWordCheckResult = await CheckRestrictedWords(description);
            if (restrictedWordCheckResult.IsSuccess())
            {
                score += 2;
            }

            return score;
        }

        // Sakıncalı kelime kontrolü
        private async Task<ServiceResult> CheckRestrictedWords(string description)
        {
            var restrictedWords = await _cacheService.GetAllRestrictedWordsAsync();

            foreach (var word in restrictedWords)
            {
                if (description.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    return Result.ReturnAsFail(string.Format(ResourceHelper.GetMessage("RestrictedWordInDescription"), word));
                }
            }

            return Result.ReturnAsSuccess();
        }

        private async Task<ServiceResult> CheckJobPostingLimit(int companyId, int jobPostingLimit)
        {
            var currentJobCount = await _jobRepo.GetAllAsync(j => j.CompanyId == companyId);

            if (currentJobCount.Count() >= jobPostingLimit)
            {
                return Result.ReturnAsFail(ResourceHelper.GetMessage("JobPostingLimitExceeded"));
            }

            return Result.ReturnAsSuccess();
        }
    }
}
