using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IBillGenerate
    {
        public Task<string> generateBill(List<GenerateBill> generateBills,string name, string customerMailId);
    }
}