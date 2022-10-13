using AutoMapper;
using Proton.Frequency.Common.Dto;
using Proton.Frequency.Config;

namespace Proton.Frequency; 

public class AutoMapperProfile : Profile {
    public AutoMapperProfile() {
        CreateMap<NetworkConfig, SampleDto>();
    }
}
