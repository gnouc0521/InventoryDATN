using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.Stock
{
    public class WarehouseViewModel
    {
        public List<WarehouseListDto> WarehouseList { get; set; }
        public List<RulesListDto> rulesCategory { get; set; }
        public List<RulesListDto> rulesGroup { get; set; }
        public List<RulesListDto> rulesKind { get; set; }

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
        public List<SelectListItem> GetGroup()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in rulesGroup)
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
        public List<SelectListItem> GetKind()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in rulesKind)
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
