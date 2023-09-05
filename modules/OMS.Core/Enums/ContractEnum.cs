using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class ContractEnum
    {
        public enum ContractStatus : byte
        {
            [Display(Name = "Bản nháp")]
            Draft = 0,
            [Display(Name = "Chờ xử lý")]
            Original = 1,
            [Display(Name = "Chờ phê duyệt")]
            Waiting = 2,
            [Display(Name = "Từ chối")]
            Reject = 3,
            [Display(Name = "Trưởng phòng Từ chối")]
            RejectManager = 4,
            [Display(Name = "Phê duyệt")]
            Approve = 5,
            [Display(Name = "Đã kí hợp đồng")]
            Contract = 6,
        }
        public enum ExportStatus : byte
        {
            [Display(Name = "Chưa tạo phiếu xuất")]
            NotExport = 0,
            [Display(Name = "Tạo phiếu xuất")]
            Done = 1,
           
        }
    }
}
