namespace Compras.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Sprites Sprites { get; set; }
    }
    public class Sprites
    {
        public string Front_default { get; set; }
    }
}
