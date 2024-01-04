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
            entity.HasKey(e => e.SubjectId).HasName("PK__Courses__AC1BA388AFB31241");

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
            entity.HasKey(e => e.EduverseId).HasName("PK__Credenti__EB5B5E105227CC08");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Credenti__4849DA010ECF2371").IsUnique();

            entity.HasIndex(e => e.EmailId, "UQ__Credenti__87355E734208C9CD").IsUnique();

            entity.Property(e => e.EduverseId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmailId)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("emailId");
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

            entity.HasOne(d => d.Institutitional).WithMany(p => p.Credentials)
                .HasForeignKey(d => d.InstitutitionalId)
                .HasConstraintName("FK__Credentia__insti__403A8C7D");
        });

        modelBuilder.Entity<EduverseRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Eduverse__8AFACE1A50E216FA");

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
            entity.HasKey(e => e.FieldId).HasName("PK__FieldsOf__C8B6FF275FB485AB");

            entity.ToTable("FieldsOfStudy");

            entity.Property(e => e.FieldId).HasColumnName("FieldID");
            entity.Property(e => e.FieldName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasKey(e => e.FolderId).HasName("PK__Folders__C2FABF93FB07023B");

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
            entity.HasKey(e => e.GenreId).HasName("PK__Genre__0385055EBF707D65");

            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.GenreName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NotesId).HasName("PK__Notes__CFE31686B50A70D2");

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
            entity.HasKey(e => e.InstitutitionalId).HasName("PK__Register__4B1CCF2659C98D25");

            entity.Property(e => e.InstitutitionalId).HasColumnName("institutitionalId");
            entity.Property(e => e.InstituteName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("instituteName");
            entity.Property(e => e.Logo)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("logo");
            entity.Property(e => e.Status)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<SmtpMailCredential>(entity =>
        {
            entity.HasKey(e => e.SmtpMailCredentialsId).HasName("PK__smtpMail__ED725F10FBB50483");

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
            entity.HasKey(e => e.ItemId).HasName("PK__SubItems__56A128AA8675236B");

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
            entity.HasKey(e => e.SubgenreId).HasName("PK__Subgenre__25469A99F147A4C5");

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
            entity.HasKey(e => e.Id).HasName("PK__Temp_OTP__3214EC07057013AA");

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
