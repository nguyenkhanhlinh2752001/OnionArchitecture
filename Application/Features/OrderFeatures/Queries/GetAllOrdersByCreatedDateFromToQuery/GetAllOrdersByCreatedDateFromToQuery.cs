using Application.Features.OrderFeatures.Queries.GetAllOrderByCreatedDateAtQuery;
using MediatR;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OrderFeatures.Queries.GetAllOrdersByCreatedDateFromToQuery
{
    public class GetAllOrdersByCreatedDateFromToQuery: IRequest<IEnumerable<GetAllOrdersByCreatedDateFromToViewModel>>

    {
        private readonly ApplicationDbContext _context;
    }
}
