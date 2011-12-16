using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using System.Net;
using System.Net.Mail;
using System.IO;


namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class MailBL : BaseBL
    {
        public const string SMTP_HOST_GMAIL = "smtp.gmail.com";
        public const string SMTP_HOST_YAHOO = "smtp.mail.yahoo.com";

        public const bool SSL_GMAIL = true;
        public const bool SSL_YAHOO = false;

        public const int PORT_GMAIL = 587;
        public const int PORT_YAHOO = 25;


        public static string SendByGmail(string from, string to, string subject, string body, string id, string pass)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (!regex.IsMatch(from))
            {
                return "Địa chỉ gửi không hợp lệ";
            }

            to += ";";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from);
            int iCharacter = -1;
            string _to = string.Empty;
            do
            {
                iCharacter = to.IndexOf(";");
                if (iCharacter == -1)
                    break;
                _to = to.Substring(0, iCharacter);
                to = to.Remove(0, iCharacter + 1);
                if (regex.IsMatch(_to))
                    mailMessage.To.Add(_to);
                else
                {
                    return "Địa nhận email không hợp lệ";
                }

            }
            while (iCharacter != -1);

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.Priority = MailPriority.Normal;

            // SMTP Configure 
            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(id, pass); // enter your user name and password for gmail
            client.Port = PORT_GMAIL;
            client.Host = SMTP_HOST_GMAIL;
            client.EnableSsl = SSL_GMAIL;

            MailMessage message = mailMessage;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                return "Khong the send mail";
            }
            return string.Empty;
        }

        public static string SendByYahoo(string from, string to, string subject, string body, string id, string pass)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (!regex.IsMatch(from))
            {
                return "Địa chỉ gửi không hợp lệ";
            }

            to += ";";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from);
            int iCharacter = -1;
            string _to = string.Empty;
            do
            {
                iCharacter = to.IndexOf(";");
                if (iCharacter == -1)
                    break;
                _to = to.Substring(0, iCharacter);
                to = to.Remove(0, iCharacter + 1);
                if (regex.IsMatch(_to))
                    mailMessage.To.Add(_to);
                else
                {
                    return "Địa nhận email không hợp lệ";
                }

            }
            while (iCharacter != -1);

            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.Priority = MailPriority.Normal;

            // SMTP Configure 
            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(id, pass); // enter your user name and password for gmail
            client.Port = PORT_YAHOO;
            client.Host = SMTP_HOST_YAHOO;
            client.EnableSsl = SSL_YAHOO;

            MailMessage message = mailMessage;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                return "Khong the send mail";
            }
            return string.Empty;
        }
    }
}
