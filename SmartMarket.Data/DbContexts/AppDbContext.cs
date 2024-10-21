using Microsoft.EntityFrameworkCore;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.Categories;
using SmartMarket.Domin.Entities.CencelOrders;
using SmartMarket.Domin.Entities.ContrAgents;
using SmartMarket.Domin.Entities.Kassas;
using SmartMarket.Domin.Entities.Orders;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Domin.Entities.Users;

namespace SmartMarket.Data.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    public DbSet<Card> Cards { get; set; }
    public DbSet<Korzinka> Korzinkas { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CencelOrder> CancelOrders { get; set; }
    public DbSet<ContrAgent> ContrAgents { get; set; }
    public DbSet<Tolov> Tolovs { get; set; }
    public DbSet<Kassa> Kassas { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Partner> Partners { get; set; }
    public DbSet<PartnerProduct> PartnerProducts { get; set; }
    public DbSet<PartnerTolov> PartnerTolovs { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<TolovUsuli> TolovUsulis { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WorkersPayment> WorkersPayment { get; set; }
    public DbSet<ProductStory> ProductStory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContrAgent>()
            .HasOne(c => c.TolovUsuli)
            .WithMany(t => t.ContrAgents)
            .HasForeignKey(c => c.TolovUsuliID)
            .OnDelete(DeleteBehavior.Restrict);
        // Configure the relationships for Korzinka entity
        modelBuilder.Entity<Korzinka>()
            .HasOne(k => k.Yiguvchi)
            .WithMany(u => u.Yiguvchi)
            .HasForeignKey(k => k.YukYiguvchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Korzinka>()
            .HasOne(k => k.YukTaxlovchi)
            .WithMany(u => u.Taxlovchi)
            .HasForeignKey(k => k.YukTaxlovchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Korzinka>()
            .HasOne(k => k.Partner)
            .WithMany(p => p.Korzinkas)
            .HasForeignKey(k => k.PartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Korzinka>()
            .HasOne(k => k.TolovUsuli)
            .WithMany(t => t.Korzinkas)
            .HasForeignKey(k => k.TolovUsulId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for Card entity
        modelBuilder.Entity<Card>()
            .HasOne(c => c.Yiguvchi)
            .WithMany(u => u.YiguvchiCards)
            .HasForeignKey(c => c.YukYiguvchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.Casher)
            .WithMany(u => u.CasherCards)
            .HasForeignKey(c => c.CasherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.YukTaxlovchi)
            .WithMany(u => u.YukTaxlovchisi)
            .HasForeignKey(c => c.YukTaxlovchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.Partner)
            .WithMany(p => p.Cards)
            .HasForeignKey(c => c.PartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.TolovUsuli)
            .WithMany(t => t.Cards)
            .HasForeignKey(c => c.TolovUsulId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Cards)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.Kassa)
            .WithMany(k => k.Cards)
            .HasForeignKey(c => c.KassaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for CencelOrder entity
        modelBuilder.Entity<CencelOrder>()
            .HasOne(co => co.Casher)
            .WithMany(u => u.CencelOrders)
            .HasForeignKey(co => co.CasherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CencelOrder>()
            .HasOne(co => co.CencelerCasher)
            .WithMany()
            .HasForeignKey(co => co.CancelerCasherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CencelOrder>()
            .HasOne(co => co.Category)
            .WithMany(cat => cat.CencelOrders)
            .HasForeignKey(co => co.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for Order entity
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Yiguvchi)
            .WithMany(u => u.YukYiguvchi)
            .HasForeignKey(o => o.YiguvchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.YukTaxlovchi)
            .WithMany(u => u.YukTaxlovchi)
            .HasForeignKey(o => o.YukTaxlovchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Category)
            .WithMany(cat => cat.Orders)
            .HasForeignKey(o => o.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Partner)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.PartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for Product entity
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(cat => cat.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ContrAgent)
            .WithMany(ca => ca.Products)
            .HasForeignKey(p => p.ContrAgentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for PartnerProduct entity
        modelBuilder.Entity<PartnerProduct>()
            .HasOne(pp => pp.Partner)
            .WithMany(p => p.PartnerProducts)
            .HasForeignKey(pp => pp.PartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PartnerProduct>()
            .HasOne(pp => pp.Category)
            .WithMany(cat => cat.PartnersProducts)
            .HasForeignKey(pp => pp.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PartnerProduct>()
            .HasOne(pp => pp.User)
            .WithMany(u => u.PartnerProducts)
            .HasForeignKey(pp => pp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PartnerProduct>()
            .HasOne(pp => pp.Yiguvchi)
            .WithMany(u => u.PartnersYukYiguvchi)
            .HasForeignKey(pp => pp.YukYiguvchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PartnerProduct>()
            .HasOne(pp => pp.YukTaxlovchi)
            .WithMany(u => u.PartnersYukTaxlovchisi)
            .HasForeignKey(pp => pp.YukTaxlovchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PartnerProduct>()
            .HasOne(pp => pp.Kassa)
            .WithMany(k => k.PartnerProducts)
            .HasForeignKey(pp => pp.KassaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PartnerProduct>()
            .HasOne(pp => pp.TolovUsuli)
            .WithMany(t => t.PartnersProduct)
            .HasForeignKey(pp => pp.TolovUsuliId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for PartnerTolov entity
        modelBuilder.Entity<PartnerTolov>()
            .HasOne(pt => pt.Partner)
            .WithMany(p => p.PartnerTolovs)
            .HasForeignKey(pt => pt.PartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for Tolov entity
        modelBuilder.Entity<Tolov>()
            .HasOne(t => t.ContrAgent)
            .WithMany(ca => ca.Tolovs)
            .HasForeignKey(t => t.ContrAgentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for TolovUsuli entity
        modelBuilder.Entity<TolovUsuli>()
            .HasOne(tu => tu.Kassa)
            .WithMany(u => u.Tolovs)
            .HasForeignKey(tu => tu.KassaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationships for WorkersPayment entity
        modelBuilder.Entity<WorkersPayment>()
            .HasOne(wp => wp.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(wp => wp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkersPayment>()
            .HasOne(wp => wp.User)
            .WithMany(p => p.Payments)
            .HasForeignKey(wp => wp.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
