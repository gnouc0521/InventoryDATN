using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ExportRequests.Dto
{
    public class ExportRequestsSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchTerm { get; set; }

        public string RequestDate { get; set; }
        public ExportEnums.ExportStatus? Status { get; set; }
        public ExportEnums.Export? ExportStatus { get; set; }

        public int? WarehouseDestinationId { get; set; }

        public long? UserIdCreate { get; set; }
        public long ItemsId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }

    }

}
