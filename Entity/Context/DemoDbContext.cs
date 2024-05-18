using Microsoft.EntityFrameworkCore;

namespace Prodcuct.Entities{
    public partial class DemoDbContext : DbContext{
        public DemoDbContext(){}
        public DemoDbContext(DbContextOptions<DemoDbContext> options): base(options) {

        }

        public virtual DbSet<Customer> Customers {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine("DemoDbContext is Setting ");
            if(!optionsBuilder.IsConfigured){
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Console.WriteLine("DemoDbContext is OnModelCreating ");
            modelBuilder.HasDefaultSchema("SalesLT").Entity<Customer>(entity =>{
                entity.HasKey(e => e.CustomerID).HasName("PK_Customer_CustomerID");
                entity.ToTable("Customer");
                entity.Property(e => e.rowguid).HasDefaultValue(Guid.NewGuid());
                entity.Property(e => e.ModifiedDate).HasDefaultValue(DateTime.Now);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}