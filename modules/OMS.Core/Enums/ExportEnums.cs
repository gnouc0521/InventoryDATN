using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
   public class ExportEnums 
    {

        public enum ExportStatus : byte
        {
            [Display(Name = "Chờ xử lý")]
            Draft = 0,
            [Display(Name = "Chờ phê duyệt")]
            Waiting = 1,
            [Display(Name = "Từ chối")]
            Reject = 2,
            [Display(Name = "Phê duyệt")]
            Approve = 3,
            
            [Display(Name = "Hoàn thành")]
            Complete = 4,

           
        }   
        public enum Export : byte
        {
            [Display(Name = "Chưa tạo phiếu xuất")]
            Draft = 0,
            [Display(Name = "Chưa xuất hàng")]
            Waiting = 1,
            [Display(Name = "Đã xuất hàng")]
            Approve = 2,
           
        }
        public enum ExportType : byte
        {
            [Display(Name = "Điều chuyển")]
            Transfer = 0,
            [Display(Name = "Xuất thường")]
            Normal = 1,
        }
    }
}
