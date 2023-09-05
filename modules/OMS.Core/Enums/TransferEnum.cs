using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class TransferEnum
    {
        public enum TransferStatus : byte
        {
            [Display(Name = "Chờ xử lý")]
            Original = 0,
            [Display(Name = "Từ chối")]
            Reject = 1,
            [Display(Name = "Đã gửi")]
            Waitinng = 2,
            [Display(Name = "Phê duyệt")]
            Approve = 3,
        }
    }
}
