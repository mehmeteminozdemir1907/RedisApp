using DistributedCache.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ConfigureServices

    // Redis Configure
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "127.0.0.1:6379";
});

builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure

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
