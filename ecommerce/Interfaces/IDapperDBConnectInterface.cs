using System.Data;
namespace ecommerce.Interfaces
{
    public interface IDapperDBConnectInterface
    {
        public IDbConnection CreateConnection();
        public string PostDBQuery(string query);
    }
}