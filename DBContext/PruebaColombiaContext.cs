using Microsoft.EntityFrameworkCore;
using PruebaColombia.Areas.CMSCore.Entities;
using PruebaColombia.Areas.CMSCore.Entities.EntitiesConfiguration;
using PruebaColombia.Areas.BasicCore.Entities.EntitiesConfiguration;
using PruebaColombia.Areas.BasicCore.Entities;
using PruebaColombia.Areas.PruebaColombia.Entities.EntitiesConfiguration;
using PruebaColombia.Areas.PruebaColombia.Entities;

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
        public DbSet<Dispensador> Dispensador { get; set; }
        public DbSet<DispensadorManguera> DispensadorManguera { get; set; }
        public DbSet<Precio> Precio { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<ProductoTipo> ProductoTipo { get; set; }
        public DbSet<ProductoUnidad> ProductoUnidad { get; set; }

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
                modelBuilder.ApplyConfiguration(new DispensadorConfiguration());
                modelBuilder.ApplyConfiguration(new DispensadorMangueraConfiguration());
                modelBuilder.ApplyConfiguration(new PrecioConfiguration());
                modelBuilder.ApplyConfiguration(new ProductoConfiguration());
                modelBuilder.ApplyConfiguration(new ProductoTipoConfiguration());
                modelBuilder.ApplyConfiguration(new ProductoUnidadConfiguration());
            }
            catch (Exception) { throw; }
        }
    }
}
