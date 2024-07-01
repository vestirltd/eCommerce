using Dapper;
using ecommerce.Interfaces;
using ecommerce.Models;

namespace ecommerce.Services
{
    public class ViewSalesReport:IViewSalesReport
    {
        private readonly IDapperDBConnectInterface _Database;
        public ViewSalesReport(IDapperDBConnectInterface DapperDbConnect)
        {
            this._Database = DapperDbConnect;
        }
        public List<Bills> ViewsalesReport()
        {
            System.Console.WriteLine("Hi i am inside");
            string query = "select * from BillHistory";
            using(var connection = this._Database.CreateConnection())
            {
                List<Bills> results = connection.Query<Bills>(query).ToList();
                return results;
            }
        }
    }
}