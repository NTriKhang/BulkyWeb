using Bulky.DataAccess;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bulky.Utility;
using Stripe;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddControllersWithViews();

		var connection = builder.Configuration.GetConnectionString("DefaultConnect");
		var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

		builder.Services.AddDbContext<Bulky.DataAccess.Application>(option => option.UseMySql(connection, serverVersion));
		builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<Bulky.DataAccess.Application>();
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
		builder.Services.AddTransient<IEmailSender, EmailSender>();
		builder.Services.AddRazorPages();
		builder.Services.AddDistributedMemoryCache();
		builder.Services.AddSession(option =>
		{
			option.IdleTimeout = TimeSpan.FromSeconds(1);
			option.Cookie.HttpOnly = true;
			option.Cookie.IsEssential = true;
		});
		
		builder.Services.ConfigureApplicationCookie(option =>
		{
			option.LoginPath = $"/Identity/Account/Login";
			option.LogoutPath = $"/Identity/Account/Logout";
			option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
		}
		);
		builder.Services.AddAuthentication().AddFacebook(option =>
		{
			option.AppId = builder.Configuration.GetSection("Facebook:AppId").Get<string>();
			option.AppSecret = builder.Configuration.GetSection("Facebook:AppSecret").Get<string>();
        });
		builder.Services.AddAuthentication().AddGoogle(option =>
		{
			option.ClientId = builder.Configuration.GetSection("Google:ClientId").Get<string>();
			option.ClientSecret = builder.Configuration.GetSection("Google:ClientSecret").Get<string>();
		});
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();
		StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
		app.UseAuthentication();
		app.UseAuthorization();
		app.UseSession();
		app.MapRazorPages();
		app.MapControllerRoute(
			name: "default",
			pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}



