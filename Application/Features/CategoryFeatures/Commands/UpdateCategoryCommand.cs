﻿using Application.Features.ProductFeatures.Commands;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CategoryFeatures.Commands
{
    public class UpdateCategoryCommand: IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public UpdateCategoryCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
            {
                var product = _context.Categories.Where(a => a.Id == command.Id).FirstOrDefault();

                if (product == null)
                {
                    return default;
                }
                else
                {
                    product.Name = command.Name;
                    await _context.SaveChangesAsync();
                    return product.Id;
                }
            }
        }
    }
}
