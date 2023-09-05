using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Dashboard.Dto
{
    public class DashboardInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int NumInTime { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public long UserId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
