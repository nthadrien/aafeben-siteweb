
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
        public DbSet<MessageModel> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>( entity => 
            {
                entity.HasIndex(u => u.Name).IsUnique();
                entity.Property( e => e.CreatedAt ).HasDefaultValueSql("getdate()");

            });
            
            modelBuilder.Entity<PartnerModel>( entity => 
            {
                entity.HasIndex(u => u.Name).IsUnique();
                entity.Property( e => e.CreatedAt ).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<BlogPostModel>().HasIndex(u => u.Title).IsUnique();

            modelBuilder.Entity<PublicationModel>().HasIndex(u => u.Title).IsUnique();

            modelBuilder.Entity<ProjectModel>().HasIndex(u => u.Title).IsUnique();

            modelBuilder.Entity<MessageModel>().Property( e => e.SendOn ).HasDefaultValueSql("getdate()");
        }

        
    }
}
