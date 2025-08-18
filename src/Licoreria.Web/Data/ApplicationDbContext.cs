using Licoreria.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Licoreria.Web.Data;

public class ApplicationUser : IdentityUser { /* extiende si necesitas */ }

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Identity fix
        builder.Entity<IdentityRole>(entity =>
        {
            entity.Property(r => r.Id).HasColumnType("nvarchar(450)");
        });

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.Id).HasColumnType("nvarchar(450)");
        });

        // Decimal precision for business entities
        builder.Entity<Product>()
            .Property(p => p.Cost)
            .HasPrecision(18, 2);

        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Entity<Product>()
            .Property(p => p.AlcoholVolumePct)
            .HasPrecision(5, 2); // ejemplo: 99.99%

        builder.Entity<Invoice>()
            .Property(i => i.Subtotal)
            .HasPrecision(18, 2);

        builder.Entity<Invoice>()
            .Property(i => i.Tax)
            .HasPrecision(18, 2);

        builder.Entity<Invoice>()
            .Property(i => i.Total)
            .HasPrecision(18, 2);

        builder.Entity<InvoiceLine>()
            .Property(il => il.UnitPrice)
            .HasPrecision(18, 2);
    }
}
