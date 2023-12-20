using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestApiHomeTask;

public partial class HomeTaskDbContext : DbContext
{
    public HomeTaskDbContext()
    {
    }

    public HomeTaskDbContext(DbContextOptions<HomeTaskDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Database=HomeTaskDB; Username=postgres; Password=adminS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Id)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("items_pkey");

            entity.ToTable("items");

            entity.Property(e => e.Id)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Categoryid)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("categoryid");
            entity.Property(e => e.Details)
                .HasMaxLength(40)
                .HasColumnName("details");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Items)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("items_categoryid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
