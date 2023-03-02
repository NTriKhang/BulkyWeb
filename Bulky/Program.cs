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

		builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
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
			option.AppId = "696804982126158";

			option.AppSecret = "a2e91209f42c69ee4f4999db0a0ff5ef";
        });
		builder.Services.AddAuthentication().AddGoogle(option =>
		{
			option.ClientId = "209859043503-ohjavkf6q7adrp6e173db2errbremc0u.apps.googleusercontent.com";

			option.ClientSecret = "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";

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



