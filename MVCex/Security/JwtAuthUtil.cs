using Jose;
using MVCex.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCex.Security
{
    public class JwtAuthUtil
    {
        public string GenerateToken(string Account,string Name)
        {
            string secret = "Guestbook";
            var payload = new JwtObject
            {
                Account = Account,
                Name = Name,
                Exp = DateTime.Now.AddSeconds(Convert.ToInt32("1200")).ToString()
            };
            var token = JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS512);
            return token;
        }
    }
}