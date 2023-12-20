using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.WarehouseCard
{
  public class WarehouseCardViewModel : FullAuditedEntity
    {
        
        public string Code { get; set; }
        public int WarehouseDestinationId { get; set; }
        public int RequestType { get; set; }
        public long SupplierId { get; set; }
        //public decimal VAT { get; set; }
        //public decimal TotalVal { get; set; }
        //public decimal ToatlAmount { get; set; }
        public ImportResquestEnum.ImportResquestStatus ImportStatus { get; set; }
        public string ShipperName { get; set; }
        public ImportRequestListDto impRequests { get; set; }
        public List<ImportRequestListDto> ListImpRequests { get; set; }
        public List<ImportRequestDetailListDto> ImportRequestDetailListDto { get; set; }

        public ImportRequestSubListDto ImportRequestSubsidiary { get; set; }
        public string ShipperPhone { get; set; }
        public string Remark { get; set; }
        public long TransferId { get; set; }
        public string TransferCode { get; set; }
        public long SubsidiaryId { get; set; }
        public long ImportRequestSubsidiaryId { get; set; }

        public string TransferNote { get; set; }
        public int IdWarehouseExport { get; set; }
        public int IdWarehouseReceiving { get; set; }
        public DateTime BrowsingTime { get; set; }
        public TransferEnum.TransferStatus Status2 { get; set; }
        public DateTime RequestDate { get; set; }

        public string CreatedBy;
        public string UpdateBy;
        public string NameNCC { get; set; }
        public string AdressNCC { get; set; }
        public string NameWareHouse { get; set; }
        public string DiaChiKho { get; set; }

        public List<SupplierListDto> Suppliers { get; set; }
        public List<SelectListItem> GetSuppliers()
        {
            var list = new List<SelectListItem>();

            foreach (var item1 in Suppliers)
            {
                var listItems1 = new SelectListItem
                {

                    Text = item1.Name,
                    Value = item1.Id.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }

        public List<WarehouseListDto> WarehouseList { get; set; }
      

        public List<SelectListItem> GetWarehouse()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in WarehouseList)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.Name,
                    Value = item1.Id.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }
    }
}
