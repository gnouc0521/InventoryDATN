using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WarehouseTypes.Dto
{
    [AutoMap(typeof(WarehouseType))]
    public class WarehouseTypeListDto : EntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }

}
