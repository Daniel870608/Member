using MVCex.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVCex.Services
{
    public class MemberService
    {
        private readonly string cnstr = ConfigurationManager.ConnectionStrings["Guestbook"].ConnectionString;

        //註冊會員
        public void RegisterNewMember(Members MemberData)
        {
            //密碼加密
            MemberData.Password = HashPassword(MemberData.Password);
            string sql = @"INSERT INTO Members VALUES('" + MemberData.Account + "','" + MemberData.Password + "','" + MemberData.Name + "','" + MemberData.Email + "','" + MemberData.AuthCode
                + "','0')";
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql,conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        //帳號驗證
        public bool AccountCheck(string Account)
        {
            string sql = @"SELECT * FROM Members WHERE Account = '" + Account + "'";
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    int RowsCount = cmd.ExecuteNonQuery();
                    if (RowsCount > 0)
                    {
                        conn.Close();
                        return false;
                    }
                    else
                    {
                        conn.Close();
                        return true;
                    }
                }
            }
        }

        //密碼加密
        public string HashPassword(string Password)
        {
            string HashPassword = string.Empty;
            string saltKey = "sknvio8jdoj0xjsodncoskzpwkalidj";
            string saltAndPassword = string.Concat(saltKey, Password);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
            byte[] HashData = sha1.ComputeHash(PasswordData);
            string HashResult = string.Empty;
            for (int i = 0; i < HashData.Length; i++)
            {
                HashResult += HashData[i].ToString("x2");
            }
            return HashResult;
        }
    }
}