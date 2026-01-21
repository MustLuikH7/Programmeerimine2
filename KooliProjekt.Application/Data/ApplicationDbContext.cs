using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Document> Documents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore the inherited Id property and use entity-specific keys
            modelBuilder.Entity<User>().Ignore(u => u.Id);
            modelBuilder.Entity<Doctor>().Ignore(d => d.Id);
            modelBuilder.Entity<DoctorSchedule>().Ignore(ds => ds.Id);
            modelBuilder.Entity<Appointment>().Ignore(a => a.Id);
            modelBuilder.Entity<Invoice>().Ignore(i => i.Id);
            modelBuilder.Entity<InvoiceItem>().Ignore(ii => ii.Id);
            modelBuilder.Entity<Document>().Ignore(d => d.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.Email)
                .IsUnique();

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Doctor)
                .WithMany(doc => doc.Documents)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Doctor)
                .WithMany(d => d.Invoices)
                .HasForeignKey(i => i.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invoices)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}