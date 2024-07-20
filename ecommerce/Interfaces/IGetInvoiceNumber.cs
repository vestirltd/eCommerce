using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.Interfaces
{
    public interface IGetInvoiceNumber
    {
        public Task<string> GetNextInvoiceNumber();
    }
}