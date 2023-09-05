using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WorkGroups.Dto
{
    [AutoMapTo(typeof(WorkGroup))]
    public class WordGroupCreateDto
    {
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }
        public int WorkGroupLevel { get; set; }
    }
}
