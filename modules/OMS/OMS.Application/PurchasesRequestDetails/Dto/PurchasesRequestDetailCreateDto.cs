using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails.Dto
{
	[AutoMapTo(typeof(PurchasesRequestDetail))]
	public class PurchasesRequestDetailCreateDto
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
	}
}
