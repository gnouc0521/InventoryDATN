using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WarehouseItems.Dto
{
    [AutoMap(typeof(WarehouseItem))]
    public class WarehouseItemListDto
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public long WarehouseId { get; set; }

        public long ParrentId { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string TypeCode { get; set; }

        [StringLength(10)]
        public string PositionX { get; set; }

        [StringLength(10)]
        public string PositionY { get; set; }

        [StringLength(10)]
        public string PositionZ { get; set; }

        public decimal Lenght { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public decimal MaxKgVolume { get; set; }
        public decimal MaxM3Volume { get; set; }
        public string Color { get; set; }
        public bool DeleteFlag { get; set; }

        //Ha Them
        public int NumberChild { get; set; }
        public string ParentName { get; set; }
        public int UnitId { get; set; }
        public int UnitMax { get; set; }
        public string CategoryCode { get; set; }
        public int WarehouseLevel { get; set; }
    }
}
