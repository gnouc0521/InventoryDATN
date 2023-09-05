using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Works.Dto
{
    public class GetWorkInput : PagedAndSortedInputDto, IShouldNormalize
    {
        
        public string SearchTerm { get; set; }

        public WorkEnum.Status Status { get; set; }

        public WorkEnum.Priority priority { get; set; }

        public WorkEnum.JobLabel jobLabel { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
