using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.ExportRequests
{
  public class ExportRequestModel
    {
        public WarehouseListDto warehouse { get; set; }
        public SubsidiaryListDto subsidiaryDto{ get; set; }
        public string CreatedByName { get; set; }
        public ExportRequestsListDto exportRequests { get; set; }
        public List<ItemsListDto> ItemsListDtos { get; set; }
        public List<SupplierListDto> SuppliersList { get; set; }
        public List<WarehouseListDto> WarehouseList { get; set; }
        public List<SubsidiaryListDto> SubsidiaryList { get; set; }
        public List<SelectListItem> GetItems()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in ItemsListDtos)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.ItemCode + "/" + item1.Name,
                    Value = item1.Id.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }
        public List<SelectListItem> GetSuppliers()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in SuppliersList)
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
        public List<SelectListItem> GetSubsidiary()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in SubsidiaryList)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.NameCompany,
                    Value = item1.Id.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }
    }
}
