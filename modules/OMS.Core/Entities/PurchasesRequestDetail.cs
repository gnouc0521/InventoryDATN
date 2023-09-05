using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
	[Table("AppPurchasesRequestDetail", Schema = netcoreConsts.SchemaName)]
	public class PurchasesRequestDetail : FullAuditedEntity<long>
	{
		public long PurchasesRequestId { get; set; }
		public virtual PurchasesRequest purchasesRequest { get; set; }

		public long ItemId { get; set; }
		public int Quantity { get; set; }
		public int UnitId { get; set; }
		public int SupplierId { get; set; }
		public DateTime TimeNeeded { get; set; }
        public string Uses { get; set; }
        public string	Note { get; set; }
	}
}
