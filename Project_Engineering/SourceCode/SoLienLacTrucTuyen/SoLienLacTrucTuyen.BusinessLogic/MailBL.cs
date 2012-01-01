using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Data.OleDb;
using System.Net.Sockets;
using System.ServiceModel;
using System.Web;
using System.Data;


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
        public static bool CheckEmailExist(string strEmail)
        {          
            var myBinding = new BasicHttpBinding();
            myBinding.Security.Mode = BasicHttpSecurityMode.None;
            var myEndpointAddress = new EndpointAddress("http://wcf.vikasrana.com/Soap.asmx");
            ServiceEmail.SoapSoapClient obj = new ServiceEmail.SoapSoapClient(myBinding, myEndpointAddress);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (!regex.IsMatch(strEmail))
            {
                return false;
            }
            else
                return obj.EmailValidator(strEmail);
        }
        public static string SendGmailWithTemplate(string from, string to, string subject, List<DailySchedule> tabularDailySchedule, string id, string pass)
        {
            StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("template/schedule.html"));
            sr = File.OpenText(HttpContext.Current.Server.MapPath("template/schedule.html"));
            string content = sr.ReadToEnd();
            //content = content.Replace("[Sender]", TextBoxName.Text.Trim());
            //content = content.Replace("[Email]", TextBoxEmail.Text);
            //content = content.Replace("[Content]", TextBoxContent.Text);
            //content = content.Replace("[DateTime]", DateTime.Now.ToShortDateString());
            int _count,_countafter;
            for (int i = 0; i < tabularDailySchedule.Count; i++)
            {
                _count = tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet.Count;
                _countafter = tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet.Count;
                // Morning
                for (int j = 0; j < 5; j++)
                {
                    // Morning
                    string strCell = "[T" + (tabularDailySchedule[i].DayInWeekId + 1).ToString() + (j+1).ToString() + "]";
                    if (j < tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet.Count)
                    {                        
                        content = content.Replace(strCell, tabularDailySchedule[i].SessionedSchedules[0].ListThoiKhoaBieuTheoTiet[j].SubjectName);
                    }
                    else
                    {
                        content = content.Replace(strCell, "(Nghỉ)");
                    }
                    // Afternoon
                    strCell = "[T" + (tabularDailySchedule[i].DayInWeekId + 1).ToString() + (j+5+1).ToString() + "]";
                    if (j < tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet.Count)
                    {
                        content = content.Replace(strCell, tabularDailySchedule[i].SessionedSchedules[1].ListThoiKhoaBieuTheoTiet[j].SubjectName);
                    }
                    else
                    {
                        content = content.Replace(strCell, "(Nghỉ)");
                    }                    
                }
            }
            try
            {
                SendByGmail(from, to, subject, content, id, pass);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

    }
}
