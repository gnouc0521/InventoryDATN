using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Application.ImportRequestSubidiarys.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.IMBSub
{
    public class IndexViewModel 
    {
        public string NameNCC { get; set; }
        public string NameWareHouse { get; set; }
        public long OrderId { get; set; }
        public long IdSupplier { get; set; }
        public string OrderCode { get; set; }
        public ImportRequestSubListDto subListDto { get; set; } 
        public WarehouseListDto warehouseList { get; set; }
        public List<WarehouseListDto> WarehouseList { get; set; }
        public string CreatedBy;
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
    }
}
