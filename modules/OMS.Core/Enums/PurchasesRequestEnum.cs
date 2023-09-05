using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
	public class PurchasesRequestEnum
	{
		public enum Status : byte
		{
			[Display(Name = "Đã tổng hợp")]
			Approve = 0,

			[Display(Name = "Chưa tổng hợp")]
			Draft = 2,
			
		}


        public enum YCNK : byte
        {
            [Display(Name = "Chưa xử lý")]
            Draft = 0,
            [Display(Name = "Đã gửi")]
            Original = 1,
            [Display(Name = "Đã duyệt")]
            Approve = 2,
            [Display(Name = "Từ chối")]
            Reject = 3,
        }


        public enum OrderStatus : byte
        {
            [Display(Name = "Đã xác nhận")]
            Approve = 0,

            [Display(Name = "Chờ xác nhận")]
            Draft = 1,

            [Display(Name = "Hoàn thành")]
            Done = 2,

            [Display(Name = "Sắp đến hạn")]
            OutofDate = 3,

            [Display(Name = "Qúa hạn")]
            OutDate = 5,
        }

        public enum MyworkStatus : byte
        {
            [Display(Name = "Đã hoàn thành")]
            Done = 0,

            [Display(Name = "Chưa xử lý")]
            Draf = 2,

            [Display(Name = "Sắp hết hạn")]
            InProcess = 1,

            [Display(Name = "Quá hạn")]
            OutOfDate = 3,
        }

        public enum GetPriceSupkStatus : byte
        {
            [Display(Name = "Đã hoàn thành")]
            Done = 0,

            [Display(Name = "Chưa xử lý")]
            Draf = 2,

            [Display(Name = "Sắp hết hạn")]
            InProcess = 1,

            [Display(Name = "Quá hạn")]
            OutOfDate = 3,
        }

        public enum OwnerStatusEnum
        {
            [Display(Name = "Theo dõi")]
            Inform = 0,

            [Display(Name = "Chủ trì")]
            Host = 1,

            [Display(Name = "Giao việc")]
            Assign = 2
        }

    }
}
