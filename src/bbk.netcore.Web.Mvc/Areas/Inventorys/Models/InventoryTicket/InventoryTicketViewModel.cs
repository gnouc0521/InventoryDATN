﻿using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.InventoryTicket
{
    public class InventoryTicketViewModel : FullAuditedEntity
    {
        public string Code { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string NameWareHouse { get; set; }
        public DateTime CompleteTime { get; set; }
        public string RoleName { get; set; }

        public long WarehouseId { get; set; }
        public long ItemId { get; set; }
       
        public string CreatedBy { get; set; }
        public List<WarehouseListDto> WarehouseList { get; set; }

        public List<ItemsListDto> ItemsList { get; set; }

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

        public List<SelectListItem> GetItems()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in ItemsList)
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

    }
}
