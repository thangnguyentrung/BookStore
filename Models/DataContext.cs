using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Cart>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18, 2)");           

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Cấu hình EnableSensitiveDataLogging để hiển thị thông tin chi tiết về lỗi
            optionsBuilder.EnableSensitiveDataLogging();
            // Cấu hình các tùy chọn khác cho DbContext của bạn
            // ...
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Orders { get; set; }
        //      
        //      

    }
}
