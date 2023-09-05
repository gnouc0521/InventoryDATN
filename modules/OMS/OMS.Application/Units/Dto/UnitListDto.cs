using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Units.Dto
{
    [AutoMap(typeof(Unit))]
    public class UnitListDto : EntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public int? ParrentId { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
    }
}
