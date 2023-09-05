using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class ImportResquestEnum
    {
        public enum ImportResquestStatus : byte
        {
            [Display(Name = "Đã sắp xếp")]
            Approve = 0,
            [Display(Name = "Chưa xử lý")]
            Original = 1,
            [Display(Name = "Chưa sắp xếp")]
            Draft = 2,
            [Display(Name = "Gửi")]
            Waitting = 3,
            [Display(Name = "Từ chối")]
            Reject = 4,
            [Display(Name = "Hàng trong kho")]
            Done = 5,
        }
    }
}
