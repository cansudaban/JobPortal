using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos.CompanyDtos;
using JobPortal.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Yeni bir şirket oluşturur.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyCreateDto companyCreateDto)
        {
            var serviceResult = await _companyService.CreateCompanyAsync(companyCreateDto);
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
        /// Mevcut bir şirketi günceller.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyUpdateDto companyUpdateDto)
        {
            var serviceResult = await _companyService.UpdateCompanyAsync(id, companyUpdateDto);
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
        /// Bir şirketi siler.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceResult = await _companyService.DeleteCompanyAsync(id);
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

        /// <summary>
        /// Belirtilen ID'ye sahip şirketi getirir.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var serviceResult = await _companyService.GetCompanyByIdAsync(id);
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

        /// <summary>
        /// Tüm şirketleri getirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _companyService.GetAllCompaniesAsync();
            var response = new ResponseModel
            {
                Message = serviceResult.Message,
                Data = serviceResult.Data,
                StatusCode = (int)serviceResult.ResultCode
            };

            return Ok(response);
        }
    }
}
