using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bothniabladet.Models.ImageModels;
using System;

namespace Bothniabladet.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<SectionEnum> Enums { get; set; }
        public DbSet<ImageKeyword> ImageKeywords { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<EditedImage> EditedImages { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesDocument>()
                .HasKey(x => x.SalesDocumentId);

            modelBuilder.Entity<Image>()
                .Property(c => c.Section)
                .HasConversion<string>();

            modelBuilder.Entity<Image>()
                .OwnsOne(s => s.ImageLicense);

            modelBuilder.Entity<Image>()
                .OwnsOne(s => s.ImageMetaData);

            modelBuilder.Entity<Image>()
                .HasMany(c => c.EditedImages)
                .WithOne(s => s.Image);

            modelBuilder.Entity<ImageKeyword>()
                .HasKey(x => new { x.ImageId, x.KeywordId });

            modelBuilder.Entity<ShoppingCart>()
                .HasKey(iu => new { iu.ImageId, iu.UserId });

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(iu => iu.User)
                .WithMany(u => u.ShoppingCart)
                .HasForeignKey(iu => iu.UserId);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(si => si.Image)
                .WithMany(i => i.ShoppingCart)
                .HasForeignKey(si => si.ImageId);

            modelBuilder.Entity<UserDocuments>()
                .HasKey(iu => new { iu.SalesDocumentId, iu.UserId });

            modelBuilder.Entity<UserDocuments>()
                .HasOne(iu => iu.User)
                .WithMany(u => u.UserDocuments)
                .HasForeignKey(iu => iu.UserId);

            modelBuilder.Entity<UserDocuments>()
                .HasOne(si => si.SalesDocument)
                .WithMany(i => i.UserDocuments)
                .HasForeignKey(si => si.SalesDocumentId);

            modelBuilder.Entity<SectionEnum>()
                .ToTable("Enums")
                .HasKey(c => c.SectionEnumId);

            modelBuilder.Entity<SectionEnum>()
                .Property(s => s.Name)
                .HasConversion<string>();

            modelBuilder.Entity<SectionEnum>().HasData(
                new SectionEnum
                {
                    SectionEnumId = 1,
                    Name = NewsSection.CULTURE
                },
                new SectionEnum
                {
                    SectionEnumId = 2,
                    Name = NewsSection.ECONOMY
                },
                new SectionEnum
                {
                    SectionEnumId = 3,
                    Name = NewsSection.NEWS
                },
                new SectionEnum
                {
                    SectionEnumId = 4,
                    Name = NewsSection.INTERNATIONAL
                },
                new SectionEnum
                {
                    SectionEnumId = 5,
                    Name = NewsSection.SPORTS
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
