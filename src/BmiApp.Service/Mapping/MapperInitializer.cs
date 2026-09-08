
using AutoMapper;
using BmiApp.Data.Model;
using BmiApp.Data.Model.Auth;
using BmiApp.Service.Dto.Bmi;
using BmiApp.Service.Dto.Auth;


namespace BmiApp.Service.Mapping
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
