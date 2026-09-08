using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmiApp.Service.Dto.Bmi;

namespace BmiApp.Service
{
    public interface IBmiService
    {
        public Task<IEnumerable<BmiReadRecordDto>> GetAllBmiRecordsByEmail(string email);

        public Task AddBmiRecord(string email, BmiWriteRecordDto bmiWriteRecordDto);
    }
}
