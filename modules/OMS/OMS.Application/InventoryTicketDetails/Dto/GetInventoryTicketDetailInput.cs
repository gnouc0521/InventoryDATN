using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.InventoryTicketDetails.Dto
{
    public class GetInventoryTicketDetailInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long WareHouseId { get; set; }
        public string SearchTerm { get; set; }
        public long InventoryTicketsId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
