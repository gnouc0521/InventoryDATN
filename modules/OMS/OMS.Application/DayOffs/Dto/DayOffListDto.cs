using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.DayOffs.Dto
{
    [AutoMap(typeof(DayOff))]
    public class DayOffListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DayOffEnum.TypeDayOff TypeDayOff { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SumDayOff { get; set; }
        public int Order { get; set; }
    }
}
