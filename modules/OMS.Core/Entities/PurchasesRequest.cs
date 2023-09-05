using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
	[Table("AppPurchasesRequest", Schema = netcoreConsts.SchemaName)]
	public class PurchasesRequest : FullAuditedEntity<long>
	{
		public long SubsidiaryCompanyId  { get; set; }
		public PurchasesRequestEnum.MyworkStatus RequestStatus { get; set; }

		public long PurchasesSynthesiseId { get; set; }

        public DateTime RequestDate { get; set; }
	}
}
