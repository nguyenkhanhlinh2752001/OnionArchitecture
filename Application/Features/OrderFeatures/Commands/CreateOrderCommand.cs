using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.OrderFeatures.Commands
{
    public class CreateOrderCommand: IRequest<int>
    {
        public int CustomerId { get; set; }
        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
        {
            private readonly IApplicationDbContext _context;
            public CreateOrderCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
            {
                var obj = new Order();
                obj.CustomerId=command.CustomerId;
                obj.CreatedDate = DateTime.Now;
                obj.IsDeleted = false;

                _context.Orders.Add(obj);
                await _context.SaveChangesAsync();
                return obj.Id;
            }
        }


    }
}
