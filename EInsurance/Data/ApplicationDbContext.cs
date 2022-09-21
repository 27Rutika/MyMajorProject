using EInsurance.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EInsurance.Data
{
    public class ApplicationDbContext : IdentityDbContext
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
