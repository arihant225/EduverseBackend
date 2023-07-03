using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Eduverse.Backend.Entity.DBModels;

public partial class EduverseContext : DbContext
{
    public EduverseContext()
    {
    }

    public EduverseContext(DbContextOptions<EduverseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Credential> Credentials { get; set; }

    public virtual DbSet<EduverseRoles> EduverseRoles { get; set; }

    public virtual DbSet<SmtpMailCredential> SmtpMailCredentials { get; set; }

    public virtual DbSet<TempOtp> TempOtps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("data source=areyhant;initial catalog=Eduverse;integrated security=true;trustservercertificate=false;trusted_connection=true;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Credential>(entity =>
        {
            entity.HasKey(e => e.EduverseId).HasName("PK__Credenti__EB5B5E10461B2CB2");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Credenti__4849DA01609DA727").IsUnique();

            entity.HasIndex(e => e.EmailId, "UQ__Credenti__87355E7311327AC3").IsUnique();

            entity.Property(e => e.EduverseId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmailId)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("emailId");
            entity.Property(e => e.Name)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasColumnType("decimal(10, 0)")
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EduverseRoles>(entity =>
        {
            entity.HasKey(e => e.EduverseRoleId).HasName("PK__Eduverse__7B5D9970125F91EA");

            entity.HasIndex(e => new { e.EduverseRole, e.EduverseId }, "UNIQUE_EDUVERSEROLE_CREDENTIALS").IsUnique();

            entity.Property(e => e.EduverseId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EduverseID");
            entity.Property(e => e.EduverseRole)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EduverseRole");

            entity.HasOne(d => d.Eduverse).WithMany(p => p.EduverseRoles)
                .HasForeignKey(d => d.EduverseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NN_FK_CREDERNTIALROLES_CREDENTIALS");
        });

        modelBuilder.Entity<SmtpMailCredential>(entity =>
        {
            entity.HasKey(e => e.SmtpMailCredentialsId).HasName("PK__smtpMail__ED725F1085A67F84");

            entity.ToTable("smtpMailCredentials");

            entity.HasIndex(e => e.Role, "UQ_Role").IsUnique();

            entity.Property(e => e.SmtpMailCredentialsId).HasColumnName("smtpMailCredentialsId");
            entity.Property(e => e.EmailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("emailId");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Port).HasColumnName("port");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Server)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("server");
        });

        modelBuilder.Entity<TempOtp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Temp_OTP__3214EC07E510BA4F");

            entity.ToTable("Temp_OTPs");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GeneratedTimeStamp).HasColumnType("date");
            entity.Property(e => e.Method)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Otp).HasColumnType("decimal(6, 0)");
        });
        modelBuilder.HasSequence("eduverseKey").StartsAt(1000L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
