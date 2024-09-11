using JobPortal.Common.Dtos.CompanyDtos;
using JobPortal.Common.ServiceResultManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Business.Interfaces
{
    public interface ICompanyService
    {
        /// <summary>
        /// Yeni bir şirket oluşturur.
        /// </summary>
        /// <param name="companyCreateDto">Şirket oluşturma için gerekli DTO.</param>
        /// <returns>Oluşturma işleminin sonucunu döner.</returns>
        Task<ServiceResult> CreateCompanyAsync(CompanyCreateDto companyCreateDto);

        /// <summary>
        /// Mevcut bir şirketi günceller.
        /// </summary>
        /// <param name="id">Güncellenecek şirketin Id'si.</param>
        /// <param name="companyUpdateDto">Şirket güncelleme için gerekli DTO.</param>
        /// <returns>Güncelleme işleminin sonucunu döner.</returns>
        Task<ServiceResult> UpdateCompanyAsync(int id, CompanyUpdateDto companyUpdateDto);

        /// <summary>
        /// Belirtilen Id'ye sahip şirketi siler.
        /// </summary>
        /// <param name="id">Silinecek şirketin Id'si.</param>
        /// <returns>Silme işleminin sonucunu döner.</returns>
        Task<ServiceResult> DeleteCompanyAsync(int id);

        /// <summary>
        /// Belirtilen Id'ye sahip şirketi getirir.
        /// </summary>
        /// <param name="id">Getirilecek şirketin Id'si.</param>
        /// <returns>Şirket bilgilerini içeren sonucu döner.</returns>
        Task<ServiceResult> GetCompanyByIdAsync(int id);

        /// <summary>
        /// Tüm şirketleri getirir.
        /// </summary>
        /// <returns>Tüm şirketlerin listesini içeren sonucu döner.</returns>
        Task<ServiceResult> GetAllCompaniesAsync();
    }
}
