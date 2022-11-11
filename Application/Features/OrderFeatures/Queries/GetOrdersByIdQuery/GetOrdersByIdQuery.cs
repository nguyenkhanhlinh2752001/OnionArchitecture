using Application.Interfaces.Repositories;
using Application.ViewModels;
using Application.Wrappers;
using MediatR;
using Persistence.Context;

namespace Application.Features.OrderFeatures.Queries.GetOrdersByIdQuery
{
    public class GetOrdersByIdQuery : IRequest<Response<GetOrdersByIdViewModel>>
    {
        public int Id { get; set; }

        internal class GetOrdersByIdQueryHanlder : IRequestHandler<GetOrdersByIdQuery, Response<GetOrdersByIdViewModel>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IOrderRespository _orderRespository;
            private readonly IOrderDetailRepository _orderDetailRepository;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IProductRepository _productRepository;

            public GetOrdersByIdQueryHanlder(ApplicationDbContext context, IOrderRespository orderRespository, IOrderDetailRepository orderDetailRepository, IProductDetailRepository productDetailRepository, IProductRepository productRepository)
            {
                _context = context;
                _orderRespository = orderRespository;
                _orderDetailRepository = orderDetailRepository;
                _productDetailRepository = productDetailRepository;
                _productRepository = productRepository;
            }

            public async Task<Response<GetOrdersByIdViewModel>> Handle(GetOrdersByIdQuery request, CancellationToken cancellationToken)
            {
                //var result = await (from o in _context.Orders
                //                    join c in _context.Users on o.UserId equals c.Id
                //                    where o.Id == request.Id
                //                    select new GetOrdersByIdViewModel()
                //                    {
                //                        Id = o.Id,
                //                        CustomerName = c.UserName,
                //                        TotalPrice = o.TotalPrice,
                //                        CreatedOn = o.CreatedOn,
                //                        OrderDetails = (from od in _context.OrderDetails
                //                                        join p in _context.Products
                //                                        on od.ProductDetailId equals p.Id
                //                                        where od.OrderId == o.Id
                //                                        select new OrderDetailVM
                //                                        {
                //                                            ProductName = p.Name,
                //                                            Price = p.P,
                //                                            Quantity = od.Quantity,
                //                                           // UnitPrice = p.Price * od.Quantity,
                //                                        }).ToList()
                //                    }).FirstOrDefaultAsync();
                //return new Response<GetOrdersByIdViewModel>(result);

                var query = (from o in _orderRespository.Entities
                             join od in _orderDetailRepository.Entities
                             on o.Id equals od.OrderId
                             join u in _context.Users
                             on o.UserId equals u.Id
                             where o.Id == request.Id
                             join pd in _productDetailRepository.Entities
                             on od.ProductDetailId equals pd.Id
                             join p in _productRepository.Entities
                             on pd.ProductId equals p.Id
                             select new GetOrdersByIdViewModel
                             {
                                 CustomerName = u.UserName,
                                 TotalPrice = o.TotalPrice,
                                 CreatedOn = o.CreatedOn,
                                 OrderDetails = (from od in _orderDetailRepository.Entities
                                                 where od.OrderId == o.Id
                                                 select new OrderDetailVM
                                                 {
                                                     ProductName = p.Name,
                                                     Color = pd.Color,
                                                     Price = pd.Price,
                                                     Quantity = od.Quantity,
                                                 }).ToList()
                             }).FirstOrDefault();
                return new Response<GetOrdersByIdViewModel>(query);
                throw new Exception();
            }
        }
    }
}