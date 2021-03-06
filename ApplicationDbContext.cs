using ApiNominas.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiNominas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
    }
}
