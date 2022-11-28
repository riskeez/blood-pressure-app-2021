using BPLog.API.Extensions;
using BPLog.API.Helpers;
using BPLog.API.Model;
using BPLog.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPLog.API.Controllers
{
    /// <summary>
    /// List of endpoints for interaction with blood pressure records
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class BloodPressureController : ControllerBase
    {
        private readonly IBloodPressureService _service;

        /// <summary>
        /// Blood Pressure Controller constructor
        /// </summary>
        /// <param name="service">Blood Pressure service</param>
        public BloodPressureController(IBloodPressureService service)
        {
            _service = service;
        }

        private int CurrentUserId => HttpContext.User.GetUserId() ?? throw new ArgumentNullException("User is not authorized!");

        /// <summary>
        /// Gets latest blood pressure measurement for the user
        /// </summary>
        /// <returns>Blood Pressure record</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BloodPressureData>> GetLastRecord()
        {
            var lastRecord = await _service.GetLastRecordByUserId(CurrentUserId);
            if (lastRecord != null)
            {
                return Ok(lastRecord);
            }
            return NotFound();
        }

        /// <summary>
        /// Gets pressures with pagination and sorting
        /// </summary>
        /// <param name="pageSize">Size of a page</param>
        /// <param name="page">Number of a page</param>
        /// <param name="sort">Sorting (asc, desc). Desc by default</param>
        /// <returns>Page object with data</returns>
        [HttpGet("page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BloodPressurePage>> GetList(int pageSize, int page, string sort)
        {
            SortOrder sortBy = QueryHelper.GetParamSortOrder(sort);
            return await _service.GetList(CurrentUserId, pageSize, page, sortBy);
        }

        /// <summary>
        /// Saves new blood pressure measurement to database for the user
        /// </summary>
        /// <param name="measurement">Measurement</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveRecord(BloodPressureData measurement)
        {
            bool isSaved = await _service.SaveRecord(CurrentUserId, measurement);
            if (isSaved)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes specified 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            if (await _service.DeleteRecordById(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
