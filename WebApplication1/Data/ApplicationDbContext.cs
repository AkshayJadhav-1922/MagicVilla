using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext: DbContext
    {
        //remeber this step. We are passing Application Db context options to base class i.e. DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
    }
}
