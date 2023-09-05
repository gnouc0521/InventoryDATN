using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using AutoMapper.Configuration.Annotations;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses.Dto
{
    [AutoMap(typeof(WorkingProcess))]
    public class WorkingProcessDto : AuditedEntityDto<int>
    {
        [Required]
        public int PersonId { get; set; }

        public long? DocumentId { get; set; }

        [Ignore]
        public string DecisionNumber { get; set; }

        [Ignore]
        public DateTime? IssuedDate { get; set; }

        [StringLength(100)]
        public string JobPosition { get; set; }

        [Required]
        public int WorkingTitleId { get; set; }

        public int? DecisionMakerId { get; set; }

        public long? OrgId { get; set; }

        [Required]
        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string OtherOrg { get; set; }

        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string DepartmentName { get; set; }

        [StringLength(100)]
        public string TypeOfChangeString { get; set; }

        public int? TypeOfChangeId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
