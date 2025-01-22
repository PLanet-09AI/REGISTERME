using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using REGISTERME.Data;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with the correct connection string name
builder.Services.AddDbContext<REGISTERMEContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("REGISTERMEContextConnection"))); // Use "REGISTERMEContextConnection"
// Add Identity Services
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // Set to false if you don’t want email confirmation
})
.AddRoles<IdentityRole>() // Add this if you need role support
.AddEntityFrameworkStores<REGISTERMEContext>();

// Configure cookie settings (optional)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// Add MVC and Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enable authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();