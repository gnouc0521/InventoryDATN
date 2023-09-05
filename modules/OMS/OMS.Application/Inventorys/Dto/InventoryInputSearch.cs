using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Inventorys.Dto
{
    public class InventoryInputSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Category { get; set; }
        public string Group { get; set; }
        public string Kind { get; set; }
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
