using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.OrganizationUnitStaffs.Dto
{
    [AutoMap(typeof(OrganizationUnitStaff))]
    public class OrganizationUnitStaffDto : EntityDto<long>
    {
        [Required]
        public int StaffId { get; set; }

        [Required]
        public long OrganizationUnitId { get; set; }
    }
}
