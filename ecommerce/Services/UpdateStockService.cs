using ecommerce.Interfaces;
using ecommerce.Models;

namespace ecommerce.Services
{
    public class UpdateStockService:IUpdateStockInterface
    {
        private readonly IDapperDBConnectInterface _dapperDSConnectioninterface;
        private readonly IGetStockInterface _getStock;
        public UpdateStockService(IDapperDBConnectInterface dapperDBConnectInterface, IGetStockInterface stockInterface)
        {
            _dapperDSConnectioninterface = dapperDBConnectInterface;
            _getStock = stockInterface;
        }
        public string UpdateStockFunction(string ProductName, int quantity)
        {
            List<Stock> stock = _getStock.GetStock();
            foreach (var stockCheck in stock)
            {
                if(stockCheck.product == ProductName)
                {
                    int qty = stockCheck.stock + quantity;
                    string query = "UPDATE StockMaintain SET [Stock] = "+qty+" WHERE [Product] = '"+ProductName+"'";
                    Console.WriteLine(query);
                    _dapperDSConnectioninterface.PostDBQuery(query);
                    return "Stock Updated";
                }
            }
            
            // System.Console.WriteLine(stockUpdateVariable.product);
            System.Console.WriteLine("Inside Update Function");
            return ProductName+" is not available in our store. Please add as new Product";
        }
    }
}