using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseItems.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.WareHouseItem
{
    public class WarehouseItemUnitModel
    {
        public List<UnitListDto> unitListDtos { get; set; }
        public WarehouseItemListDto warehouseItem { get; set; }
        public List<RulesListDto> rulesCategory { get; set; }
        public List<SelectListItem> GetUnitList()
        {

            var list = new List<SelectListItem>();
            foreach (var item1 in unitListDtos)
            {
                var optionGroup = new SelectListGroup() { Name = item1.Name };
                foreach (var item in unitListDtos)
                {
                    if (item.ParrentId == item1.Id)
                    {
                        var listItems1 = new SelectListItem
                        {
                            Text = item.Name,
                            Value = item.Id.ToString(),
                            Group = optionGroup,
                        };
                        list.Add(listItems1);
                    }
                }
            }
            return list;
        }

        public List<SelectListItem> GetCategory()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in rulesCategory)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.ItemText,
                    Value = item1.ItemValue.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }
    }
}
