using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto
{
    [AutoMapTo(typeof(ImportRequestDetail))]
    public class ImportRequestDetailCreateDto
    {
        public long ImportRequestId { get; set; }

        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal ImportPrice { get; set; }
        public int QuantityHT { get; set; }
        public DateTime ExpireDate { get; set; }

        [StringLength(150)]
        public string ShipperName { get; set; }

        [StringLength(150)]
        public string ShipperPhone { get; set; }
        //  public DateTime MFG { get; set; }
        //public long ParcelId { get; set; }

        //[StringLength(500)]
        //public string ParcelCode { get; set; }

        //[StringLength(1000)]
        //public string Remark { get; set; }
    }
}
