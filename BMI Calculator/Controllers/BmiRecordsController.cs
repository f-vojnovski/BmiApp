using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Dto;

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

        [HttpGet("{email}")]
        public ActionResult<IEnumerable<BmiReadRecordDto>> GetBmiRecordsByEmail(String email)
        {
            var bmiRecords = _bmiService.GetAllBmiRecordsByEmail(email);

            return Ok(bmiRecords);
        }
    }
}
