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
    public class UpdateCustomerCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, string>
        {
            private readonly ApplicationDbContext _context;
            public UpdateCustomerCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<string> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
            {
                var product = _context.Users.Where(a => a.Id == command.Id).FirstOrDefault();

                if (product == null)
                {
                    return default;
                }
                else
                {
                    product.UserName = command.Username;
                    product.PhoneNumber = command.Phone;
                    product.Address = command.Address;

                    await _context.SaveChangesAsync();
                    return product.Id;
                }
            }
        }
    }
}
