using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Inventorys.Dto
{
    public class InventorySearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public long WareHouseId { get; set; }
        public long ItemId { get; set; }
        public string SearchTerm { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }

    }
}