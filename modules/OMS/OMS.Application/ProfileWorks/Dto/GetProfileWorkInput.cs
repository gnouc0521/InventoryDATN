using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto
{
    public class GetProfileWorkInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int? IdProfileWork { get; set; }
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
