using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.Interfaces
{
    public interface ISendMail
    {
        public string sendingMail(string toMail, string subject, string body);
    }
}