using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.WarehouseCard
{
    public class IndexViewModel
    {
        public WarehouseListDto warehouseList { get; set; }
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
