using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Services;
using Shop.Models;
using Microsoft.AspNetCore.Identity;
using Shop.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders().AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
});
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Mailkit"));
builder.Services.Configure<CipherOptions>(builder.Configuration.GetSection("Cryptography"));
builder.Services.AddTransient<IEmailSender, EmailService>();
builder.Services.AddScoped<CryptographyService>();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProStatusOnly", policy => policy.RequireClaim("EmailConfirmed", "True"));
});
builder.Services.AddMemoryCache();
builder.Services.AddSession();
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
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
