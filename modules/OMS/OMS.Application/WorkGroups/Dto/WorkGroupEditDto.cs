using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WorkGroups.Dto
{
    [AutoMap(typeof(WorkGroup))]
    public class WorkGroupEditDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }
        public int WorkGroupLevel { get; set; }

    }
}
