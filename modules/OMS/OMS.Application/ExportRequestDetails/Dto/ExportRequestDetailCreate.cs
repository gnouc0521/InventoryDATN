using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ExportRequestDetails.Dto
{
    [AutoMap(typeof(ExportRequestDetail))]
    public class ExportRequestDetailCreate
    {
        public long ExportRequestId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal ExportPrice { get; set; }
        public DateTime ExpireDate { get; set; }
        public long ParcelId { get; set; }
        public string ParcelCode { get; set; }
        public string Remark { get; set; }
        public long WarehouseSourceId { get; set; }
        public long BlockId { get; set; }
        public long FloorId { get; set; }
        public long ShelfId { get; set; }
    }
}
