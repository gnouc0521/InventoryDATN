using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.QuotesRequests.Dto
{
    [AutoMap(typeof(QuoteRequest))]
    public class QuoteRequestsListDto : FullAuditedEntityDto<long>
    {
        public long QuoteId { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }

        public string Note { get; set; }
        public long QuantityQuote { get; set; }
        public long QuotesSynthesiseId { get; set; }

        public int UnitId { get; set; }
        public string UnitName { get; set; }

        public decimal QuotePrice { get; set; }
        public decimal TotalNumber { get; set; }
        public string Specifications { get; set; }
        public string SymbolCode { get; set; }
        public string QuoteSynCode { get; set; }

        public string NameStaff { get; set; }
        public long UserId { get; set; }

        public List<DateTime> QuoteDate { get; set; }
     
        public List<QuoteRequestsHistory> QuoteHistoryList { get; set; }
        //Ha Them
        public List<string> SupplierNames { get; set; }
        public List<TimeAndIdRequests> timeAndIds { get; set; }
    }
    public class QuoteRequestsHistory
    {
        public List<DateTime> QuoteDate { get; set; }
        public List<long> QouteIdList { get; set; }
        public List<string> SupplierNames { get; set; }
        //Ha them 
        public long ItemId { get; set; }
        public string SupplierName { get; set; }
        public List<TimeAndIdRequests> timeAndIds { get; set; }
    }

    public class TimeAndIdRequests
    {
        public long IdQuotes { get; set; }
        public DateTime TimeCre { get; set; }
    }

}
