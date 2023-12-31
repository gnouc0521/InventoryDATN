using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ImportRequests.Dto
{
    public class GetImportRequestInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchTerm { get; set; }
        public int WarehouseDestinationId { get; set; }
        public int CreatorById { get; set; }
        public int Status { get; set; }
        public string ResquestDate { get; set; }
        public long ItemsId { get; set; }

        //public string NameRequest { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
