using Compras.HttpCLiente;
using Compras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Compras.Pages
{
    public class CompraModel : PageModel
    {
        private readonly IPokeApiService _pokeApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;

        public CompraModel(IPokeApiService pokeApiService, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            _pokeApiService = pokeApiService;
            Pokemons = new List<Pokemon>();
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public List<Pokemon> Pokemons { get; set; }
        public List<Pokemon> Compras
        {
            get => HttpContext.Session.GetObject<List<Pokemon>>("Compras") ?? new List<Pokemon>();
            set => HttpContext.Session.SetObject("Compras", value);
        }

        public async Task OnGetAsync()
        {
            Pokemons = await _pokeApiService.GetPokemonsAsync();
        }

        public IActionResult OnPostComprar(int pokemonId, string pokemonName)
        {
            var carrito = Compras;
            carrito.Add(new Pokemon { Id = pokemonId, Name = pokemonName });
            Compras = carrito;
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostExportarCSV()
        {
            // Generar el contenido del archivo CSV
            var csv = new StringBuilder();
            csv.AppendLine("ID,Nombre");
            var order = new Order{orderDetails = new List<OrderDetails>() };
            foreach (var pokemon in Compras)
            {
                csv.AppendLine($"{pokemon.Id},{pokemon.Name}");
                try
                {
                    order.orderDetails.Add(new OrderDetails() { PokemonId = pokemon.Id });

                    _dbContext.Orders.Add(order);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            await _dbContext.SaveChangesAsync();
            // Guardar el archivo CSV en wwwroot (o en otro lugar según sea necesario)
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "compras.csv");
            System.IO.File.WriteAllText(filePath, csv.ToString());

            // Descargar el archivo CSV generado
            return PhysicalFile(filePath, "text/csv", "compras.csv");
        }

        
    }
    public static class SessionExtensions
    {
        public static T GetObject<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}

