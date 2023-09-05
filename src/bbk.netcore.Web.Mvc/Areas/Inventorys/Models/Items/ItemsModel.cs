using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.Inventorys.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.Items
{
    public class ItemsModel
    {
        public IFormFile Excel { get; set; }
        public InventoryListDto inventoryListDto { get; set; }  
        public WarehouseListDto WarehouseList {get; set;}
        public ItemsListDto Items { get; set; }
        public List<SupplierListDto> Suppliers { get; set; }
        public List<UnitListDto> unitListDtos { get; set; }
        public List<ProducerListDto> Producers { get; set; }
        public List<RulesListDto> rulesCategory { get; set; }
        public List<RulesListDto> rulesGroup { get; set; }
        public List<RulesListDto> rulesKind { get; set; }
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
        public List<SelectListItem> GetProducers()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in Producers)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.Name,
                    Value = item1.Code.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }
        public List<SelectListItem> GetSuppliers()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in Suppliers)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.Name,
                    Value = item1.Code.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }
        public List<SelectListItem> GetUnitList()
        {
          
            var list = new List<SelectListItem>();
            foreach (var item1 in unitListDtos)
            {
                var optionGroup = new SelectListGroup() { Name  = item1.Name };
                foreach (var item in unitListDtos)
                {
                    if(item.ParrentId == item1.Id)
                    {
                        var listItems1 = new SelectListItem
                        {
                            Text = item.Name,
                            Value = item.Name,
                            Group = optionGroup,
                        };
                        list.Add(listItems1);
                    }
                }
            }
            return list;
        }
    }
}
