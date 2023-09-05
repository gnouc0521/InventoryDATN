using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.Quotes.Dto
{
    [AutoMap(typeof(Quote))]
    public class QuoteListDto : FullAuditedEntityDto<long>
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }

        public string Note { get; set; }
        public long QuantityQuote { get; set; }

        public int UnitId { get; set; }
        public string UnitName { get; set; }

        public decimal QuotePrice { get; set; }
        public decimal TotalNumber { get; set; }
        public string Specifications { get; set; }
        public string SymbolCode { get; set; }
        public string QuoteSynCode { get; set; }

        public string NameStaff { get; set; }
        public long UserId { get; set; }
        public long QuoteSynthesiseId { get; set; }
        public long QuoteRequestsId { get; set; }
        public long ContractId { get; set; }

        public List<DateTime> QuoteDate { get; set; }
        public List<QuoteHistory> QuoteHistoryList { get; set; }
        //Ha Them
        public List<string> SupplierNames { get; set; }
        public List<TimeAndId> timeAndIds { get; set; }

        ///kiemm them
        public int QuantityHT { get; set; }
    }
    public class QuoteHistory
    {
        public List<DateTime> QuoteDate { get; set; }
        public List<long> QouteIdList { get; set; }
        public List<string> SupplierNames { get; set; }
        //Ha them 
        public long ItemId { get; set; }
        public string SupplierName { get; set; }
        public List<TimeAndId> timeAndIds { get; set; }



     
    }

    public class TimeAndId
    {
        public long IdQuotes { get; set; }
        public DateTime TimeCre { get; set; }
    }

}
