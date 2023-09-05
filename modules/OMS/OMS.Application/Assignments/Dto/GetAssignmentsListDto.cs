using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Assignments.Dto
{
    [AutoMap(typeof(Assignment))]
    public class GetAssignmentsListDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public long ItemId { get; set; }
    }
}
