using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayTrPaymentSample;
using PayTrPaymentSample.Services;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Add ApplicationDbContext with InMemory database
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

// PayTR ödeme sistemi
builder.Services.AddScoped<PayTrPaymentService>();
builder.Services.Configure<PayTrSettings>(builder.Configuration.GetSection("PayTr"));

// other services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();


var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
