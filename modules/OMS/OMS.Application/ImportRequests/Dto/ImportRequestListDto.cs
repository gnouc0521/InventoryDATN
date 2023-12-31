using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ImportRequests.Dto
{
    [AutoMap(typeof(ImportRequest))]
    public class ImportRequestListDto: FullAuditedEntity
    {
        [StringLength(50)]
        public string Code { get; set; }

        public int WarehouseDestinationId { get; set; }
       public int RequestType { get; set; }

        public long SubsidiaryId { get; set; }
        public long SupplierId { get; set; }

        public decimal VAT { get; set; }

        public decimal TotalVal { get; set; }
        public decimal ToatlAmount { get; set; }
        public ImportResquestEnum.ImportResquestStatus ImportStatus { get; set; }

        [StringLength(150)]
        public string ShipperName { get; set; }

        [StringLength(150)]
        public string ShipperPhone { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }
        public long TransferId { get; set; }
        public DateTime RequestDate { get; set; }
        public long ImportRequestSubsidiaryId { get; set; }
        public string NameWareHouse { get; set; }
        public string CreatedBy { get; set; }
        public string TranferCode { get; set; }
        public string YcnkCode { get; set; }
        public string UpdatedBy { get; set; }
        public int QuantityItems { get; set; }
    }
}
