using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClientTicketing.Core.Models;
using ClientTicketing.Core.Interfaces;

namespace ClientTicketing.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
{
    private readonly ITenantService? _tenantService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService? tenantService = null) 
        : base(options)
    {
        _tenantService = tenantService;
    }


        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tenant configuration
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Subdomain).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Subdomain).IsUnique();
                entity.Property(e => e.CustomDomain).HasMaxLength(100);
            });

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.HasOne(e => e.Tenant)
                      .WithMany(e => e.Users)
                      .HasForeignKey(e => e.TenantId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Client configuration
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Company).HasMaxLength(100);
                entity.HasOne(e => e.Tenant)
                      .WithMany(e => e.Clients)
                      .HasForeignKey(e => e.TenantId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Multi-tenant filter - only apply if tenant service is available
                if (_tenantService != null)
                {
                    entity.HasQueryFilter(e => e.TenantId == _tenantService.GetCurrentTenantId());
                }
            });

            // Ticket configuration
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                
                entity.HasOne(e => e.Tenant)
                      .WithMany()
                      .HasForeignKey(e => e.TenantId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Client)
                      .WithMany(e => e.Tickets)
                      .HasForeignKey(e => e.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.AssignedTo)
                      .WithMany(e => e.AssignedTickets)
                      .HasForeignKey(e => e.AssignedToId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(e => e.CreatedBy)
                      .WithMany(e => e.CreatedTickets)
                      .HasForeignKey(e => e.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);

                // Multi-tenant filter
                if (_tenantService != null)
                {
                    entity.HasQueryFilter(e => e.TenantId == _tenantService.GetCurrentTenantId());
                }
            });

            // TaskItem configuration
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                
                entity.HasOne(e => e.Tenant)
                      .WithMany()
                      .HasForeignKey(e => e.TenantId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Ticket)
                      .WithMany()
                      .HasForeignKey(e => e.TicketId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasOne(e => e.CreatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.AssignedTo)
                      .WithMany()
                      .HasForeignKey(e => e.AssignedToId)
                      .OnDelete(DeleteBehavior.SetNull);

                if (_tenantService != null)
                {
                    entity.HasQueryFilter(e => e.TenantId == _tenantService.GetCurrentTenantId());
                }
            });

            // TicketComment configuration
            modelBuilder.Entity<TicketComment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                
                entity.HasOne(e => e.Ticket)
                      .WithMany(e => e.Comments)
                      .HasForeignKey(e => e.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // TimeEntry configuration
            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                
                entity.HasOne(e => e.Ticket)
                      .WithMany(e => e.TimeEntries)
                      .HasForeignKey(e => e.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Subscription configuration
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MonthlyPrice).HasColumnType("decimal(10,2)");
                entity.Property(e => e.StripeSubscriptionId).HasMaxLength(100);
                
                entity.HasOne(e => e.Tenant)
                      .WithMany()
                      .HasForeignKey(e => e.TenantId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default tenant for development
            modelBuilder.Entity<Tenant>().HasData(
                new Tenant
                {
                    Id = 1,
                    Name = "Default Company",
                    Subdomain = "demo",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    Plan = SubscriptionPlan.Free,
                    MaxUsers = 5,
                    MaxTickets = 100,
                    CurrentUsers = 0,
                    CurrentTickets = 0
                }
            );

            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN", Description = "System Administrator" },
                new Role { Id = 2, Name = "Manager", NormalizedName = "MANAGER", Description = "Team Manager" },
                new Role { Id = 3, Name = "Agent", NormalizedName = "AGENT", Description = "Support Agent" },
                new Role { Id = 4, Name = "Client", NormalizedName = "CLIENT", Description = "Client User" }
            );
        }
    }
}