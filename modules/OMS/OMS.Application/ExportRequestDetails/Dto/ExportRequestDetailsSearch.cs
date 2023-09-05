using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ExportRequestDetails.Dto
{
    public class ExportRequestDetailsSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchTerm { get; set; }
        public long ExportRequestId { get; set; }
        public long WarehouseId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }

    }
}
