using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Itemses.Dto
{
    public class ItemsSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchTerm { get; set; }
        public int WareHouseId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }

    }
}