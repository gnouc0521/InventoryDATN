using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.QuotesSynthesises.Dto
{
    public class QuotesSynthesisSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public long Id { get; set; }    
        public string Name { get; set; }
        public int PurchasesSynthesiseCode { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "PurchasesSynthesiseCode";
            }
        }
    }
}
