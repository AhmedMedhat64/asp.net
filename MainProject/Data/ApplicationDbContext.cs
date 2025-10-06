using Microsoft.EntityFrameworkCore;

namespace MainProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /* HasKey tells the Primary key but by default it looks for (Id or ((EntityName)Id)) it is not necessery
            modelBuilder.Entity<Product>().ToTable("Products").HasKey("Id"); */
            // standered to make the entity singular and the table is plural
            // the Entity is (Product) and the Table name is (Products) 
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions").HasKey(x => new { x.UserId, x.PermissionId});
            // if you give the Entity and the Table the same name then
            // you can make it like this -> here you suppose that 
            // the entity and the table are named (Product)
            /* modelBuilder.Entity<Product>(); */

        }
    }
}
