using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppSubsidiary", Schema = netcoreConsts.SchemaName)]
    public class Subsidiary : FullAuditedEntity<long>
    {
        public string NameCompany { get; set; }
        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public DateTime RequestDate { get; set; }

        [StringLength(10)]
        public string CityId { get; set; }

        [StringLength(10)]
        public string DistrictId { get; set; }

        [StringLength(10)]
        public string WardsId { get; set; }

    }
}
