using Application.Features.CategoryFeatures.Commands;
using Domain.Entities;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Commands
{
    public class CreateCustomerCommand: IRequest<int>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
        {
            private readonly ApplicationDbContext _context;
            public CreateCustomerCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
            {
                var obj = new Customer();
                obj.Name = command.Name;
                obj.Phone = command.Phone;
                obj.Address = command.Address;
                obj.CreatedDate = DateTime.Now;
                obj.IsDeleted = false;

                _context.Customers.Add(obj);
                await _context.SaveChangesAsync();
                return obj.Id;
            }
        }
    }
}
