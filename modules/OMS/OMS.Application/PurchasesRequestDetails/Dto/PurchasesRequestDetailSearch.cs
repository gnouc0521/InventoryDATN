using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails.Dto
{
	public class PurchasesRequestDetailSearch : PagedAndSortedInputDto, IShouldNormalize
	{
		public string SearchTerm { get; set; }
		public long PurchasesRequestId { get; set; }
		public long SubsidiaryId { get; set; }
		public void Normalize()
		{
			if (string.IsNullOrEmpty(Sorting))
			{
				Sorting = "Id";
			}
		}
	}
}
