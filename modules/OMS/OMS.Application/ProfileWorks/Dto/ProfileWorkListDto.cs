using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto
{
    [AutoMap(typeof(ProfileWork))]
    public class ProfileWorkListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int? ParentId { get; set; }

        public int Order { get; set; }
        public int WorkProfileLevel { get; set; }

        //Số phần tử con
        public int NumItemsChild { get; set; }

        public int UserId { get; set; }
    }
}
