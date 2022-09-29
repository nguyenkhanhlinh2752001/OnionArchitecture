using Application.Features.CategoryFeatures.Commands;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Commands
{
    public class DeleteCustomerByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteCustomerByIdCommandHandler : IRequestHandler<DeleteCustomerByIdCommand, int>
        {
            private readonly ApplicationDbContext _context;
            public DeleteCustomerByIdCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteCustomerByIdCommand command, CancellationToken cancellationToken)
            {
                var obj = _context.Customers.Where(a => a.Id == command.Id).FirstOrDefault();

                if (obj == null)
                {
                    return default;
                }
                else
                {
                    obj.IsDeleted = true;
                    obj.DeleledDate = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return obj.Id;
                }
            }
        }
    }
}
