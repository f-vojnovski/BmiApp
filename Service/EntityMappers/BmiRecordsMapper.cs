using System.Collections.Generic;
using System.Linq;
using Data;
using Service.Dto;

namespace Service.EntityMappers
{
    public class BmiRecordsMapper
    {
        public static IEnumerable<BmiReadRecordDto> MapBmiRecordsToBmiReadRecordDtos(IEnumerable<BmiRecord> bmiRecords)
        {
            return bmiRecords.Select(x => new BmiReadRecordDto()
            {
                Weight = x.Weight,
                Height = x.Height,
                Bmi = x.Bmi
            });
        }
    }
}
