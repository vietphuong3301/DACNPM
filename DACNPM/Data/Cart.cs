using DACNPM.Models;
#nullable disable
namespace DACNPM.Data
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Amount { get; set; }
        public double totalPrice => Amount * Product.Price;
    }
}
