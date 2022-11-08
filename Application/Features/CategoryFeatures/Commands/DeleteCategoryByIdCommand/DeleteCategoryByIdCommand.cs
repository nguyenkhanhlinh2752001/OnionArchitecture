using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;

namespace Application.Features.CategoryFeatures.Commands.DeleteCategoryByIdCommand
{
    public class DeleteCategoryByIdCommand : IRequest<Response<DeleteCategoryByIdCommand>>
    {
        public int Id { get; set; }

        internal class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, Response<DeleteCategoryByIdCommand>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteCategoryByIdCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork<int> unitOfWork)
            {
                _categoryRepository = categoryRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Response<DeleteCategoryByIdCommand>> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
            {
                var category = await _categoryRepository.FindAsync(x => x.Id == request.Id && !x.IsDeleted);
                if (category == null) throw new ApiException("Category not found");
                category.IsDeleted = true;
                await _categoryRepository.UpdateAsync(category);
                await _unitOfWork.Commit(cancellationToken);
                return new Response<DeleteCategoryByIdCommand>(request);
            }
        }
    }
}