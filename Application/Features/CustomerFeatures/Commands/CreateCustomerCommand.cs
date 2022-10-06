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
    public class CreateCustomerCommand: IRequest<string>
    {
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
        {
            private readonly ApplicationDbContext _context;
            public CreateCustomerCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<string> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
            {
                var obj = new User();
                obj.UserName = command.Username;
                obj.Phone = command.Phone;
                obj.Address = command.Address;
                obj.CreatedDate = DateTime.Now;
                obj.IsActive = false;

                _context.Users.Add(obj);
                await _context.SaveChangesAsync();
                return obj.Id;
            }
        }
    }
}
