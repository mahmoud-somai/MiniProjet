using Microsoft.EntityFrameworkCore;
using MiniProjet.Models.Domain;

namespace MiniProjet.Data
{
    public class MVCDemoDbContext : DbContext
    {
        public MVCDemoDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> employees{get; set;}
    }                                                                                                                                                                                                                                   
}
