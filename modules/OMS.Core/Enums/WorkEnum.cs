using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class WorkEnum
    {
        public enum Priority : byte
        {
            [Display(Name = "Thấp")]
            Low = 0,
            [Display(Name = "Trung bình")]
            Medium = 1,

            [Display(Name = "Cao")]
            High = 2,
        }

      
        public enum Status : byte
        {
            [Display(Name = "Chờ xử lý")]
            Draft = 0,

            [Display(Name = "Đang xử lý")]
            InProcess = 1,

            [Display(Name = "Hoàn thành")]
            Done = 2,

            [Display(Name = "Quá hạn")]
            OutOfDate = 3,
        }

        public enum JobLabel : byte
        {
            [Display(Name = "Tạo mới")]
            Create = 0,

            [Display(Name = "Nghiên cứu")]
            Study = 1,

            [Display(Name = "Kiểm tra")]
            Review = 2
        }

        public enum OwnerStatusEnum
        {
            [Display(Name = "Theo dõi")]
            Inform = 0,

            [Display(Name = "Chủ trì")]
            Host = 1,

            [Display(Name = "Phối hợp")]
            CoWork = 2,

            [Display(Name = "Giao việc")]
            Assign = 4
        }

    }
}
