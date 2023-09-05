using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Assignments.Dto
{
    [AutoMapTo(typeof(Assignment))]
    public class AssignmentsListDto :FullAuditedEntityDto
    {
        public int UserId { get; set; }
        public long ItemId { get; set; }

        public int AssignmentId { get; set; }
    }
}
