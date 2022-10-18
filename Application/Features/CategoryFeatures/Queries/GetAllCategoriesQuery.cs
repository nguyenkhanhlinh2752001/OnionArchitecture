using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.CategoryFeatures.Queries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<Category>>
    {
        public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
        {
            private readonly ApplicationDbContext _context;

            public GetAllCategoriesQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
            {
                var list = await _context.Categories.Where(p => p.IsDeleted == false).ToListAsync();
                if (list == null)
                {
                    return null;
                }
                return list.AsReadOnly();
            }
        }
    }
}