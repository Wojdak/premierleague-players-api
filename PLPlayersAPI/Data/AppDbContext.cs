using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PLPlayersAPI.Models;

namespace PLPlayersAPI.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Nationality> Nationalities { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.ClubId).HasName("PK__Clubs__D35058E7C7348FD5");

            entity.Property(e => e.BadgeSrc).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Nationality>(entity =>
        {
            entity.HasKey(e => e.NationalityId).HasName("PK__National__F628E74474058B0E");

            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.FlagSrc).HasMaxLength(255);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK__Players__4A4E74C82BB0B33E");

            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.ImgSrc).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.Club).WithMany(p => p.Players)
                .HasForeignKey(d => d.ClubId)
                .HasConstraintName("FK_Club");

            entity.HasOne(d => d.Nationality).WithMany(p => p.Players)
                .HasForeignKey(d => d.NationalityId)
                .HasConstraintName("FK_Nationality");

            entity.HasOne(d => d.Position).WithMany(p => p.Players)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK_Position");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Position__60BB9A7943366B64");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C0D376E83");

            entity.Property(e => e.PasswordHash).HasMaxLength(64);
            entity.Property(e => e.Role).HasMaxLength(40);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
