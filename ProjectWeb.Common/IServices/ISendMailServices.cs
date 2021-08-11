using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface ISendMailServices
    {
        Task<bool> SendMailGoogleSmtp(string _to, string _subject, string _body);
        Task<bool> SendMailLocalSmtp(string _from, string _to, string _subject, string _body);
        Task<bool> SendMail(string _from, string _to, string _subject, string _body, SmtpClient client);
    }
}
