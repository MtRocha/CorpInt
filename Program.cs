using Intranet_NEW.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Intranet_NEW.Services.Validadores;
using Intranet_NEW.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{ 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "LoginCookie";
    options.LoginPath = "/Login/Login";
    options.LogoutPath = "/Home/Logout";
    options.AccessDeniedPath = "/Login/AcessoNegado";
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(45);
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;

    options.Events.OnRedirectToLogin = context => {
        context.Response.Redirect("/Login/AcessoNegado");
        return Task.CompletedTask;
    };

});


builder.Services.AddAuthorization();

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
    pattern: "{controller=Home}/{action=Feed}/{id?}");

app.Run();
