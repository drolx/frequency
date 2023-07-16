using AutoMapper;
using Frequency.Common.Config;
using Frequency.Common.Dto;

namespace Frequency;

public class AutoMapperProfile : Profile {
    public AutoMapperProfile() {
        CreateMap<NetConfig, SampleDto>();
    }
}