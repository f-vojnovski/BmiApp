using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Dto;

namespace Service
{
    public interface IBmiService
    {
        public IEnumerable<BmiReadRecordDto> GetAllBmiRecordsByEmail(string email);
    }
}
