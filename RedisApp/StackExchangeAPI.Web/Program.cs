using StackExchange.Redis;
using StackExchangeAPI.Web.Repository;
using StackExchangeAPI.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>(sp =>
{
	return new RedisCacheService(builder.Configuration["Redis:Host"]);
});

builder.Services.AddSingleton<IDatabase>(sd =>
{
	var redisService = sd.GetService<IRedisCacheService>();
	return redisService.GetDb(0); // Set Database Index Number
});

builder.Services.AddSingleton<IProductRepository, ProductRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();