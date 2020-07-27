using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Central_De_Erros.Models
{
    public partial class CentralDeErrosContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Error> Errors { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }


        public CentralDeErrosContext(DbContextOptions<CentralDeErrosContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
