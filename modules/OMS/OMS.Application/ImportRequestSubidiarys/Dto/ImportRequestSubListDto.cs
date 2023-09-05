using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto
{
    [AutoMap(typeof(ImportRequestSubsidiary))]
    public class ImportRequestSubListDto : FullAuditedEntityDto
    {
        public string Code { get; set; }
        public int WarehouseDestinationId { get; set; }

        public long SuppilerId { get; set; }

        public PurchasesRequestEnum.YCNK ImportStatus { get; set; }

        public string Comment { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime Browsingtime { get; set; }

        public long OrderId { get; set; }

        public string Note { get; set; }

        public string CreatedBy { get; set; }

        public string NameWareHouse { get; set; }
        public string NameSup { get; set; }
        public long ImportRequestSubsidiaryId { get; set; }
        public bool StatusImport { get; set; }


        public string Email { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
