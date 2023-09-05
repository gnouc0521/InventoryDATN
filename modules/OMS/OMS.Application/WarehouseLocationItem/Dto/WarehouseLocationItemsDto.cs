using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WarehouseLocationItem.Dto
{
    [AutoMap(typeof(WarehouseLocationItems))]
    public class WarehouseLocationItemsDto : FullAuditedEntityDto<long>
    {
        public long? Id { get; set; }
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public long ParcelId { get; set; }
        public string Block { get; set; }
        public string LocationName { get; set; }
        public string BlockName { get; set; }
        public string Shelf { get; set; }
        public string Floor { get; set; }
        public string DescriptionLocation { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpireDate { get; set; }
        public long ImportRequestId { get; set; }
        public long ImportRequestDetailId { get; set; }
        public DateTime ImportDate { get; set; }
        public string WarehouseName { get; set; }
        public string ImportRequestCode { get; set; }
        public int QuantityImport { get; set; }
        public int QuantityLocaiton { get; set; }
        public string ItemsName { get; set; }
        public string ShelfName { get; set; }
        public string FloorName { get; set; }

        public string UnitName { get; set; }
        public string CodeLocation { get; set; }
        public long WarehouseLocationItemsId { get; set; }
        public bool IsItems { get; set; }
        public ImportResquestEnum.ImportResquestStatus ImportStatus { get; set; }
        public int QuantityReality { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }


    }
}
