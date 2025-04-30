using FluentValidation;
using Intranet_NEW.DAL;
using Intranet_NEW.Services.Handlers;
using Intranet_NEW.Services.Validadores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSignalR();
builder.WebHost.UseIISIntegration();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); ;
// Configura sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(45);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
builder.Services.AddScoped<IUploadFileService, UploadFileService>();
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
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("Views/Administrativo/{0}.cshtml");
    options.ViewLocationFormats.Add("Views/Acompanhamento/{0}.cshtml");
    options.ViewLocationFormats.Add("Views/Home/{0}.cshtml");
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configurar o pipeline de requisições HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // O valor padrão de HSTS é 30 dias. Você pode querer mudar isso para cenários de produção, veja https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapHub<ObjectHubService>("/objecthub");
app.MapHub<ReactionHubService>("/reactionhub");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider("C:\\uploads"),
    RequestPath = "/uploads"
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Feed}/{id?}");

app.Run();