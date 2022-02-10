using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.BmiRepository;
using Service.Dto;
using Service.EntityMappers;

namespace Service
{
    public class BmiService : IBmiService
    {
        private readonly BmiRepository _repository;

        public BmiService(BmiRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<BmiReadRecordDto> GetAllBmiRecordsByEmail(string email)
        {
            return BmiRecordsMapper.MapBmiRecordsToBmiReadRecordDtos(_repository.GetBmiRecordsByEmail(email));
        }
    }
}
