using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.InventoryTickets.Dto
{
    public class GetInventoryTicketInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchTerm { get; set; }
        public string ResquestDate { get; set; }
        public int CreatorById { get; set; }
        public int WarehouseId { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
