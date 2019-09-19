using Microsoft.EntityFrameworkCore;

namespace Order.Models
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }
        public DbSet<OrderModel> Orders { get; set; }

    }

}
