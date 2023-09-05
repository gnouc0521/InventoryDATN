using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto
{
	[AutoMapTo(typeof(PurchasesRequest))]
	public class PurchasesRequestCreateDto
	{
        public long   SubsidiaryCompanyId { get; set; }
        public string SubsidiaryCompany { get; set; }

		public string Address { get; set; }

		public string PhoneNumber { get; set; }

		public string EmailAddress { get; set; }

		public DateTime RequestDate { get; set; }

		public PurchasesRequestEnum.MyworkStatus RequestStatus { get; set; }
	}
}
