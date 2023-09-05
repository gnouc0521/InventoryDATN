using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ExportRequests.Dto
{
    [AutoMap(typeof(ExportRequest))]
    public class ExportRequestsListDto : FullAuditedEntityDto<long>
    {
        public string Code { get; set; }
        public int? WarehouseDestinationId { get; set; }
        public string WarehouseDestinationName { get; set; }  
        public int? WarehouseSourceId { get; set; }
        public string WarehouseSourceName { get; set; }
        public ExportEnums.ExportType RequestType { get; set; }
        public long SupplierId { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalVal { get; set; }
        public decimal TotalAmount { get; set; }
        public ExportEnums.ExportStatus Status { get; set; }
        public ExportEnums.Export ExportStatus { get; set; }
        public string Remark { get; set; }
        public DateTime RequestDate { get; set; }
      //thời gian xuất kho
        public List<DateTime> ExportTime { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CreateBy { get; set; }
        public string ReceiverName { get; set; }
        public string CodeRequirement { get; set; }
        public string TranferCode { get; set; }
        public long SubsidiaryId { get; set; }
        public long TransferId { get; set; }
        public string SubsidiaryName { get; set; }
        public string Comment { get; set; }
        public long IdWarehouseReceiving { get; set; }
        public string CodeTransfer { get; set; }
        public List<string> ListWarehouseSourceName { get; set;}


        //kienb
        public string Email { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
