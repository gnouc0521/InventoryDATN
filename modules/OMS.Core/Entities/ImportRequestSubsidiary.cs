using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Entities
{
    [Table("AppImportRequestSubsidiary", Schema = netcoreConsts.SchemaName)]
    public class ImportRequestSubsidiary : FullAuditedEntity
    {
        [StringLength(50)]
        public string Code { get; set; }
        public int WarehouseDestinationId { get; set; }

        public long SuppilerId { get; set; }

        public PurchasesRequestEnum.YCNK ImportStatus { get; set; }

        public string Comment { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime Browsingtime { get; set; }

        public long OrderId { get; set; }

        public string Note { get; set; }

        public bool StatusImport { get; set; }
        

    }
}
