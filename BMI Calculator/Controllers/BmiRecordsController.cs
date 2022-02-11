using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Service;
using Service.Dto;
using Service.Dto.Bmi;

namespace BMI_Calculator.Controllers
{
    [ApiController]
    [Route("api/bmi")]
    public class BmiRecordsController : ControllerBase
    {
        private readonly IBmiService _bmiService;

        public BmiRecordsController(IBmiService bmiService)
        {
            this._bmiService = bmiService;
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<IEnumerable<BmiRecord>>> GetBmiRecordsByEmail(String email)
        {
            var bmiRecords = await _bmiService.GetAllBmiRecordsByEmail(email);

            return Ok(bmiRecords);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBmiRecord([FromBody] BmiWriteRecordDto bmiWriteRecordDto)
        {
            await _bmiService.AddBmiRecord(bmiWriteRecordDto);

            return Ok();
        }
    }
}
