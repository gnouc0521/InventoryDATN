using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ExportRequests.Dto
{
    [AutoMap(typeof(ExportRequest))]
    public class ExportRequestsCreate 
    {
        public string Code { get; set; }
        public int WarehouseDestinationId { get; set; }
        public ExportEnums.ExportType RequestType { get; set; }
        public long SupplierId { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalVal { get; set; }
        public decimal TotalAmount { get; set; }
        public ExportEnums.ExportStatus Status  { get; set; }
        public ExportEnums.Export ExportStatus { get; set; }
        public string Remark { get; set; }
        public DateTime RequestDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string CodeRequirement { get; set; }
        public long SubsidiaryId { get; set; }
        public long TransferId { get; set; }
        public string Comment { get; set; }
        public long IdWarehouseReceiving { get; set; }
        public string CodeTransfer { get; set; }
    }
}
