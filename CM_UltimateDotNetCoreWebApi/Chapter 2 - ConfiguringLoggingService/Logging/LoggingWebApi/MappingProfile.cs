using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects.RequestDtos;
using Shared.DataTransferObjects.ResponseDtos;

namespace LoggingWebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateMap<Company, CompanyDto>()
        //     .ForCtorParam("FullAddress",
        //         opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
        //

        CreateMap<Company, CompanyResponseDto>()
            .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
        CreateMap<CompanyForCreationRequestDto, Company>();

        CreateMap<Employee, EmployeeResponseDto>();
        CreateMap<EmployeeForCreationRequestDto, Employee>();

        CreateMap<EmployeeForUpdateRequestDto, Employee>().ReverseMap();
        CreateMap<CompanyForUpdateRequestDto, Company>();
    }
}