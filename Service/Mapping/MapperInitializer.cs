
using AutoMapper;
using Data.Model;
using Data.Model.Auth;
using Service.Dto;
using Service.Dto.Auth;
using Service.Dto.Bmi;


namespace Service.Mapping
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<BmiReadRecordDto, BmiRecord>().ReverseMap();
            CreateMap<BmiWriteRecordDto, BmiRecord>().ReverseMap();
            CreateMap<ApiUser, UserDto>().ReverseMap();
        }
    }
}
