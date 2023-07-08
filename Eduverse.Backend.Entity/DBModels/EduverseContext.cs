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

    public virtual DbSet<EduverseRole> EduverseRoles { get; set; }

    public virtual DbSet<SmtpMailCredential> SmtpMailCredentials { get; set; }

    public virtual DbSet<Streamer> Streamers { get; set; }

    public virtual DbSet<TempOtp> TempOtps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=areyhant;initial catalog=Eduverse;integrated security=true;trustservercertificate=false;trusted_connection=true;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Credential>(entity =>
        {
            entity.HasKey(e => e.EduverseId).HasName("PK__Credenti__EB5B5E10AC79A850");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Credenti__4849DA01E7C2AD8C").IsUnique();

            entity.HasIndex(e => e.EmailId, "UQ__Credenti__87355E735F6558D7").IsUnique();

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

        modelBuilder.Entity<EduverseRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Eduverse__8AFACE1A40995F95");

            entity.HasIndex(e => new { e.Role, e.EduverseId }, "UNIQUE_EDUVERSEROLE_CREDENTIALS").IsUnique();

            entity.Property(e => e.EduverseId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EduverseID");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Eduverse).WithMany(p => p.EduverseRoles)
                .HasForeignKey(d => d.EduverseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NN_FK_CREDERNTIALROLES_CREDENTIALS");
        });

        modelBuilder.Entity<SmtpMailCredential>(entity =>
        {
            entity.HasKey(e => e.SmtpMailCredentialsId).HasName("PK__smtpMail__ED725F10D9120961");

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

        modelBuilder.Entity<Streamer>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("STREAMER");

            entity.HasIndex(e => e.EduverseId, "FK_CREDENTIALS_STREAMER").IsUnique();

            entity.Property(e => e.EduverseId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Streamerdescription)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("STREAMERDescription");
            entity.Property(e => e.Streamerid)
                .ValueGeneratedOnAdd()
                .HasColumnName("STREAMERId");
            entity.Property(e => e.Streamername)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STREAMERName");
            entity.Property(e => e.Streamertype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STREAMERType");

            entity.HasOne(d => d.Eduverse).WithOne()
                .HasForeignKey<Streamer>(d => d.EduverseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__STREAMER__Eduver__4AB81AF0");
        });

        modelBuilder.Entity<TempOtp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Temp_OTP__3214EC075EC23FF2");

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
