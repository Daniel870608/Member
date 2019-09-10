using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCex.Models
{
    public class Members
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        [Remote("AccountCheck","Member",ErrorMessage ="此帳號已存在")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "長度需介於6~30字元")]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(40, ErrorMessage = "不可超過40字元")]
        public string Password { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(20, ErrorMessage = "不可超過20字元")]
        public string Name { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "請輸入電子信箱")]
        [StringLength(200, ErrorMessage = "不可超過200字元")]
        [EmailAddress(ErrorMessage ="這不是Email格式")]
        public string Email { get; set; }

        public string AuthCode { get; set; }

        public bool isAdmin { get; set; }
    }
}