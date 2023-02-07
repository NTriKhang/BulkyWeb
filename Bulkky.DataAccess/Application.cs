using Bulky.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess
{
    public class Application : IdentityDbContext<IdentityUser>
    {
        public Application(DbContextOptions<Application> options) : base(options)
        {

        }
        public DbSet<Product> products { set; get; }
        public DbSet<Category> categories { get; set; }
        public DbSet<CoverType> coverTypes { get; set; }
		public DbSet<ApplicationUser> users { get; set; }
		public DbSet<Company> companies { get; set; }
		public DbSet<ShoppingCart2> shoppingCartsFixed { get; set; }
		public DbSet<OrderHeader> orderHeaders { get; set; }
		public DbSet<OrderDetail> orderDetail { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);
		}
	}
}
