using System.Runtime.CompilerServices;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

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
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        private readonly IGetInvoiceNumber _getInvoiceNo;
        private const string InvoiceKey = "invoiceNumber";

        public ecommerceController(IDapperDBConnectInterface IDapperDBConnect,IGetStockInterface getStockInterface,IBillGenerate billGenerate, IViewSalesReport viewSalesReport,IExportSalesReport exportSalesReport, IUpdateStockInterface stockInterface,IConnectionMultiplexer redis, IGetInvoiceNumber invoiNo)
        {
            _IDapperDBConnect = IDapperDBConnect;
            _IgetStock = getStockInterface;
            _billGenerate = billGenerate;
            _viewSalesReport=viewSalesReport;
            _exportSalesReport=exportSalesReport;
            _updateStk = stockInterface;
            _redis = redis;
            _db = _redis.GetDatabase();
            _getInvoiceNo = invoiNo;
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
        public async Task<string> GenerateBill([FromBody] List<GenerateBill> generateBill, [FromHeader] string name,[FromHeader] string enterYourEmail)
        {
            string s =await _billGenerate.generateBill(generateBill, name, enterYourEmail);
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
        [HttpPost("Admin/SetInitialInvoiceNumber")]
        public async Task<IActionResult> SetInitialInvoiceNumber(string initialValue)
        {
            bool isSet = await _db.StringSetAsync(InvoiceKey, initialValue, when: When.NotExists);
            if (isSet)
            {
                return Ok(new { Message = "Initial value set successfully", InitialValue = initialValue });
            }
            else
            {
                return BadRequest(new { Message = "Initial value already exists" });
            }
        }
        // To understand how Redis works
        // [HttpGet("Admin/CheckInvoiceNumber")]
        // public async Task<string> GetNextInvoiceNumbers()
        // {
        //     // Increment the invoice number in Redis
        //     // long nextInvoiceNumber = await _db.StringIncrementAsync(InvoiceKey);
        //     string INo = await _getInvoiceNo.GetNextInvoiceNumber();
        //     System.Console.WriteLine(INo);
        //     return INo;
        // }
    }
}