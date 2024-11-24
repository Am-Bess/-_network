using Microsoft.EntityFrameworkCore;


namespace HW_6_ChatApp.Models
{
    public class Context : DbContext
    {
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Message>? Messages { get; set; }
        public Context() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder./*LogTo(Console.WriteLine)*/UseLazyLoadingProxies().UseNpgsql("Host=localhost;Username=postgres;Password=example;Database=ChatDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("massages_pkey");
                entity.ToTable("Messages");
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Text).HasColumnName("text");
                entity.Property(x => x.FromUserId).HasColumnName("from_user_id");
                entity.Property(x => x.ToUserId).HasColumnName("to_user_id");

                entity.HasOne(x => x.FromUser)
                    .WithMany(x => x.FromMessages).HasForeignKey(x => x.FromUserId).HasConstraintName("messages_from_user_id_fkey");
                entity.HasOne(x => x.ToUser)
                    .WithMany(x => x.ToMessage).HasForeignKey(x => x.ToUserId).HasConstraintName("messages_to_user_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("users_pkey");
                entity.ToTable("Users");
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Name).HasMaxLength(50).HasColumnName("name");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
