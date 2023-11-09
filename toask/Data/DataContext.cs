using Microsoft.EntityFrameworkCore;
using toask.Models;

namespace toask.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext>options):base(options) 
        { 
        
        }
        public DbSet<Producto>? Productos { get; set; } = null;

        
    }
}
