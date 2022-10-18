using MediatR;
using Persistence.Context;
using Persistence.Services;

namespace Application.Features.ProductFeatures.Commands
{
    public class DeleteProductByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, int>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;

            public DeleteProductByIdCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<int> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                var product = _context.Products.Where(a => a.Id == command.Id).FirstOrDefault();

                if (product == null)
                {
                    return default;
                }
                else
                {
                    product.IsDeleted = true;
                    product.DeleledOn = DateTime.Now;
                    product.DeletedBy = _currentUserService.Id;

                    await _context.SaveChangesAsync();
                    return product.Id;
                }
            }
        }
    }
}