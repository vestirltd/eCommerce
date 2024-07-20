using ecommerce.Interfaces;
using ecommerce.Models;

namespace ecommerce.Services
{
    public class FileExportSalesReport : IExportSalesReport
    {
        // private readonly IExportSalesReport _exportSalesReport;
        private readonly IViewSalesReport _viewSalesReport;
        public FileExportSalesReport(IViewSalesReport viewSalesReport)
        {
            // _exportSalesReport = exportSalesReport;
            _viewSalesReport = viewSalesReport;
        }
        public string exportSalesReport()
        {
            List<Bills> bills = _viewSalesReport.ViewsalesReport();
            string fileName=DateTime.Now.ToString("ddMMyyyy_hhmmss");
            string filePath = "/Users/kannan/Desktop/DemoFile/Demo"+fileName+".csv";
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("InvoiceNumber,InvoiceDate,CustomerName,product,Price,Quantity,NetTotal,VAT,GrossTotal");         
                foreach (var item in bills)
                {
                    writer.WriteLine($"{item.InvoiceNumber},{item.InvoiceDate},{item.CustomerName},{item.product},{item.price},{item.quantity},{item.NetTotal},{item.VAT},{item.GrossTotal}");
                }
            }
            return "File Exported Successfully";
        }
        
    }
}