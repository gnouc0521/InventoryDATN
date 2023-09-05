using bbk.netcore.mdl.OMS.Application.WorkGroups.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace bbk.netcore.Web.Areas.Inventorys.Models.WorkGroups
{
    public class IndexViewWorkGroupModel
    {
        public IReadOnlyList<WorkGroupListDto> workGroupListDtos { get; set; }
        //public List<SelectListItem> GetSelectListItemWorkGroup()
        //{
        //    var list = new List<SelectListItem>();
        //    var groups = workGroupListDtos.Where(u => u.ParentId == null);
        //    var items = workGroupListDtos.Where(u => u.ParentId != null);
        //    foreach (var group in groups)
        //    {
        //        var listGroup = new SelectListGroup
        //        {
        //            Name = group.Title,
        //            Disabled = false
        //        };
        //        foreach (var item in items.Where(u => u.ParentId == group.Id))
        //        {
        //            var listItems = new SelectListItem
        //            {
        //                Text = item.Title,
        //                Value = item.Id.ToString(),
        //                Group = listGroup
        //            };
        //            list.Add(listItems);
        //        }
        //    }
        //    return list;
        //}

        public List<SelectListItem> GetSelectListItemWorkGroup()
        {
            var list = new List<SelectListItem>();
            var groups = workGroupListDtos.Where(u => u.ParentId == null);
            var items = workGroupListDtos.Where(u => u.ParentId != null).ToArray();
            var item1s = workGroupListDtos.Where(u => u.ParentId != null);

            foreach (var group in groups)
            {
                var listGroup = new SelectListItem
                {
                    Text = "["+group.Title+"]",
                    Value = group.Id.ToString(),
                };
                list.Add(listGroup);
                //foreach (var item in items.Where(u => u.ParentId == group.Id))
                //{
                //    var listItems = new SelectListItem
                //    {
                //        Text = "--"+item.Title,
                //        Value = item.Id.ToString(),
                //    };
                //    list.Add(listItems);
                //    foreach (var item1 in item1s.Where(i => i.ParentId == item.Id))
                //    {
                //        var listItem1s = new SelectListItem
                //        {
                //            Text = "----" + item1.Title,
                //            Value = item1.Id.ToString(),
                //        };
                //        list.Add(listItem1s);
                //    }
                //}
            }

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < items.Length; j++)
                {
                    if (items[j].ParentId != int.Parse(list[i].Value)) {
                        continue;
                    }
                    else
                    {
                        var listitems = new SelectListItem
                        {
                            Text = "-" + items[j].Title,
                            Value = items[j].Id.ToString(),
                        };

                        list.Add(listitems);
                        continue;
                    }
                }
            }


            return list;
        }
    }
}
