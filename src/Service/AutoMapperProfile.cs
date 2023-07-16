using AutoMapper;
using Frequency.Common.Config;
using Frequency.Common.Dto;

namespace Proton.Frequency;

public class AutoMapperProfile : Profile {
    public AutoMapperProfile() {
        CreateMap<NetConfig, SampleDto>();
    }
}