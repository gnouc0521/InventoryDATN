using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WareHouses.Dto
{
    [AutoMapTo(typeof(Warehouse))]
    public class WarehouseCreateDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public WarehouseEnum.TypeWarehouse TypeWarehouse { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardsId { get; set; }
        public string Number { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string TypeCode { get; set; }
        public bool DeleteFlag { get; set; }
        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }
    }
}
