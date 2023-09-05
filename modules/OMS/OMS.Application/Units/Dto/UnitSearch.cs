using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Units.Dto
{
    public class UnitSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchTerm { get; set; }
        public int? ParrentId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }
    }
   
}
