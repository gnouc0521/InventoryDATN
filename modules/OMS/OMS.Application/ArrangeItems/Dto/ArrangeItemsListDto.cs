using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ArrangeItems.Dto
{
    [AutoMap(typeof(WarehouseLocationItems))]
    public class ArrangeItemsListDto : FullAuditedEntityDto<long>
    {
        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public long ParcelId { get; set; }
        public string Block { get; set; }
        public string Shelf { get; set; }
        public string Floor { get; set; }
        public string DescriptionLocation { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpireDate { get; set; }
        public long ImportRequestId { get; set; }
        public long ImportRequestDetailId { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
