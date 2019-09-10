using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCex.Security.Models
{
    public class JwtObject
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Exp { get; set; } //時效
    }
}