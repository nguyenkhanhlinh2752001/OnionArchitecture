using Application.Features.CategoryFeatures.Commands.CreateCategoryCommand;
using AutoMapper;

namespace Application.Mappings.Category
{
    public class CategoryMappings : Profile
    {
        public CategoryMappings()
        {
            CreateMap<AddEditCategoryCommand, Domain.Entities.Category>();
        }
    }
}