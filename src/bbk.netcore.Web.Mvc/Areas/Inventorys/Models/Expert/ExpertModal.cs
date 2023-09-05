using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Producers.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.Expert
{
    public class ExpertModal
    {
        public List<ItemsListDto> itemlist { get; set; }

        public List<SelectListItem> GetItem()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in itemlist)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.Name,
                    Value = item1.ItemCode,
                };
                list.Add(listItems1);
            }
            return list;
        }
    }
}
