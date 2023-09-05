using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.QuotesRelationships.Dto
{
    [AutoMap(typeof(QuoteRelationship))]
    public class QuotesRelationshipListDto : FullAuditedEntityDto<long>
    {
        public long QuoteSynthesiseId { get; set; }
        public long QuoteId { get; set; }

    }
}
