using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Works.Dto
{
    public class GetWorkListNumOfType : PagedAndSortedInputDto, IShouldNormalize
    {
        public int Statustest { get; set; }
        public string SearchTerm { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public WorkEnum.Priority? priority { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
