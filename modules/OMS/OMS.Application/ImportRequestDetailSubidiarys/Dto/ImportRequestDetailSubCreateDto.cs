using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestDetailSubidiarys.Dto
{
    [AutoMapTo(typeof(ImportRequestSubsidiaryDetail))]
    public class ImportRequestDetailSubCreateDto
    {
        public int ImportRequestSubsidiaryId { get; set; }
        public virtual ImportRequestSubsidiary ImportRequestSubsidiary { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        public string unitName { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public DateTime TimeNeeded { get; set; }
    }
}
