using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.CategoryFeatures.Commands.CreateCategoryCommand
{
    public class AddEditCategoryCommand : IRequest<Response<AddEditCategoryCommand>>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        internal class AddEditCategoryCommandHandler : IRequestHandler<AddEditCategoryCommand, Response<AddEditCategoryCommand>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;
            private readonly IUnitOfWork<int> _unitOfWork;

            public AddEditCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork<int> unitOfWork)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
                _unitOfWork = unitOfWork;
            }

            public async Task<Response<AddEditCategoryCommand>> Handle(AddEditCategoryCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == 0)
                {
                    var addCategory = _mapper.Map<Category>(request);
                    await _categoryRepository.AddAsync(addCategory);
                    await _unitOfWork.Commit(cancellationToken);
                    request.Id = addCategory.Id;
                }
                else
                {
                    var updateCategory = await _categoryRepository.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
                    if (updateCategory == null) throw new ApiException("Category not found");
                    _mapper.Map(request, updateCategory);
                    await _categoryRepository.UpdateAsync(updateCategory);
                    await _unitOfWork.Commit(cancellationToken);
                    request.Id = updateCategory.Id;
                }
                return (new Response<AddEditCategoryCommand>(request));
            }
        }
    }
}