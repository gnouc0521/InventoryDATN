using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using bbk.netcore.mdl.OMS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using static bbk.netcore.mdl.OMS.Core.Enums.ContractEnum;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppContracts", Schema = netcoreConsts.SchemaName)]
    public class Contract : FullAuditedEntity, ISoftDelete
    {
        public long SupplierId { get; set; }
        public ContractEnum.ContractStatus Status { get; set; }
        public ContractEnum.ExportStatus ExportStatus { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public long QuoteSynId { get; set; }

        public int MouthNumber { get; set; }
        public decimal Price { get; set; }
        public int Indemnify { get; set; }
        public DateTime SellerDate { get; set; }
        public string BankName { get; set; }
        public string Stk { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileUrl { get; set; }
        public string Comment { get; set; }

        public string RepresentA { get; set; }
        public string PositionA { get; set; }

        public string RepresentB { get; set; }
        public string PositionB { get; set; }



    }
}
