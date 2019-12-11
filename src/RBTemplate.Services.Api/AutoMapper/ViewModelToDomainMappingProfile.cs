using AutoMapper;
using RBTemplate.Domain.Business.Example;
using RBTemplate.Services.Api.ViewModels;

namespace RBTemplate.Services.Api.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ExampleViewModel,Example>();
        }
    }
}