using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BmiApp.Data.Model;
using BmiApp.Repository.Core;
using BmiApp.Service.Dto.Bmi;

namespace BmiApp.Service
{
    public class BmiService : IBmiService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BmiService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<BmiReadRecordDto>> GetAllBmiRecordsByEmail(string email)
        {
            var bmiRecords = await _unitOfWork.BmiRecords.GetByEmailAsync(email);

            var bmiRecordsDto = _mapper.Map<IEnumerable<BmiReadRecordDto>>(bmiRecords);

            return bmiRecordsDto;
        }

        public async Task AddBmiRecord(BmiWriteRecordDto bmiWriteRecordDto)
        {
            var bmiRecord = _mapper.Map<BmiRecord>(bmiWriteRecordDto);

            _unitOfWork.BmiRecords.AddRecord(bmiRecord);
            await _unitOfWork.Save();
        }
    }
}
