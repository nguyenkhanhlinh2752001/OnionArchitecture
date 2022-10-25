using Application.ViewModels;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Queries.GetOrdersByIdQuery
{
    public class GetOrdersByIdQuery : IRequest<Response<GetOrdersByIdViewModel>>
    {
        public int Id { get; set; }

        internal class GetOrdersByIdQueryHanlder : IRequestHandler<GetOrdersByIdQuery, Response<GetOrdersByIdViewModel>>
        {
            private readonly ApplicationDbContext _context;

            public GetOrdersByIdQueryHanlder(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<GetOrdersByIdViewModel>> Handle(GetOrdersByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await (from o in _context.Orders
                                    join c in _context.Users on o.UserId equals c.Id
                                    where o.Id == request.Id
                                    select new GetOrdersByIdViewModel()
                                    {
                                        Id = o.Id,
                                        CustomerName = c.UserName,
                                        TotalPrice = o.TotalPrice,
                                        CreatedOn = o.CreatedOn,
                                        OrderDetails = (from od in _context.OrderDetails
                                                        join p in _context.Products
                                                        on od.ProductId equals p.Id
                                                        where od.OrderId == o.Id
                                                        select new OrderDetailVM
                                                        {
                                                            ProductName = p.Name,
                                                            Price = p.Price,
                                                            Quantity = od.Quantity,
                                                            UnitPrice = p.Price * od.Quantity,
                                                        }).ToList()
                                    }).FirstOrDefaultAsync();
                return new Response<GetOrdersByIdViewModel>(result);
            }
        }
    }
}