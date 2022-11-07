using BaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Strategy.Models;
using WebApp.Strategy.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IProductRepository>(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    var claim = httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == Settings.claimDbType).FirstOrDefault();

    var context = sp.GetRequiredService<AppIdentityDbContext>();

    if (claim == null) 
        return new ProductRepositoryFromSqlServer(context);

    var dbType = (EnumDatabaseType) int.Parse(claim.Value);

    return dbType switch
    {
        EnumDatabaseType.SqlServer => new ProductRepositoryFromSqlServer(context),
        EnumDatabaseType.MongoDb => new ProductRepositoryFromMongoDb(builder.Configuration),
        _ => throw new global::System.NotImplementedException()
    };
});
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppIdentityDbContext>(options =>  options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.User.RequireUniqueEmail = true).AddEntityFrameworkStores<AppIdentityDbContext>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using var scope = app.Services.CreateScope();
var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
identityDbContext.Database.Migrate();

if (!userManager.Users.Any())
{
    userManager.CreateAsync(new AppUser() { UserName = "user1", Email = "user1@mail.com" }, "123456Aa*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user2", Email = "user2@mail.com" }, "123456Aa*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user3", Email = "user3@mail.com" }, "123456Aa*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user4", Email = "user4@mail.com" }, "123456Aa*").Wait();
    userManager.CreateAsync(new AppUser() { UserName = "user5", Email = "user5@mail.com" }, "123456Aa*").Wait();
}

app.Run();
