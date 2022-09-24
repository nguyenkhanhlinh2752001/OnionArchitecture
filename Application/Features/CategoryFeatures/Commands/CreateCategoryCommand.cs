using Application.Features.ProductFeatures.Commands;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatures.Commands
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; }

        public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public CreateCategoryCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
            {
                var obj = new Category();
                obj.Name = command.Name;
                obj.CreatedDate = DateTime.Now;
                obj.IsDeleted = false;

                _context.Categories.Add(obj);
                await _context.SaveChangesAsync();
                return obj.Id;
            }
        }
    }
}
