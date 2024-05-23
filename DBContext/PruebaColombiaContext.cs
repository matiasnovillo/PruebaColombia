using Microsoft.EntityFrameworkCore;
using PruebaColombia.Areas.CMSCore.Entities;
using PruebaColombia.Areas.CMSCore.Entities.EntitiesConfiguration;
using PruebaColombia.Areas.BasicCore.Entities.EntitiesConfiguration;
using PruebaColombia.Areas.BasicCore.Entities;

namespace PruebaColombia.DBContext
{
    public class PruebaColombiaContext : DbContext
    {
        protected IConfiguration _configuration { get; }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        public DbSet<Failure> Failure { get; set; }
        public DbSet<Parameter> Parameter { get; set; }

        //PruebaColombia

        public PruebaColombiaContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                string ConnectionString = "";
#if DEBUG
                ConnectionString = "data source =.; " +
                    "initial catalog = PruebaColombia; " +
                    "Integrated Security = SSPI;" +
                    " MultipleActiveResultSets=True;" +
                    "Pooling=false;" +
                    "Persist Security Info=True;" +
                    "App=EntityFramework;" +
                    "TrustServerCertificate=True;";
#else
                ConnectionString = "Password=[Password];" +
                    "Persist Security Info=True;" +
                    "User ID=[User];" +
                    "Initial Catalog=[Database];" +
                    "Data Source=[Server];" +
                    "TrustServerCertificate=True";
#endif

                optionsBuilder
                    .UseSqlServer(ConnectionString);
            }
            catch (Exception) { throw; }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.ApplyConfiguration(new UserConfiguration());
                modelBuilder.ApplyConfiguration(new RoleConfiguration());
                modelBuilder.ApplyConfiguration(new MenuConfiguration());
                modelBuilder.ApplyConfiguration(new RoleMenuConfiguration());
                modelBuilder.ApplyConfiguration(new FailureConfiguration());
                modelBuilder.ApplyConfiguration(new ParameterConfiguration());

                //PruebaColombia
            }
            catch (Exception) { throw; }
        }
    }
}
