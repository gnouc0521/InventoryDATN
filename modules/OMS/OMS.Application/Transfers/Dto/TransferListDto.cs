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
    public class TransferListDto : FullAuditedEntityDto<long>
    {
        public long Id { get; set; }
        public string TransferCode { get; set; }
        public int IdWarehouseExport { get; set; }
        public DateTime BrowsingTime { get; set; }
        public TransferEnum.TransferStatus Status { get; set; }
        public ContractEnum.ExportStatus ExportStatus { get; set; }
        public string TransferNote { get; set; }

        //kiem them
        public string NameWareHouseExport { get; set; }

        public string NameWareHouseReceiving { get; set; }

        public int IdWarehouseReceiving { get; set; }

        public bool StatusImportPrice { get; set; }
        public string CommentText { get; set; }

        public List<string> ListNameWareHouseReceiving { get; set; }

        public string Email { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
    }
}
