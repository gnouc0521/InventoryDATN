using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppBonusInfomations", Schema = netcoreConsts.SchemaName)]
    public class BonusInfomation : Entity
    {
        [Required]
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string EmulationTitle { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Bonus { get; set; }

        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string RewardDecisionLevel { get; set; }

        [ForeignKey(nameof(ProfileStaffId))]
        public ProfileStaff ProfileStaff { get; set; }
        public int ProfileStaffId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
    }
}
