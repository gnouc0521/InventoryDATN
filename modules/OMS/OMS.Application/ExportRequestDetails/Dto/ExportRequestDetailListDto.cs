using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ExportRequestDetails.Dto
{
    [AutoMap(typeof(ExportRequestDetail))]
    public class ExportRequestDetailListDto : FullAuditedEntityDto<long>
    {
        public long ExportRequestId { get; set; }
        public long ItemId { get; set; }
        public int QuantityExport { get; set; }
        public long QuantityTotal { get; set; }
        public long QuantityLocation { get; set; }
        public long LocationId { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal ExportPrice { get; set; }
        public DateTime ExpireDate { get; set; }
        public long ParcelId { get; set; }
        public string ParcelCode { get; set; }
        public string Remark { get; set; }
        public long WarehouseSourceId { get; set; }
        public long BlockId { get; set; }
        public string BlockName { get; set; }
        public long FloorId { get; set; }
        public string FloorName { get; set; }
        public long ShelfId { get; set; }
        public string ShelfName { get; set; }
        public string ItemsCode { get; set; }
        public string ItemsName { get; set; }

        public decimal Thanhtien { get; set; }   
    }
}
