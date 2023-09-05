using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Experts.Dto
{
    [AutoMapTo(typeof(Expert))]
    public class ExpertCreateDto
    {
        public string ExpertCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
    }
}
