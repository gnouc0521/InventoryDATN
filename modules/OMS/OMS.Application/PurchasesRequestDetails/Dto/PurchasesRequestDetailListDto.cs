using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails.Dto
{
	[AutoMap(typeof(PurchasesRequestDetail))]
	public class PurchasesRequestDetailListDto : Entity<long>
	{
		public long PurchasesRequestId { get; set; }
		public virtual PurchasesRequest purchasesRequest { get; set; }

		public long ItemId { get; set; }
		public int Quantity { get; set; }
		public int UnitId { get; set; }
		public int SupplierId { get; set; }
		public DateTime TimeNeeded { get; set; }
        public string Uses { get; set; }
        public string Note { get; set; }

        public string NameItem { get; set; }
        public string NameNCC { get; set; }
        public string NameUnit { get; set; }

    }
}
