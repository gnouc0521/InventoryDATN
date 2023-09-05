using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Contracts.Dto
{
    public class ContractSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public long Id { get; set; }
        public int Supper { get; set; }
        public int quoSyn { get; set; }
        public string SearchTerm { get; set; }

        public ContractEnum.ContractStatus? status { get; set; }
        public ContractEnum.ExportStatus? ExportStatus { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Code";
            }
        }
    }

}
