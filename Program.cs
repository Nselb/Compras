using Compras;
using Compras.HttpCLiente;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Compra", ""); // Ruta por defecto para Compra.cshtml
}); ;
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient<IPokeApiService, PokeClient>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer("Data Source=UPOAULA10717;Initial Catalog=OrdenesCompra;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"));
var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
