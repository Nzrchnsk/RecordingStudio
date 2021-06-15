using AutoMapper;
using RecordingStudio.Dto;
using RecordingStudio.Models;

namespace RecordingStudio
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Studio, StudioDto>();
            CreateMap<StudioDto, StudioDto>();
        }
    }
}