using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BmiApp.Service;
using BmiApp.Service.Dto.Bmi;

namespace BmiApp.Api.Controllers
{
    [ApiController]
    [Route("api/bmi")]
    [Authorize]
    public class BmiRecordsController : ControllerBase
    {
        private readonly IBmiService _bmiService;

        public BmiRecordsController(IBmiService bmiService)
        {
            this._bmiService = bmiService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<BmiReadRecordDto>>> GetBmiRecords()
        {
            var email = CurrentUserEmail();

            if (email == null)
            {
                return Unauthorized();
            }

            var bmiRecords = await _bmiService.GetAllBmiRecordsByEmail(email);

            return Ok(bmiRecords);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateBmiRecord([FromBody] BmiWriteRecordDto bmiWriteRecordDto)
        {
            var email = CurrentUserEmail();

            if (email == null)
            {
                return Unauthorized();
            }

            await _bmiService.AddBmiRecord(email, bmiWriteRecordDto);

            return Ok();
        }

        /// <summary>
        /// The record owner is taken from the validated token, so a caller can only ever
        /// read and write their own records regardless of what the request carries.
        /// </summary>
        private string CurrentUserEmail()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            return string.IsNullOrWhiteSpace(email) ? null : email;
        }
    }
}
