using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MVCex.Services
{
    public class MailSerice
    {
        private string gmail_account = "acer77890@gmail.com";
        private string gmail_password = "7a0c1e1r9";
        private string gmail_mail = "acer77890@gmail.com";

        //將使用者資料填入驗證信範本中
        public string GetRegisterMailBody(string TempString, string UserName, string ValidateUrl)
        {
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);
            return TempString;
        }

        //寄送驗證信
        public void SendRegisterMail(string MailBody, string ToEmail)
        {
            //建立寄信用Smtp物件，以Gmail為例
            SmtpClient SmtpServer = new SmtpClient("smtp.gamil.com");
            //設定使用的Port，設定Gmail使用的Port
            SmtpServer.Port = 587;
            //建立使用者憑據，設定Gmail帳戶
            SmtpServer.Credentials = new System.Net.NetworkCredential(gmail_account, gmail_password);
            //開啟SSL
            SmtpServer.EnableSsl = true;
            //宣告信件內容MailMessage物件
            MailMessage mail = new MailMessage();
            //設定來源信箱
            mail.From = new MailAddress(gmail_mail);
            //設定收信者信箱
            mail.To.Add(ToEmail);
            //設定信件主旨
            mail.Subject = "會員註冊確認信";
            //設定信件內容
            mail.Body = MailBody;
            //設定信件內容為 HTML 格式
            mail.IsBodyHtml = true;
            //送出信件
            SmtpServer.Send(mail);
        }

        //取得驗證碼
        public string GetAuthCode()
        {
            string ValidateCode = string.Empty;
            //驗證碼陣列
            string[] Code = { "A","B","C","D","E","F","G","H","I","J","K","L","M",
                               "N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                                "1","2","3","4","5","6","7","8","9","0"};
            //產生亂數
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }

            return ValidateCode;
        }
    }
}