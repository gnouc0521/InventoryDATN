using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto
{
    [AutoMapTo(typeof(ImportRequestSubsidiary))]
    public class ImportRequestSubCreateDto
    {
        [StringLength(50)]
        public string Code { get; set; }
        public int WarehouseDestinationId { get; set; }

        public long SuppilerId { get; set; }

        public PurchasesRequestEnum.YCNK ImportStatus { get; set; }

        public string Comment { get; set; }

        public DateTime RequestDate { get; set; }

        public long OrderId { get; set; }

        public string Note { get; set; }
       

    }
}
