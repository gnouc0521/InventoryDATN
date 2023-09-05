using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto
{
    [AutoMapTo(typeof(ProfileWork))]
    public class ProfileWorkCreateDto
    {
       
        public string Title { get; set; }

        public int? ParentId { get; set; }

        public int Order { get; set; }

        public int WorkProfileLevel { get; set; }
        public int UserId { get; set; }
    }
}
