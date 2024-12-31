using Microsoft.EntityFrameworkCore;
using AthleteTracker.Models;

// this allow pages to acces data
namespace AthleteTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<SportsCenter> SportsCenters { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<PaymentPlan> PaymentPlans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<InstructorBranch> InstructorBranches { get; set; }
        public DbSet<DevelopmentRecord> DevelopmentRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Parent>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Parent>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Student>(entity =>
                {
                    entity.HasKey(e => e.StudentId);

                    entity.HasOne(d => d.Parent)
                        .WithMany(p => p.Students)
                        .HasForeignKey(d => d.ParentId);

                    entity.HasMany(d => d.Enrollments)
                        .WithOne(p => p.Student)
                        .HasForeignKey(d => d.StudentId);
                });


            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InstructorBranch>()
                        .HasOne(ib => ib.Instructor)
                        .WithMany(i => i.InstructorBranches)
                        .HasForeignKey(ib => ib.InstructorId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InstructorBranch>()
                        .HasOne(ib => ib.Branch)
                        .WithMany(b => b.InstructorBranches)
                        .HasForeignKey(ib => ib.BranchId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InstructorBranch>()
                        .HasIndex(ib => new { ib.InstructorId, ib.BranchId })
                        .IsUnique();

            modelBuilder.Entity<Instructor>()
                        .HasOne(i => i.User)
                        .WithOne()
                        .HasForeignKey<Instructor>(i => i.UserId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Center)
                .WithMany(sc => sc.Branches)
                .HasForeignKey(b => b.CenterId);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Branch)
                .WithMany(b => b.Sessions)
                .HasForeignKey(s => s.BranchId);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Instructor)
                .WithMany()
                .HasForeignKey(s => s.InstructorId);

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.StudentId);

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.SessionId);
            });
            // modelBuilder.Entity<Enrollment>()
            //     .HasOne(e => e.Student)
            //     .WithMany()
            //     .HasForeignKey(e => e.StudentId);
            //
            // modelBuilder.Entity<Enrollment>()
            //         .HasOne(e => e.Session)
            //         .WithMany(s => s.Enrollments)
            //         .HasForeignKey(e => e.SessionId)
            //         .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentPlan>()
                .HasOne(p => p.Enrollment)
                .WithOne(e => e.PaymentPlan)
                .HasForeignKey<PaymentPlan>(p => p.EnrollmentId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Plan)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.PlanId);

            modelBuilder.Entity<DevelopmentRecord>()
                       .HasOne(d => d.Student)
                       .WithMany()
                       .HasForeignKey(d => d.StudentId)
                       .OnDelete(DeleteBehavior.Restrict);

            // Index for better querry speed
            modelBuilder.Entity<DevelopmentRecord>()
                .HasIndex(d => d.StudentId);


            // Configure timestamp columns
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    property.SetColumnType("timestamp with time zone");
                }
            }
        }
    }
}
