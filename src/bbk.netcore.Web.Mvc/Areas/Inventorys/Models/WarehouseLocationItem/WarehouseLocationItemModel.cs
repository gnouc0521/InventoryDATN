using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.WarehouseLocationItem
{
    public class WarehouseLocationItemModel
    {
        public List<WarehouseListDto> ListWarehouse { get; set; }

        public List<ProducerListDto> ListProducer { get; set; }
        public List<SupplierListDto> ListSupplier { get; set; }

        public List<SelectListItem> GetWarehouse()
        {
            var list = new List<SelectListItem>();

            foreach (var item in ListWarehouse)
            {
                var listItem = new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                };
                list.Add(listItem);
            }
            return list;
        }

        public List<SelectListItem> GetProducer()
        {
            var list = new List<SelectListItem>();

            foreach (var item in ListProducer)
            {
                var listItem = new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Code,
                };
                list.Add(listItem);
            }
            return list;
        }

        public List<SelectListItem> GetSupplier()
        {
            var list = new List<SelectListItem>();

            foreach (var item in ListSupplier)
            {
                var listItem = new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Code,
                };
                list.Add(listItem);
            }
            return list;
        }
    }
}
