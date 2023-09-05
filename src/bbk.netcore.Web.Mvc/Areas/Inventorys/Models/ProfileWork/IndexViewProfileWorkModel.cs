using bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace bbk.netcore.Web.Areas.Inventorys.Models.ProfileWork
{
    public class IndexViewProfileWorkModel
    {
        public IReadOnlyList<ProfileWorkListDto> profileWorkGroupListDtos { get; set; }

        public List<SelectListItem> GetSelectListItemProfileWorkGroup()
        {
            var list = new List<SelectListItem>();
            var groups = profileWorkGroupListDtos.Where(u => u.ParentId == null);
            var items = profileWorkGroupListDtos.Where(u => u.ParentId != null);
            var item1s = profileWorkGroupListDtos.Where(u => u.ParentId != null);
            foreach (var group in groups)
            {
                var listGroup = new SelectListItem
                {
                    Text = "[" + group.Title + "]",
                    Value = group.Id.ToString(),
                };
                list.Add(listGroup);
                foreach (var item in items.Where(u => u.ParentId == group.Id))
                {
                    var listItems = new SelectListItem
                    {
                        Text = "--" + item.Title,
                        Value = item.Id.ToString(),
                    };
                    list.Add(listItems);
                    foreach (var item1 in item1s.Where(i => i.ParentId == item.Id))
                    {
                        var listItem1s = new SelectListItem
                        {
                            Text = "----" + item1.Title,
                            Value = item1.Id.ToString(),
                        };
                        list.Add(listItem1s);
                    }
                }
            }
            return list;
        }
    }
}
