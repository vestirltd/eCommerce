using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    public class ecommerceController : ControllerBase
    {
        private readonly IDapperDBConnectInterface _IDapperDBConnect;
        private readonly IGetStockInterface _IgetStock;
        private readonly IBillGenerate _billGenerate;
        private readonly IViewSalesReport _viewSalesReport;
        private readonly IExportSalesReport _exportSalesReport;
        // private readonly IUpdateStockInterface _updateStock;
        private readonly IUpdateStockInterface _updateStk;
        private readonly ISendMail _mail;
        public ecommerceController(IDapperDBConnectInterface IDapperDBConnect,IGetStockInterface getStockInterface,IBillGenerate billGenerate, IViewSalesReport viewSalesReport,IExportSalesReport exportSalesReport, IUpdateStockInterface stockInterface, ISendMail mail)
        {
            _IDapperDBConnect = IDapperDBConnect;
            _IgetStock = getStockInterface;
            _billGenerate = billGenerate;
            _viewSalesReport=viewSalesReport;
            _exportSalesReport=exportSalesReport;
            _updateStk = stockInterface;
            _mail = mail;
        }

        [HttpGet()]
        [Route("Admin/GetStock")]
        public List<Stock> GetStocks()
        {
            List<Stock> stock = _IgetStock.GetStock();
            return stock;
        }
        [HttpPost()]
        [Route("SalesMan/GenerateBill")]
        public string GenerateBill([FromBody] List<GenerateBill> generateBill, [FromHeader] string name,[FromHeader] string enterYourEmail)
        {
            string s = _billGenerate.generateBill(generateBill, name, enterYourEmail);
            return s;
        }
        [HttpGet()]
        [Route("Admin/SalesReport")]
        public List<Bills> ViewReport()
        {
            List<Bills> bills = _viewSalesReport.ViewsalesReport();
            return bills;
        }
        [HttpPost()]
        [Route("Admin/Export")]
        public string exportReport()
        {
            _exportSalesReport.exportSalesReport();
            return "File Exported";
        }
        [HttpPost()]
        [Route("Admin/UpdateStock")]
        public string updateStock([FromHeader] string ProductName, [FromHeader] int Quantity)
        {
            string updatestk = _updateStk.UpdateStockFunction(ProductName,Quantity);
            return updatestk;
        }

        [HttpPost()]
        [Route("Admin/SendMail")]
        public string sendingMail()
        {
            _mail.sendingMail("", "", "");
            return "hi";
        }
    }
}