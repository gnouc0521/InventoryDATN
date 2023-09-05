using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Works.Dto
{
    [AutoMap(typeof(Work))]
    public class UsersListDto: Entity<long>
    {
        public long? Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
