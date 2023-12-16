using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Physiosoft.Data
{
    public partial class PhysiosoftDbContext : DbContext
    {
        public PhysiosoftDbContext()
        {

        }

        public PhysiosoftDbContext(DbContextOptions<PhysiosoftDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Physio> Physios { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Physio>(entity =>
            {
                entity.HasKey(e => e.PhysioId).HasName("PK__PHYSIOS__8BB9145E55AAA5F5");

                entity.ToTable("PHYSIOS");

                entity.HasIndex(e => e.Lastname, "IX_PHYSIOS_Lastname");

                entity.HasIndex(e => e.Telephone, "UQ_PHYSIOS_TELEPHONE").IsUnique();

                entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
                entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
                entity.Property(e => e.Telephone)
                .HasMaxLength(50)
                .HasColumnName("telephone");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId).HasName("PK__PATIENTS__4D5CE4769C9C68C2");

                entity.ToTable("PATIENTS");

                entity.HasIndex(e => e.Lastname, "IX_PATIENTS_LASTNAME");

                entity.HasIndex(e => e.Ssn, "UQ_PATIENTS_SSN").IsUnique();

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
                entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
                entity.Property(e => e.Telephone)
                .HasMaxLength(50)
                .HasColumnName("telephone");
                entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
                entity.Property(e => e.Vat)
                 .HasMaxLength(9)
                 .HasColumnName("vat");
                entity.Property(e => e.Ssn)
                .HasMaxLength(11)
                .HasColumnName("ssn");
                entity.Property(e => e.RegNum)
                .HasMaxLength(9)
                .HasColumnName("reg_num");
                entity.Property(e => e.Notes)
                .HasMaxLength(5000)
                .HasColumnName("notes");
                entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
                entity.Property(e => e.HasReviewed)
                .HasDefaultValue(false)
                .HasColumnName("has_reviewed");
                entity.Property(e => e.PatientIssue)
                .HasMaxLength(500)
                .HasColumnName("patient_issue");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.AppointmentID).HasName("PK__APPOINTMENT");

                entity.ToTable("APPOINTMENTS");

                entity.HasIndex(e => e.AppointmentDate, "appointment_date");

                entity.HasIndex(e => e.PatientID, "patient_id");

                entity.Property(e => e.AppointmentID).HasColumnName("appointment_id");

                entity.HasOne(d => d.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Patients");

                entity.HasOne(d => d.Physio)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PhysioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Appointments_Physios");

                entity.Property(e => e.DurationMinutes)
               .HasMaxLength(3)
               .HasColumnName("duration_minutes");
                entity.Property(e => e.AppointmentStatus)
                .HasMaxLength(50)
                .HasColumnName("appointment_status");
                entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
                entity.Property(e => e.PatientIssuse)
                 .HasMaxLength(500)
                 .HasColumnName("patient_issue");
                entity.Property(e => e.HasScans)
                .IsRequired(true)
                .HasDefaultValue(false)
                .HasColumnName("has_scans");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__USERS");

                entity.ToTable("USERS");

                entity.HasIndex(e => e.Username, "username").IsUnique();
                entity.HasIndex(e => e.Email, "email").IsUnique();

                entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
                entity.Property(e => e.Password)
                 .HasMaxLength(255)
                 .HasColumnName("password");
                entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
                entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false)
                .HasColumnName("is_admin");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
