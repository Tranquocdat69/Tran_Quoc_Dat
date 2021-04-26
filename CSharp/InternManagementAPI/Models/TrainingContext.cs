using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace InternManagementAPI.Models
{
    public partial class TrainingContext : DbContext
    {
        public TrainingContext()
        {
        }

        public TrainingContext(DbContextOptions<TrainingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TSchedule> TSchedules { get; set; }
        public virtual DbSet<TStudents> TStudents { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=.;Database=Training;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TSchedule>(entity =>
            {
                entity.HasKey(e => e.AScheduleId)
                    .HasName("PK__tSchedul__4B22A2E76E9CE7D9");

                entity.ToTable("tSchedule");

                entity.HasIndex(e => e.AAttendedDate)
                    .HasName("IX_aAttendedDate");

                entity.HasIndex(e => e.ASession)
                    .HasName("IX_aSession");

                entity.Property(e => e.AScheduleId).HasColumnName("aScheduleID");

                entity.Property(e => e.AAttendedDate)
                    .HasColumnName("aAttendedDate")
                    .HasColumnType("date");

                entity.Property(e => e.ACreatedDate)
                    .HasColumnName("aCreatedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ASession).HasColumnName("aSession");

                entity.Property(e => e.AStudentId).HasColumnName("aStudentID");

                entity.Property(e => e.AUpdateDate)
                    .HasColumnName("aUpdateDate")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.AStudent)
                    .WithMany(p => p.TSchedule)
                    .HasForeignKey(d => d.AStudentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__tSchedule__aStud__1273C1CD");
            });

            modelBuilder.Entity<TStudents>(entity =>
            {
                entity.HasKey(e => e.AStudentId)
                    .HasName("PK__tStudent__994BA4DE0EA3BE4C");

                entity.ToTable("tStudents");

                entity.HasIndex(e => e.AUsername)
                    .HasName("IX_aUsername");

                entity.Property(e => e.AStudentId).HasColumnName("aStudentID");

                entity.Property(e => e.ACreatedDate)
                    .HasColumnName("aCreatedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.AEmail)
                    .HasColumnName("aEmail")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AFullName)
                    .HasColumnName("aFullName")
                    .HasMaxLength(255);

                entity.Property(e => e.AUpdatedDate)
                    .HasColumnName("aUpdatedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.AUsername)
                    .HasColumnName("aUsername")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
