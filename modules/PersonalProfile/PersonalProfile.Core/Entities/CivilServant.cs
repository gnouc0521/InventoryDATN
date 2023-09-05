using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppCivilServants", Schema = netcoreConsts.SchemaName)]
    public class CivilServant : Entity
    {
        [StringLength(netcoreConsts.MaxShortLineLength)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        public CivilServantGroup Group { get; set; }
    }
}
