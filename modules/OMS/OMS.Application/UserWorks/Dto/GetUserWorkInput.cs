using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.UserWorks.Dto
{
    public class GetUserWorkInput : PagedAndSortedInputDto, IShouldNormalize
    {
        
        public string SearchTerm { get; set; }

        public WorkEnum.Status Status { get; set; }
        public WorkEnum.OwnerStatusEnum OwnerStatus { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
