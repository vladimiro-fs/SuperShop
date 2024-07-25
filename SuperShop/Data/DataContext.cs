namespace SuperShop.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using SuperShop.Data.Entities;

    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Injectar o nosso DataContext no DbContextOptions e usar a herança para dizer que o nosso DataContext herda do DbContextOptions
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)  
        {                                                                           

        } 
    }
}
