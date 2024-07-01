using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IBillGenerate
    {
        public string generateBill(List<GenerateBill> generateBills,string name);
    }
}