using EInsurance.Models;
using Microsoft.EntityFrameworkCore;

namespace EInsurance.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<EInsurance.Models.Customer> Customer { get; set; }
        public DbSet<EInsurance.Models.InsurancePolicy> InsurancePolicy { get; set; }
        public DbSet<EInsurance.Models.PolicyStatus> PolicyStatus { get; set; }

    }
}
