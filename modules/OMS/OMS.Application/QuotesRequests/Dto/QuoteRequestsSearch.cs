using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.QuotesRequests.Dto
{
    public class QuoteRequestsSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchTerm { get; set; }
        public long ItemsId { get; set; }
        public long? Id { get; set; }
        public string Year { get; set; }
        public int? Quy { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }

    }
}