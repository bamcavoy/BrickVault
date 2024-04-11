using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BrickVault.Models;

public partial class IntexDbContext : IdentityDbContext
{
    public IntexDbContext()
    {
        
    }

    public IntexDbContext(DbContextOptions<IntexDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<ItemRecommendation> ItemRecommendations { get; set; }
    public virtual DbSet<LineItem> LineItems { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    public virtual DbSet<UserRecommendation> UserRecommendations { get; set; }
    public IQueryable<AspNetUser> AspNetUsers { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            // entity.HasMany(d => d.Roles).WithMany(p => p.Users)
            //     .UsingEntity<Dictionary<string, object>>(
            //         "AspNetUserRole",
            //         r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
            //         l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
            //         j =>
            //         {
            //             j.HasKey("UserId", "RoleId");
            //             j.ToTable("AspNetUserRoles");
            //             j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
            //         });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_ID");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CountryOfResidence)
                .HasMaxLength(50)
                .HasColumnName("country_of_residence");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<ItemRecommendation>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Recommendation1).HasColumnName("recommendation_1");
            entity.Property(e => e.Recommendation2).HasColumnName("recommendation_2");
            entity.Property(e => e.Recommendation3).HasColumnName("recommendation_3");
            entity.Property(e => e.Recommendation4).HasColumnName("recommendation_4");
            entity.Property(e => e.Recommendation5).HasColumnName("recommendation_5");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemRecommendations_Products");
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_ID");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineItems_Products");

            entity.HasOne(d => d.Transaction).WithMany()
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LineItems_Orders");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.Property(e => e.TransactionId)
                .ValueGeneratedNever()
                .HasColumnName("transaction_ID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Bank)
                .HasMaxLength(50)
                .HasColumnName("bank");
            entity.Property(e => e.CountryOfTransaction)
                .HasMaxLength(50)
                .HasColumnName("country_of_transaction");
            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(50)
                .HasColumnName("day_of_week");
            entity.Property(e => e.EntryMode)
                .HasMaxLength(50)
                .HasColumnName("entry_mode");
            entity.Property(e => e.Fraud).HasColumnName("fraud");
            entity.Property(e => e.ShippingAddress)
                .HasMaxLength(50)
                .HasColumnName("shipping_address");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.TypeOfCard)
                .HasMaxLength(50)
                .HasColumnName("type_of_card");
            entity.Property(e => e.TypeOfTransaction)
                .HasMaxLength(50)
                .HasColumnName("type_of_transaction");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId); // Ensure that ProductId is the primary key
            entity.Property(e => e.ProductId)
                .HasColumnName("product_ID")
                .ValueGeneratedOnAdd(); // Configure ProductId as an identity column

            entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
            entity.Property(e => e.Description)
                .HasMaxLength(2750)
                .HasColumnName("description");
            entity.Property(e => e.ImgLink)
                .HasMaxLength(3500)
                .HasColumnName("img_link");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NumParts).HasColumnName("num_parts");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.PrimaryColor)
                .HasMaxLength(50)
                .HasColumnName("primary_color");
            entity.Property(e => e.SecondaryColor)
                .HasMaxLength(50)
                .HasColumnName("secondary_color");
            entity.Property(e => e.Year).HasColumnName("year");
        });


        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Product-Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_ID");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");

            entity.HasOne(d => d.Category).WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product-Category_Category");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product-Category_Products");
        });

        modelBuilder.Entity<UserRecommendation>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CustomerId).HasColumnName("customer_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Recommendation1).HasColumnName("recommendation_1");
            entity.Property(e => e.Recommendation2).HasColumnName("recommendation_2");
            entity.Property(e => e.Recommendation3).HasColumnName("recommendation_3");
            entity.Property(e => e.Recommendation4).HasColumnName("recommendation_4");
            entity.Property(e => e.Recommendation5).HasColumnName("recommendation_5");

            entity.HasOne(d => d.Customer).WithMany()
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRecommendations_Customers");
        });
        
        //OnModelCreatingPartial(modelBuilder);
        
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
