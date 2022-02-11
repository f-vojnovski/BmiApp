using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Dto;
using Service.Dto.Bmi;

namespace Service
{
    public interface IBmiService
    {
        public Task<IEnumerable<BmiReadRecordDto>> GetAllBmiRecordsByEmail(string email);

        public Task AddBmiRecord(BmiWriteRecordDto bmiWriteRecordDto);
    }
}
