using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ScheduleWorks.Dto
{
    [AutoMap(typeof(ScheduleWork))]
    public class ScheduleWorkEditDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public string Address { get; set; }
        public int? Repeat { get; set; }
        public ScheduleWorkEnum.ColorPriority Color { get; set; }
    }
}
