using Microsoft.EntityFrameworkCore;
using WebApiJoinwithLinq.Model.Entities;

namespace WebApiJoinwithLinq.Data
{
    public class ApplicationDbcontext:DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options):base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
