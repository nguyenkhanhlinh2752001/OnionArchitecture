using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        Task<int> SaveChangesAsync();
    }
}
