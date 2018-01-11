using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemList> Lists { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ItemsListRelation> ItemsListRelations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("items");

                entity.HasIndex(e => e.Id)
                    .IsUnique();
                
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<ItemList>(entity =>
            {
                entity.ToTable("lists");

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreationDate)
                    .IsRequired()
                    .HasColumnName("creation_date")
                    .HasColumnType("DATE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Lists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<ItemsListRelation>(entity =>
            {
                entity.ToTable("items_lists");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasColumnType("boolean");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.ListId).HasColumnName("list_id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemListRelations)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.ItemList)
                    .WithMany(p => p.ItemListRelations)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Id)
                    .IsUnique();
                
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });
        }
    }
}