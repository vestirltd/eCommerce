using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Interfaces;
using StackExchange.Redis;
namespace ecommerce.Services
{
    public class GetInvoiceNumber : IGetInvoiceNumber
    {
        // private readonly IConnectionMultiplexer _redis;
        private const string InvoiceKey = "invoiceNumber";
        private readonly IDatabase _db;
        public GetInvoiceNumber(IConnectionMultiplexer reddis)
        {
            _db = reddis.GetDatabase();
            
        }
        public async Task<string> GetNextInvoiceNumber()
        {
            
            long nextInvoiceNumber = await _db.StringIncrementAsync(InvoiceKey);
            string InvoiceNumber = "VSL_" + nextInvoiceNumber.ToString();
            return InvoiceNumber;
        }
    }
}