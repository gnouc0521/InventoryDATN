using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto
{
    [AutoMap(typeof(QuoteSynthesise))]
    public class QuotesSynthesisListDto : FullAuditedEntityDto<long>
    {
        public string Code { get; set; }
        public string Comment { get; set; }
        public DateTime QuoteSynthesiseDate { get; set; }
        public string SuppliersName { get; set; }
        public string CreateName { get; set; }
        public DateTime CreateDate { get; set; }
        public QuoteEnum.SyntheticQuote Status { get; set; }

        //Ha Taoo
        public List<string> SuppliersNames { get; set; }

        public List<SupplierList> supplierList { get; set; }
        public class SupplierList
        {
            public long SupplierNameId { get; set; }
            public string SupplierName { get; set; }
          
        }

        //kieb 
        public string Email { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
    }
}
