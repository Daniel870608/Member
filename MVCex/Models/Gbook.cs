using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCex.Models
{
    public class Gbook
    {
        [DisplayName("編號")]
        public int Id { get; set; }

        [DisplayName("姓名")]
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(30, ErrorMessage = "長度不可超過30字元")]
        public string Name { get; set; }

        [DisplayName("內容")]
        [Required(ErrorMessage = "請輸入內容")]
        [StringLength(50, ErrorMessage = "長度不可超過50字元")]
        public string Content { get; set; }

        [DisplayName("新增時間")]
        public DateTime CreateTime { get; set; }

        [DisplayName("回覆內容")]
        [StringLength(50, ErrorMessage = "長度不可超過50字元")]
        public string Reply { get; set; }


        [DisplayName("回覆時間")]
        public DateTime ReplyTime { get; set; }
    }
}