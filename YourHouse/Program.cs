using Microsoft.EntityFrameworkCore;
using YourHouse.Application.Interfaces;
using YourHouse.Application.Services;
using YourHouse.Domain.Interfaces;
using YourHouse.Infrastructure.Repositories;
using YourHouse.Web.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<YourHousebContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("cntr")
    );
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IChungCuService, ChungCuService>();
builder.Services.AddScoped<IHouseService, HouseService>();
builder.Services.AddScoped<IOfficeService, OfficeService>();
builder.Services.AddScoped<ITroService, TroService>();
builder.Services.AddScoped<IImageArticleService, ImageArticleService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
//builder.Services.AddScoped<IArticleService, ArticleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
//});

app.Run();
