using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<PagedResponse<IEnumerable<GetAllProductsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
        public string? ProductName { get; set; }

        internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<IEnumerable<GetAllProductsViewModel>>>
        {
            private readonly IProductRepository _productRepsitory;
            private readonly IProductDetailRepository _productDetailRepository;
            private readonly IOrderDetailRepository _orderDetailRepository;
            private readonly IReviewRepository _reviewRepository;

            public GetAllProductsQueryHandler(IProductRepository productRepsitory, IProductDetailRepository productDetailRepository, IOrderDetailRepository orderDetailRepository, IReviewRepository reviewRepository)
            {
                _productRepsitory = productRepsitory;
                _productDetailRepository = productDetailRepository;
                _orderDetailRepository = orderDetailRepository;
                _reviewRepository = reviewRepository;
            }

            public async Task<PagedResponse<IEnumerable<GetAllProductsViewModel>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
            {
                var query = (from p in _productRepsitory.Entities
                             where (string.IsNullOrEmpty(request.ProductName) || p.Name.ToLower().Contains(request.ProductName.ToLower()))
                             select new GetAllProductsViewModel()
                             {
                                 ProductId = p.Id,
                                 ProductName = p.Name,
                                 MinPrice = (from pd in _productDetailRepository.Entities
                                             where pd.ProductId == p.Id
                                             select pd).Min(e => e.Price),
                                 MaxPrice = (from pd in _productDetailRepository.Entities
                                             where pd.ProductId == p.Id
                                             select pd).Max(e => e.Price),
                                 AvgRate = (decimal)(from r in _reviewRepository.Entities
                                                     join pd in _productDetailRepository.Entities
                                                     on r.ProductDetailId equals pd.Id
                                                     where pd.ProductId == p.Id
                                                     select r).Average(e => e.Rate),
                                 SaleAmount = (from od in _orderDetailRepository.Entities
                                               join pd in _productDetailRepository.Entities
                                               on od.ProductDetailId equals pd.Id
                                               where pd.ProductId == p.Id
                                               select od).Sum(e => e.Quantity),
                                 CreatedOn = p.CreatedOn,
                             });
                var data = query.OrderBy(request.OrderBy!);
                var total = query.Count();
                var rs = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
                return (new PagedResponse<IEnumerable<GetAllProductsViewModel>>(rs, request.PageNumber, request.PageSize, total));
            }
        }
    }
}