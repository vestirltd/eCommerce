using System.Net;
using System.Net.Mail;
using ecommerce.Interfaces;

namespace ecommerce.Services
{
    public class SendMail:ISendMail
    {
        private readonly string _email;
        private readonly string _password;
        public SendMail(IConfiguration config)
        {
            _email = config.GetConnectionString("AdminMail");
            _password = config.GetConnectionString("AdminPass");

        }
        public string sendingMail(string toMail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_email,_password),
                EnableSsl = true,
            };
            smtpClient.Send(_email , toMail, subject, body);
            //smtpClient.Send(_email , toMail, subject, body);
            return "MailSent";
        }
        
    }
}