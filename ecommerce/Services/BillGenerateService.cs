using ecommerce.Interfaces;
using ecommerce.Models;

namespace ecommerce.Services
{
    public class BillGenerateService:IBillGenerate
    {
        private readonly IDapperDBConnectInterface _Database;
        private readonly IGetStockInterface _stock;
        private readonly string _adminMailNotificationMail;
        private readonly ISendMail _mail;
        private readonly IGetInvoiceNumber _invoiceNo;
        public BillGenerateService(IDapperDBConnectInterface DapperDbConnect, IGetStockInterface stock, IConfiguration config, ISendMail mail, IGetInvoiceNumber invoiceNo)
        {
            _Database = DapperDbConnect;
            _stock = stock;
            _adminMailNotificationMail = config.GetConnectionString("AdminMailId");
            _mail = mail;
            _invoiceNo = invoiceNo;
        }
        public async Task<string> generateBill(List<GenerateBill> generateBills, string name, string customerMailId)
        {
            Bills bill = new Bills();
            List<updateStock> QtyUpdate = new List<updateStock>();
            for(int i=0;i<generateBills.Count; ++i)
            {
                Stock stock = _stock.GetParticularStock(generateBills[i].product);
                if(stock.stock < generateBills[i].quantity)
                {
                    string outOfStockMessage = generateBills[i].product + " - Out Of Stock";
                    _mail.sendingMail(_adminMailNotificationMail,"Out Of Stock Alert !!",outOfStockMessage);
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
            bill.InvoiceNumber = await _invoiceNo.GetNextInvoiceNumber();
            bill.InvoiceDate = DateTime.Now.ToString("dd-MMM-yyyy");
            bill.CustomerName = name;
            bill.VAT = bill.NetTotal * 0.2;
            bill.GrossTotal = bill.NetTotal+bill.VAT;
            string BillGenerateQuery = "INSERT INTO [sampleEcommerce].[dbo].[BillsHistory] ([InvoiceNumber],[InvoiceDate],[CustomerName],[Quantity],[Product],[Price],[NetTotal],[VAT],[GrossTotal]) VALUES ('"+bill.InvoiceNumber+"', '"+bill.InvoiceDate+"', '"+bill.CustomerName+"', '"+bill.quantity+"', '"+bill.product+"', '"+bill.price+"', "+bill.NetTotal+" , "+bill.VAT+", "+bill.GrossTotal+")";
            System.Console.WriteLine(BillGenerateQuery+"==========");
            string response = _Database.PostDBQuery(BillGenerateQuery);
            _mail.sendingMail(customerMailId, "VSL Bill", "Hi "+bill.CustomerName+",\nPlease find the bill details\n\nInvoice No : "+bill.InvoiceNumber
                                                        +"\n Invoice Date: "+bill.InvoiceDate
                                                        +"\nProducts: "+bill.product
                                                        +"\n Quantity : "+bill.quantity
                                                        +"\nPrice: "+bill.price
                                                        +"\nNetTotal : "+bill.NetTotal
                                                        +"\nVAT : "+bill.VAT
                                                        +"\nGross Total : "
                                                        +bill.GrossTotal
                                                        +" \n\n Thanks, \n VSLGroups");
            return "Bill Generated";
        }
    }
}