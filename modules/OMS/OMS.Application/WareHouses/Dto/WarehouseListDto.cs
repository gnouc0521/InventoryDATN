using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WareHouses.Dto
{
    [AutoMap(typeof(Warehouse))]
    public class WarehouseListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardsId { get; set; }
        public string Number { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string TypeCode { get; set; }
        public bool DeleteFlag { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string WardsName { get; set; }
        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }
        public WarehouseEnum.TypeWarehouse TypeWarehouse { get; set; }
    }
}
