using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class ImageProductRepository : RepositoryAsync<ImageProduct, int>, IImageProductRepository
    {
        public ImageProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
