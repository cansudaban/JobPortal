using JobPortal.Common.Dtos.JobDtos;
using JobPortal.Common.ServiceResultManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Business.Interfaces
{
    public interface IJobService
    {
        /// <summary>
        /// Yeni bir iş ilanı oluşturur.
        /// </summary>
        Task<ServiceResult> CreateJobAsync(JobCreateDto jobCreateDto);

        /// <summary>
        /// Mevcut bir iş ilanını günceller.
        /// </summary>
        Task<ServiceResult> UpdateJobAsync(int id, JobUpdateDto jobUpdateDto);

        /// <summary>
        /// Bir iş ilanını siler.
        /// </summary>
        Task<ServiceResult> DeleteJobAsync(int id);

        /// <summary>
        /// Belirtilen ID'ye sahip iş ilanını getirir.
        /// </summary>
        Task<ServiceResult> GetJobByIdAsync(int id);

        /// <summary>
        /// Tüm iş ilanlarını getirir.
        /// </summary>
        Task<ServiceResult> GetAllJobsAsync();

        /// <summary>
        /// Elastic üzerinden arama sonuçlarını getirir.
        /// </summary>
        /// <returns>Arama sonucu</returns>
        Task<ServiceResult> SearchJobsAsync(string query);

    }
}
