using MVCex.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVCex.ViewModels
{
    public class Guestbook
    {
        [DisplayName("搜尋")]
        public string Search { get; set; }

        public ForPaging ForPaging { get; set; }

        public List<Gbook> DataList { get; set; }

    }
}