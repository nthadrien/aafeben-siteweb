
using Aafeben.Models;
using Microsoft.EntityFrameworkCore;

namespace Aafeben.Data
{
    public class AafebenDbContext : DbContext
    {
        public AafebenDbContext (DbContextOptions<AafebenDbContext> options) : base(options)
        {  
        }

        public DbSet<BlogPostModel> BlogPosts { get; set; }
        // public DbSet<PartnerModel> Partners { get; set; }
        public DbSet<OpportunityModel> Opportunities { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<PartnerModel> Partners { get; set; }
        public DbSet<PublicationModel> Publications { get; set; }
        public DbSet<ProjectModel> Projets { get; set; }
        public DbSet<MediaModel> Medias { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>().HasIndex(u => u.Name).IsUnique();

            modelBuilder.Entity<PartnerModel>().HasIndex(u => u.Name).IsUnique();

            modelBuilder.Entity<BlogPostModel>().HasIndex(u => u.Title).IsUnique();

            modelBuilder.Entity<PublicationModel>().HasIndex(u => u.Title).IsUnique();

            modelBuilder.Entity<ProjectModel>().HasIndex(u => u.Title).IsUnique();
        }

        
    }
}

// dotnet aspnet-codegenerator controller -name BlogsController -m BlogPostModel -dc Aafeben.Data.AafebenDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlServer
// dotnet aspnet-codegenerator controller -name TagsController -m TagModel -dc Aafeben.Data.AafebenDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlServer
// dotnet aspnet-codegenerator controller -name OpportunityController -m OpportunityModel -dc Aafeben.Data.AafebenDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlServer
// dotnet aspnet-codegenerator controller -name staffController -m UserModel -dc Aafeben.Data.AafebenDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlServer
// dotnet aspnet-codegenerator controller -name PartnersController -m PartnerModel -dc Aafeben.Data.AafebenDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlServer


// dotnet ef database drop -f -v
// dotnet ef migrations add Initial
// dotnet ef database update

// <script>
// var model = @Html.Raw(JsonSerializer.Serialize(Model));
// </script>

// dotnet aspnet-codegenerator controller -name GrenierController -m PublicationModel -dc Aafeben.Data.AafebenDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlServer

