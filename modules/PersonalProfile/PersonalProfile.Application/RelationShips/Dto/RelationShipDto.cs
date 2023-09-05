using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.RelationShips.Dto
{
    public class GetAllFilter
    {
        public int? PersonId { get; set; }

        public RelationType? Type { get; set; }
    }

    [AutoMap(typeof(RelationShip))]
    public class RelationShipDto : AuditedEntityDto<int>
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        public int? YearBirth { get; set; }

        [StringLength(netcoreConsts.MaxSingleLineLength)]
        public string Info { get; set; }

        [Required]
        [StringLength(50)]
        public string RelationName { get; set; }

        [Required]
        public RelationType Type { get; set; }

        public ProfileStaff ProfileStaff { get; set; }
    }
}
