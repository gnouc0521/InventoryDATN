using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestDetailSubidiarys.Dto
{
    [AutoMap(typeof(ImportRequestSubsidiaryDetail))]
    public class ImportRequestDetailSubListDto :FullAuditedEntity
    {
        public int ImportRequestSubsidiaryId { get; set; }
       // public virtual ImportRequestSubsidiary ImportRequestSubsidiary { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public DateTime TimeNeeded { get; set; }
        public string UnitName { get; set; }
        public string Itemcode { get; set; }
    }
}
