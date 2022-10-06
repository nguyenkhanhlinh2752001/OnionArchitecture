using MediatR;
using Persistence.Context;

namespace Application.Features.CustomerFeatures.Commands
{
    public class DeleteCustomerByIdCommand : IRequest<string>
    {
        public string Id { get; set; }

        public class DeleteCustomerByIdCommandHandler : IRequestHandler<DeleteCustomerByIdCommand, string>
        {
            private readonly ApplicationDbContext _context;

            public DeleteCustomerByIdCommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(DeleteCustomerByIdCommand command, CancellationToken cancellationToken)
            {
                var obj = _context.Users.Where(a => a.Id == command.Id).FirstOrDefault();

                if (obj == null)
                {
                    return default;
                }
                else
                {
                    obj.IsActive = true;

                    await _context.SaveChangesAsync();
                    return obj.Id;
                }
            }
        }
    }
}