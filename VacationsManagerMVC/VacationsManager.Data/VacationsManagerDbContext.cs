using Microsoft.EntityFrameworkCore;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Enums;

namespace VacationsManager.Data
{
    public class VacationsManagerDbContext : DbContext
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }

        public VacationsManagerDbContext() { }

        public VacationsManagerDbContext(DbContextOptions<VacationsManagerDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Recipient)
                .WithMany()
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Teams)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Developers)
                .WithOne(u => u.Team)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.TeamLeader)
                .WithMany()
                .HasForeignKey(t => t.TeamLeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Developers)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VacationRequest>()
                .HasOne(vr => vr.Requester)
                .WithMany(u => u.VacationRequests)
                .HasForeignKey(vr => vr.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VacationRequest>()
                .Property(vr => vr.VacationType)
                .HasConversion<int>();

            foreach (var role in Enum.GetValues(typeof(RoleType)).Cast<RoleType>())
            {
                modelBuilder.Entity<Role>().HasData(new Role { Id = (int)role, Name = role.ToString(), RoleType = role });
            }

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                FirstName = "Admin",
                LastName = "User",
                Password = "hashedpassword",
                RoleId = (int)RoleType.CEO
            });
        }
    }
}
