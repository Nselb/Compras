using Compras.Models;

namespace Compras.HttpCLiente
{
    public class PokeClient : IPokeApiService
    {
        private readonly HttpClient _httpClient;

        public PokeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Pokemon>> GetPokemonsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<PokemonApiResponse>("https://pokeapi.co/api/v2/pokemon?limit=20&offset=0");
            if (response == null)
            {
                return new List<Pokemon>();
            }
            foreach (var pokemon in response.Results)
            {
                var res = await _httpClient.GetFromJsonAsync<PokemonSpriteResponse>(pokemon.Url);
                if (res == null){
                    pokemon.Sprites = new Sprites() { Front_default = "" };
                    continue;
                }
                pokemon.Id = res.id;
                pokemon.Sprites = res.Sprites;
            }
            return response.Results;
        }
    }
    public interface IPokeApiService
    {
        Task<List<Pokemon>> GetPokemonsAsync();
    }
    public class PokemonApiResponse
    {
        public List<Pokemon> Results { get; set; }
    }
    public class PokemonSpriteResponse
    {
        public int id {  get; set; }
        public Sprites Sprites { get; set; }
    }
}
