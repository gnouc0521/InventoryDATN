using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Suppliers.Dto
{
    public class GetSupplierInput : PagedAndSortedInputDto, IShouldNormalize
    
    {
        public string SearchTerm { get; set; }
        public long supplierId { get;set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
