﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Throw.Models
{
    public partial class ThrowSQLContext : DbContext
    {
        public ThrowSQLContext()
        {
        }

        public ThrowSQLContext(DbContextOptions<ThrowSQLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectSnapshot> ProjectSnapshot { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserProject> UserProject { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=ThrowSQL;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Idproject);

                entity.Property(e => e.Idproject).HasColumnName("IDProject");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<ProjectSnapshot>(entity =>
            {
                entity.HasKey(e => e.Idsnapshot);

                entity.Property(e => e.Idsnapshot)
                    .HasColumnName("IDSnapshot")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Idproject).HasColumnName("IDProject");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.IdprojectNavigation)
                    .WithMany(p => p.ProjectSnapshot)
                    .HasForeignKey(d => d.Idproject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjekatSnapshot_Projekat");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Iduser);

                entity.HasIndex(e => e.Iduser)
                    .HasName("IX_Korisnik");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Photo)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserProject>(entity =>
            {
                entity.HasKey(e => new { e.Iduser, e.Idproject });

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.Idproject).HasColumnName("IDProject");

                entity.Property(e => e.Accessed).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.HasOne(d => d.IdprojectNavigation)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.Idproject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KorisnikProjekat_Projekat");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.Iduser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KorisnikProjekat_Korisnik");
            });
        }
    }
}
