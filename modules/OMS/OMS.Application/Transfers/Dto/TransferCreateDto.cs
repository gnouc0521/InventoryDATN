using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Transfers.Dto
{
    [AutoMap(typeof(Transfer))]
    public class TransferCreateDto 
    {
        public string TransferCode { get; set; }
        public int IdWarehouseExport { get; set; }
        public DateTime BrowsingTime { get; set; }
        public string TransferNote { get; set; }
        public TransferEnum.TransferStatus Status { get; set; }
        public ContractEnum.ExportStatus ExportStatus { get; set; }
        public bool StatusImportPrice { get; set; }
        public string CommentText { get; set; }
    }
}
