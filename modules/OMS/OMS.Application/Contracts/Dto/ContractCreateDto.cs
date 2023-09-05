using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Contracts.Dto
{
    [AutoMapTo(typeof(Contract))]
    public class ContractCreateDto
    {
        public long SupplierId { get; set; }
        public long QuoteSynId { get; set; }

        public ContractEnum.ContractStatus Status { get; set; }
        public ContractEnum.ExportStatus ExportStatus  { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

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
        public int Number { get; set; }
    }
}
