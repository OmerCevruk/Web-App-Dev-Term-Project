using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AthleteTracker.Data;
using AthleteTracker.Models;
using AthleteTracker.Authorization;
using AthleteTracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

// Add password hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Add HttpContext accessor
builder.Services.AddHttpContextAccessor();
// student selection services
builder.Services.AddScoped<IStudentService, StudentService>();
// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HR", policy =>
        policy.Requirements.Add(new DepartmentRequirement("HR", "Management")));

    options.AddPolicy("HROnly", policy =>
        policy.Requirements.Add(new DepartmentRequirement("HR")));

    options.AddPolicy("IT", policy =>
        policy.Requirements.Add(new DepartmentRequirement("IT", "Management")));

    options.AddPolicy("ITOnly", policy =>
        policy.Requirements.Add(new DepartmentRequirement("IT")));

    options.AddPolicy("Coach", policy =>
        policy.Requirements.Add(new DepartmentRequirement("Coach")));
});

// Register the authorization handler
builder.Services.AddScoped<IAuthorizationHandler, DepartmentHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
