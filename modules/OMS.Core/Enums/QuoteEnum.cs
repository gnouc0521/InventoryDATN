using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class QuoteEnum
    {
        public enum SyntheticQuote : byte
        {
            [Display(Name = "Bản nháp")]
            Draft = 0,
            [Display(Name = "Chờ xử lý")]
            Original = 1 ,
            [Display(Name = "Phê duyệt")]
            Approve = 2,
            [Display(Name = "Từ chối")]
            Reject = 3,
            [Display(Name = "Đã tạo hợp đồng")]
            Contact = 4,  
          
        }
    }
}
