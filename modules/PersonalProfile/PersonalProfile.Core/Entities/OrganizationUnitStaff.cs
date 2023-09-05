using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppOrganizationUnitStaffs", Schema = netcoreConsts.SchemaName)]
    public class OrganizationUnitStaff : Entity<long>, IHasCreationTime
    {
        [Required]
        public int StaffId { get; set; }

        [Required]
        public long OrganizationUnitId { get; set; }

        [ForeignKey(nameof(StaffId))]
        public ProfileStaff ProfileStaff { get; set; }

        public DateTime CreationTime { get; set; }

        [ForeignKey(nameof(OrganizationUnitId))]
        public OrganizationUnit OrganizationUnit { get; set; }
    }
}
