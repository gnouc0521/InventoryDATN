using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WorkGroups.Dto
{
    public class GetWorkGroupInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int? IdWorkGroup { get; set; }
        public string SearchTerm { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
