using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Web.Infrastructure.Data;

public partial class YourHousebContext : DbContext
{
    public YourHousebContext(DbContextOptions<YourHousebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ChungCu> ChungCus { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<House> Houses { get; set; }

    public virtual DbSet<ImagesArticle> ImagesArticles { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tro> Tros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK_Users");

            entity.ToTable("Account");

            entity.HasIndex(e => e.AccountId, "UQ__Account__349DA5A76DA33554").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Account__5C7E359E82BA37BD").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__A9D105343CE321EF").IsUnique();

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.ImageUser).HasDefaultValue("https://cdn.pixabay.com/photo/2023/02/18/11/00/icon-7797704_640.png");
            entity.Property(e => e.PasswordHash).HasMaxLength(128);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("Article");

            entity.HasIndex(e => e.ArticleId, "UQ__Article__9C6270E9A50014B4").IsUnique();

            entity.Property(e => e.Addr).HasMaxLength(100);
            entity.Property(e => e.CreateAt).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.S).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TienCoc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.TypeAr).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithMany(p => p.Articles)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Article_Users");

            entity.HasOne(d => d.CityArNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CityAr)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Article_City");

            entity.HasOne(d => d.DistrictArNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.DistrictAr)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Article_District");
        });

        modelBuilder.Entity<ChungCu>(entity =>
        {
            entity.HasKey(e => e.ArticleId);

            entity.ToTable("ChungCu");

            entity.HasIndex(e => e.ArticleId, "UQ__ChungCu__9C6270E9F57DFF8B").IsUnique();

            entity.Property(e => e.ArticleId).ValueGeneratedNever();
            entity.Property(e => e.ElectricPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WaterPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Article).WithOne(p => p.ChungCu)
                .HasForeignKey<ChungCu>(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChungCu_Article");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.CityName).HasMaxLength(50);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK_Comment");

            entity.HasIndex(e => e.CommentId, "UQ__Comments__C3B4DFCB1A8B707B").IsUnique();

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(CONVERT([date],getdate()))");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Account");

            entity.HasOne(d => d.Article).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Article");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK_ParentComment_Comment");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.ToTable("District");

            entity.HasIndex(e => e.DistrictId, "UQ__District__85FDA4C781C30FC9").IsUnique();

            entity.Property(e => e.DistrictName).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.Districts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_District_City");
        });

        modelBuilder.Entity<House>(entity =>
        {
            entity.HasKey(e => e.ArticleId);

            entity.ToTable("House");

            entity.HasIndex(e => e.ArticleId, "UQ__House__9C6270E9C43B0FAB").IsUnique();

            entity.Property(e => e.ArticleId).ValueGeneratedNever();

            entity.HasOne(d => d.Article).WithOne(p => p.House)
                .HasForeignKey<House>(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_House_Article");
        });

        modelBuilder.Entity<ImagesArticle>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.ToTable("ImagesArticle");

            entity.HasOne(d => d.Article).WithMany(p => p.ImagesArticles)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagesArticle_Article");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.ArticleId);

            entity.ToTable("Office");

            entity.HasIndex(e => e.ArticleId, "UQ__Office__9C6270E9818B3FF7").IsUnique();

            entity.Property(e => e.ArticleId).ValueGeneratedNever();

            entity.HasOne(d => d.Article).WithOne(p => p.Office)
                .HasForeignKey<Office>(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Office_Article");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160F14415AB").IsUnique();

            entity.HasIndex(e => e.RoleId, "UQ__Roles__8AFACE1B6710CF8E").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Tro>(entity =>
        {
            entity.HasKey(e => e.ArticleId);

            entity.ToTable("Tro");

            entity.HasIndex(e => e.ArticleId, "UQ__Tro__9C6270E9A3D3D227").IsUnique();

            entity.Property(e => e.ArticleId).ValueGeneratedNever();
            entity.Property(e => e.ElectricPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WaterPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Article).WithOne(p => p.Tro)
                .HasForeignKey<Tro>(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tro_Article");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
