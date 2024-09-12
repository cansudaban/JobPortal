using JobPortal.Business.Classes;
using JobPortal.Business.Interfaces;
using JobPortal.Common.Dtos.JobDtos;
using JobPortal.Common.Helpers;
using JobPortal.Common.Models;
using JobPortal.Common.ServiceResultManager;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JobPortal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IRestrictedWordService _cacheService;

        public JobController(IJobService jobService, IRestrictedWordService cacheService)
        {
            _jobService = jobService;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Yeni bir iş ilanı oluşturur.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobCreateDto jobCreateDto)
        {
            var serviceResult = await _jobService.CreateJobAsync(jobCreateDto);
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
        /// Mevcut bir iş ilanını günceller.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobUpdateDto jobUpdateDto)
        {
            var serviceResult = await _jobService.UpdateJobAsync(id, jobUpdateDto);
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
        /// Bir iş ilanını siler.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceResult = await _jobService.DeleteJobAsync(id);
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
        /// Belirtilen ID'ye sahip iş ilanını getirir.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var serviceResult = await _jobService.GetJobByIdAsync(id);
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
        /// Tüm iş ilanlarını getirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _jobService.GetAllJobsAsync();
            var response = new ResponseModel
            {
                Message = serviceResult.Message,
                Data = serviceResult.Data,
                StatusCode = (int)serviceResult.ResultCode
            };

            return Ok(response);
        }

        /// <summary>
        /// Sakıncalı kelimeler listesini getirir.
        /// </summary>
        [HttpGet("restricted-words")]
        public async Task<IActionResult> GetAllRestrictedWords()
        {
            var serviceResult = await _cacheService.GetAllRestrictedWordsAsync();
            
            var response = new ResponseModel
            {
                Message = ResourceHelper.OperationSuccess,
                Data = serviceResult
            };
            
            return Ok(response);
        }

        /// <summary>
        /// Listeye yeni bir sakıncalı kelime ekler.
        /// </summary>
        [HttpPost("add-restricted-word")]
        public async Task<IActionResult> AddRestrictedWord([FromBody] string word)
        {
            var serviceResult = await _cacheService.AddRestrictedWordAsync(word);

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
        /// Listeden bir sakıncalı kelimeyi siler.
        /// </summary>
        [HttpDelete("remove-restricted-word")]
        public async Task<IActionResult> RemoveRestrictedWord([FromBody] string word)
        {
            var serviceResult = await _cacheService.RemoveRestrictedWordAsync(word);

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
        /// Elastic search ile arama yapar.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var result = await _jobService.SearchJobsAsync(query);
            if (result.IsSuccess())
            {
                return Ok(result);
            }

            return NotFound(result);
        }
    }
}
