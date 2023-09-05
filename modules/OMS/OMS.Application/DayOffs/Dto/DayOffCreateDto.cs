using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.DayOffs.Dto
{
    [AutoMapTo(typeof(DayOff))]
    public class DayOffCreateDto
    {
        public string Title { get; set; }
        public DayOffEnum.TypeDayOff TypeDayOff { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int SumDayOff { get; set; }
        public int Order { get; set; }
    }
}
