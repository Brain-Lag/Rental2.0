using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Rental.Services;
using Rental;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<PropertyService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authorization/Login";
        options.LogoutPath = "/Authorization/Login";

    });
builder.Services.AddAuthorization();
string? connection = builder.Configuration.GetConnectionString("MyConnectionString");
builder.Services.AddDbContext<AppContextdb>(options => options.UseSqlServer(connection));
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{Controller=Home}/{Action=Index}"
    );

app.Run();