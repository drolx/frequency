using AutoMapper;
using Proton.Frequency.Common.Config;
using Proton.Frequency.Common.Dto;

namespace Proton.Frequency;

public class AutoMapperProfile : Profile {
    public AutoMapperProfile() {
        CreateMap<NetworkConfig, SampleDto>();
    }
}
