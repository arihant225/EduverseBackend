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

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Credential> Credentials { get; set; }

    public virtual DbSet<EduverseRole> EduverseRoles { get; set; }

    public virtual DbSet<FieldsOfStudy> FieldsOfStudies { get; set; }

    public virtual DbSet<Folder> Folders { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<InstitutionalDomain> InstitutionalDomains { get; set; }

    public virtual DbSet<InstitutionalRole> InstitutionalRoles { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<RegisterdInstitute> RegisterdInstitutes { get; set; }

    public virtual DbSet<SmtpMailCredential> SmtpMailCredentials { get; set; }

    public virtual DbSet<SubItem> SubItems { get; set; }

    public virtual DbSet<Subgenre> Subgenres { get; set; }

    public virtual DbSet<TempOtp> TempOtps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=areyhant;initial catalog=Eduverse;integrated security=true;trustservercertificate=false;trusted_connection=true;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Courses__AC1BA3887488664B");

            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.FieldId).HasColumnName("FieldID");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Field).WithMany(p => p.Courses)
                .HasForeignKey(d => d.FieldId)
                .HasConstraintName("FK__Courses__FieldID__5441852A");
        });

        modelBuilder.Entity<Credential>(entity =>
        {
            entity.HasKey(e => e.EduverseId).HasName("PK_Credentials_EduverseId");

            entity.HasIndex(e => e.Guidaccessor, "UQ__Credenti__832162033034EB1B").IsUnique();

            entity.Property(e => e.EduverseId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmailId)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("emailId");
            entity.Property(e => e.Guidaccessor)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("GUIDAccessor");
            entity.Property(e => e.InstitutitionalId).HasColumnName("institutitionalId");
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
            entity.Property(e => e.Status)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Institutitional).WithMany(p => p.Credentials)
                .HasForeignKey(d => d.InstitutitionalId)
                .HasConstraintName("FK_Credentials_RegisterdInstitutes");
        });

        modelBuilder.Entity<EduverseRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Eduverse__8AFACE1A0F2F83C8");

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

        modelBuilder.Entity<FieldsOfStudy>(entity =>
        {
            entity.HasKey(e => e.FieldId).HasName("PK__FieldsOf__C8B6FF2776EBC718");

            entity.ToTable("FieldsOfStudy");

            entity.Property(e => e.FieldId).HasColumnName("FieldID");
            entity.Property(e => e.FieldName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasKey(e => e.FolderId).HasName("PK__Folders__C2FABF9385FCB791");

            entity.Property(e => e.FolderId).HasColumnName("folderId");
            entity.Property(e => e.FolderName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("folderName");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Folders)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK__Folders__userid__59FA5E80");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genre__0385055E28B01200");

            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.GenreName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InstitutionalDomain>(entity =>
        {
            entity.HasKey(e => e.DomainId).HasName("PK__Institut__4A894871C30912AF");

            entity.Property(e => e.DomainId).HasColumnName("domainId");
            entity.Property(e => e.DomainName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InstituteId).HasColumnName("instituteId");
            entity.Property(e => e.ParentDomainId).HasColumnName("parentDomainId");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Institute).WithMany(p => p.InstitutionalDomains)
                .HasForeignKey(d => d.InstituteId)
                .HasConstraintName("FK__Instituti__insti__628FA481");

            entity.HasOne(d => d.ParentDomain).WithMany(p => p.InverseParentDomain)
                .HasForeignKey(d => d.ParentDomainId)
                .HasConstraintName("FK__Instituti__paren__619B8048");
        });

        modelBuilder.Entity<InstitutionalRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Institut__8AFACE1A3DD0248C");

            entity.Property(e => e.EduverseId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Eduverse).WithMany(p => p.InstitutionalRoles)
                .HasForeignKey(d => d.EduverseId)
                .HasConstraintName("FK__Instituti__Eduve__656C112C");

            entity.HasOne(d => d.Institutional).WithMany(p => p.InstitutionalRoles)
                .HasForeignKey(d => d.InstitutionalId)
                .HasConstraintName("FK__Instituti__Insti__66603565");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NotesId).HasName("PK__Notes__CFE31686EB85DC17");

            entity.Property(e => e.NotesId).HasColumnName("notesId");
            entity.Property(e => e.Body)
                .IsUnicode(false)
                .HasColumnName("body");
            entity.Property(e => e.BodyStyle)
                .IsUnicode(false)
                .HasColumnName("bodyStyle");
            entity.Property(e => e.IsPrivate).HasColumnName("isPrivate");
            entity.Property(e => e.Title)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.TitleStyle)
                .IsUnicode(false)
                .HasColumnName("titleStyle");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notes__userId__571DF1D5");
        });

        modelBuilder.Entity<RegisterdInstitute>(entity =>
        {
            entity.HasKey(e => e.InstitutitionalId).HasName("PK__Register__4B1CCF264A6301F3");

            entity.HasIndex(e => e.Guidaccessor, "UQ__Register__83216203ADFEBF6D").IsUnique();

            entity.Property(e => e.InstitutitionalId).HasColumnName("institutitionalId");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.Email)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Guidaccessor)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("GUIDAccessor");
            entity.Property(e => e.InstituteName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("instituteName");
            entity.Property(e => e.InstituteType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("instituteType");
            entity.Property(e => e.Logo)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("logo");
            entity.Property(e => e.PhoneNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Url)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SmtpMailCredential>(entity =>
        {
            entity.HasKey(e => e.SmtpMailCredentialsId).HasName("PK__smtpMail__ED725F10DC40A2EE");

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

        modelBuilder.Entity<SubItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__SubItems__56A128AA723F5F9D");

            entity.Property(e => e.ItemId).HasColumnName("itemId");
            entity.Property(e => e.FolderId).HasColumnName("folderId");

            entity.HasOne(d => d.Folder).WithMany(p => p.SubItemFolders)
                .HasForeignKey(d => d.FolderId)
                .HasConstraintName("FK__SubItems__folder__5EBF139D");

            entity.HasOne(d => d.LinkedFolder).WithMany(p => p.SubItemLinkedFolders)
                .HasForeignKey(d => d.LinkedFolderId)
                .HasConstraintName("FK__SubItems__Linked__5CD6CB2B");

            entity.HasOne(d => d.Note).WithMany(p => p.SubItems)
                .HasForeignKey(d => d.NoteId)
                .HasConstraintName("FK__SubItems__NoteId__5DCAEF64");
        });

        modelBuilder.Entity<Subgenre>(entity =>
        {
            entity.HasKey(e => e.SubgenreId).HasName("PK__Subgenre__25469A99DA7B5BAC");

            entity.ToTable("Subgenre");

            entity.Property(e => e.SubgenreId).HasColumnName("SubgenreID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.SubgenreName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Genre).WithMany(p => p.Subgenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Subgenre__GenreI__4F7CD00D");
        });

        modelBuilder.Entity<TempOtp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Temp_OTP__3214EC07A31225B4");

            entity.ToTable("Temp_OTPs");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GeneratedTimeStamp).HasColumnType("datetime");
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
