using Dapper;
using ecommerce.Interfaces;
using ecommerce.Models;

namespace ecommerce.Services
{
    public class GetStockServices:IGetStockInterface
    {
        private readonly IDapperDBConnectInterface _Database;
        public GetStockServices(IDapperDBConnectInterface DapperDbConnect)
        {
            this._Database = DapperDbConnect;
        }
        public List<Stock> GetStock()
        {
            string query = "select * from StockMaintain";
            using(var connection = this._Database.CreateConnection())
            {
                // connection.Open();
                List<Stock> results = connection.Query<Stock>(query).ToList();
                return results;
            }
        }
        public Stock GetParticularStock(string productName)
        {
            string query = "select * from StockMaintain where Product = '"+ productName +"'; ";
            Stock ProductStock = new Stock();
            using(var connection = this._Database.CreateConnection())
            {
                connection.Open();
                var stockMaintain =  connection.Query<Stock>(query);
                
                foreach (var stock in stockMaintain)
                {
                    ProductStock.product = stock.product;
                    ProductStock.stock = stock.stock;
                    ProductStock.price = stock.price;
                }
                return ProductStock;
            }
        }
    }
}