using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IGetStockInterface
    {
        public List<Stock> GetStock();
        public Stock GetParticularStock(string productName);
    }
}