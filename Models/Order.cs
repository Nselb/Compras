namespace Compras.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderDetails> orderDetails { get; set; }
    }

    public class OrderDetails
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
    }
}
