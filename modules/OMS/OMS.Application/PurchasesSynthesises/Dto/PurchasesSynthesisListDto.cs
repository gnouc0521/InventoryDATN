using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto
{
    [AutoMap(typeof(PurchasesSynthesise))]
    public class PurchasesSynthesisListDto : FullAuditedEntityDto<long>
    {
        public bool StatusAssignment { get; set; }
        public long PurchasesSynthesiseId { get; set; }
        public long PurchasesDetailId { get; set; }
        public string PurchasesSynthesiseCode { get; set; }  
        public long PurchasesRequetsId { get; set; }
        public string CreateName { get; set; } 
        public long UserId { get; set; }
        public string ItemsName { get; set; }   
        public long ItemsId { get; set; }   
        public int QuantityItems { get; set; }
        public string  UnitName { get; set; }
        public int  UnitId { get; set; }
        public string NameStaff { get; set; }
        public string Itemcode { get; set; }
        public string SubsidiariesName { get; set; }
        public List<string> Supplier { get; set; }
        public long SubsidiariesId { get; set; }
        public string SupplierName { get; set; }
        public long SupplierId { get; set; }
        public string Comment { get; set; }
        public PurchasesRequestEnum.YCNK PurchasesRequestStatus { get; set; }
        public PurchasesRequestEnum.MyworkStatus Status { get; set; }
        public PurchasesRequestEnum.GetPriceSupkStatus GetPriceStatus { get; set; }
        public int ActualAmount { get; set; }
        public string Note { get; set; }
        public string Purpose { get; set; }
        public DateTime? DateAssignment { get; set; }
        public DateTime DateTimeNeed { get; set; }
        public DateTime CompleteDate { get; set; }
        public List<string> subsidiaries { get; set; }  
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string Email { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }

    }
}
