using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Transfers.Dto
{
    public class TransferSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public long TransferId { get; set; }
        public string SearchTerm { get; set; }
        public QuoteEnum.SyntheticQuote Status { get; set; }
        public ContractEnum.ExportStatus? ExportStatus { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "PurchasesSynthesiseCode";
            }
        }
    }
}
