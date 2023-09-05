using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto
{
    [AutoMap(typeof(Subsidiary))]
    public class SubsidiaryListDto : Entity<long>
    {
        public Address provinces { get; set; }
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
