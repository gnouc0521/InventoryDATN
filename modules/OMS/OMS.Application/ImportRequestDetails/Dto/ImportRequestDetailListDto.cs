using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto
{

    [AutoMap(typeof(ImportRequestDetail))]
    public class ImportRequestDetailListDto: Entity<long>
    {
        public long ImportRequestId { get; set; }

        public string ImportRequestCode { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal ImportPrice { get; set; }
        public DateTime ExpireDate { get; set; }
        public int QuantityHT { get; set; }

        [StringLength(150)]
        public string ShipperName { get; set; }

        [StringLength(150)]
        public string ShipperPhone { get; set; }


        //public DateTime MFG { get; set; }
        //public long ParcelId { get; set; }

        //[StringLength(500)]
        //public string ParcelCode { get; set; }

        //[StringLength(1000)]
        //public string Remark { get; set; }

        public string CodeItem { get; set; }

        public string NameItem { get; set; }

        public int TongSl { get; set; }

        public string WarehouseName { get; set; }

        public decimal Thanhtien { get; set; }
    }
}
