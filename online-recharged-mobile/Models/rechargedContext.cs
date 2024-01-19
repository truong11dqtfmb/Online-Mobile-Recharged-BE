using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace online_recharged_mobile.Models
{
    public partial class rechargedContext : DbContext
    {
        public rechargedContext()
        {
        }

        public rechargedContext(DbContextOptions<rechargedContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<ServiceProvider> ServiceProviders { get; set; } = null!;
        public virtual DbSet<Subcription> Subcriptions { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ModifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modify_at");

                entity.Property(e => e.ModifyBy)
                    .HasMaxLength(50)
                    .HasColumnName("modify_by");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_feedback_user");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ModifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modify_at");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<ServiceProvider>(entity =>
            {
                entity.ToTable("ServiceProvider");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminDiscount).HasColumnName("admin_discount");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("create_by");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ModifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modify_at");

                entity.Property(e => e.ModifyBy)
                    .HasMaxLength(50)
                    .HasColumnName("modify_by");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UserDiscount).HasColumnName("user_discount");
            });

            modelBuilder.Entity<Subcription>(entity =>
            {
                entity.ToTable("Subcription");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("create_by");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ModifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modify_at");

                entity.Property(e => e.ModifyBy)
                    .HasMaxLength(50)
                    .HasColumnName("modify_by");

                entity.Property(e => e.ProviderId).HasColumnName("provider_id");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.Subcriptions)
                    .HasForeignKey(d => d.ProviderId)
                    .HasConstraintName("fk_subcription_provider");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ModifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modify_at");

                entity.Property(e => e.ModifyBy)
                    .HasMaxLength(50)
                    .HasColumnName("modify_by");

                entity.Property(e => e.SubcriptionId).HasColumnName("subcription_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Subcription)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.SubcriptionId)
                    .HasConstraintName("fk_transaction_subcription");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_transaction_user");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ModifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modify_at");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .HasColumnName("phone");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("fk_userrole_role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_userrole_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
