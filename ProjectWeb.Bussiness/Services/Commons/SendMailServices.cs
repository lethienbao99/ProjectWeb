using Microsoft.Extensions.Configuration;
using ProjectWeb.Common.IServices;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static ProjectWeb.Common.Enums.EnumConstants;

namespace ProjectWeb.Bussiness.Services.Commons
{
    public class SendMailServices : ISendMailServices
    {
        private readonly IConfiguration _config;
        public SendMailServices(IConfiguration config)
        {
            _config = config;
        }
     
        public async Task<ResultMessage<bool>> SendMailGoogleSmtp(string _to, string _subject, string _body)
        {
           
            MailMessage message = new MailMessage(
                from: _config[SystemsConstants.MailSettings_Mail],
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_config[SystemsConstants.MailSettings_Mail]));
            message.Sender = new MailAddress(_config[SystemsConstants.MailSettings_Mail]);

            // Tạo SmtpClient kết nối đến smtp.gmail.com
            using (SmtpClient client = new SmtpClient(_config[SystemsConstants.MailSettings_SmtpClient]))
            {
                client.TargetName = "STARTTLS/smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_config[SystemsConstants.MailSettings_Mail], _config[SystemsConstants.MailSettings_Password]);
                client.EnableSsl = true;
                var result =  await SendMail(_config[SystemsConstants.MailSettings_Mail], _to, _subject, _body, client);
                return result;
            }

        }


        public async Task<bool> SendMailLocalSmtp(string _from, string _to, string _subject, string _body)
        {
            using (SmtpClient client = new SmtpClient("localhost"))
            {
                var result =  await SendMail(_from, _to, _subject, _body, client);
                return result.Object;
            }
        }

        public async Task<ResultMessage<bool>> SendMail(string _from, string _to, string _subject, string _body, SmtpClient client)
        {
            // Tạo nội dung Email
            MailMessage message = new MailMessage(
                from: _from,
                to: _to,
                subject: _subject,
                body: _body
            );
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(_from));
            message.Sender = new MailAddress(_from);


            try
            {
                await client.SendMailAsync(message);
                return new ResultObjectSuccess<bool>(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ResultObjectError<bool>(ex.Message);
            }
        }

    }
}

