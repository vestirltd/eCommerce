using ecommerce.Interfaces;
using ecommerce.Models;

namespace ecommerce.Services
{
    public class BillGenerateService:IBillGenerate
    {
        private readonly IDapperDBConnectInterface _Database;
        private readonly IGetStockInterface _stock;
        public BillGenerateService(IDapperDBConnectInterface DapperDbConnect, IGetStockInterface stock)
        {
            _Database = DapperDbConnect;
            _stock = stock;
        }
        public string generateBill(List<GenerateBill> generateBills, string name)
        {
            Bills bill = new Bills();
            List<updateStock> QtyUpdate = new List<updateStock>();
            for(int i=0;i<generateBills.Count; ++i)
            {
                Stock stock = _stock.GetParticularStock(generateBills[i].product);
                if(stock.stock < generateBills[i].quantity)
                {
                    return generateBills[i].product+" - Out Of Stock";  // Todo add mail notification
                }
                else
                {
                    updateStock UpdateStk = new updateStock{product = stock.product, quantity = (stock.stock - generateBills[i].quantity)};
                    QtyUpdate.Add(UpdateStk);
                    bill.product = bill.product+stock.product+";";
                    bill.quantity = bill.quantity + generateBills[i].quantity +";";
                    bill.price = bill.price+stock.price.ToString()+";";
                    bill.NetTotal = bill.NetTotal+(generateBills[i].quantity * stock.price); 
                }
            }
            foreach(var a in QtyUpdate)
            {
                string stockUpdateQuery = "UPDATE StockMaintain SET [Stock] = "+a.quantity+" where [Product] = '"+a.product+"'";
                string logmessage = _Database.PostDBQuery(stockUpdateQuery);
            }
            bill.InvoiceDate = DateTime.Now.ToString("dd-MMM-yyyy");
            bill.CustomerName = name;
            bill.VAT = bill.NetTotal * 0.2;
            bill.GrossTotal = bill.NetTotal+bill.VAT;
            string BillGenerateQuery = "INSERT INTO [sampleEcommerce].[dbo].[BillHistory] ([InvoiceDate],[CustomerName],[Quantity],[Product],[Price],[NetTotal],[VAT],[GrossTotal]) VALUES ('"+bill.InvoiceDate+"', '"+bill.CustomerName+"', '"+bill.quantity+"', '"+bill.product+"', '"+bill.price+"', "+bill.NetTotal+" , "+bill.VAT+", "+bill.GrossTotal+")";
            string response = _Database.PostDBQuery(BillGenerateQuery);
            return "Bill Generated";
        }
    }
}