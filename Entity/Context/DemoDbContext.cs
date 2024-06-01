using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Prodcuct.Entities{
    public partial class DemoDbContext : DbContext{
        public DemoDbContext(){}
        public DemoDbContext(DbContextOptions<DemoDbContext> options): base(options) {

        }

        public virtual DbSet<Customer> Customers {get; set;}
        public virtual DbSet<Address> Addresses {get; set;}
        public virtual DbSet<CustomerAddress> CustomerAddress {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured){
                Console.WriteLine("DemoDbContext is Setting ");
                optionsBuilder.UseSqlServer(
                    Environment.GetEnvironmentVariable("SqlConnectionString"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(60)
                );
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Console.WriteLine("DemoDbContext is OnModelCreating ");
            # region Table [Customer]

            modelBuilder.HasDefaultSchema("SalesLT").Entity<Customer>(entity =>{
                entity.ToTable("Customer");
                entity.HasKey(e => e.CustomerID).HasName("PK_Customer_CustomerID");
                entity.Property(e => e.CustomerID).UseIdentityColumn();
                
                
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.PasswordSalt).IsRequired();
                entity.Property(e => e.rowguid).ValueGeneratedOnAdd();
                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();
            });

            # endregion

            # region Table [Address]

            modelBuilder.HasDefaultSchema("SalesLT").Entity<Address>(entity =>{
                entity.ToTable("Address");
                entity.HasKey(e => e.AddressID).HasName("PK_Address_AddressID");
                entity.Property(e => e.AddressID).UseIdentityColumn();

                entity.Property(e => e.AddressLine1).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.StateProvince).IsRequired();
                entity.Property(e => e.CountryRegion).IsRequired();
                entity.Property(e => e.PostalCode).IsRequired();
                entity.Property(e => e.rowguid).ValueGeneratedOnAdd();
                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();
            });

            # endregion
            
            # region Table [CustomerAddress]

            modelBuilder.HasDefaultSchema("SalesLT").Entity<CustomerAddress>(entity =>{
                entity.ToTable("CustomerAddress");
                // Primary Key 
                entity.HasKey(e => new {e.CustomerID , e.AddressID}).HasName("PK_CustomerAddress_CustomerID_AddressID");
                // Foreign Key 
                entity.HasOne(e => e.Customer).WithMany().HasForeignKey(e => e.CustomerID).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Address).WithMany().HasForeignKey(e => e.AddressID).IsRequired().OnDelete(DeleteBehavior.Cascade);

                // Column 
                entity.Property(e => e.CustomerID).IsRequired();
                entity.Property(e => e.AddressID).IsRequired();
                entity.Property(e => e.AddressType).IsRequired();
                entity.Property(e => e.rowguid).ValueGeneratedOnAdd();
                entity.Property(e => e.ModifiedDate).ValueGeneratedOnAdd();
            });

            # endregion

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}