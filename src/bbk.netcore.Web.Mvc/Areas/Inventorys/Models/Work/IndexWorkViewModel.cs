using bbk.netcore.mdl.PersonalProfile.Application.Categories.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto;
using bbk.netcore.mdl.OMS.Application.WorkGroups.Dto;
using Microsoft.AspNetCore.Http;
using Abp.Domain.Entities;

namespace bbk.netcore.Web.Areas.Inventorys.Models.Work
{
    public class IndexWorkViewModel : Entity<long>
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int IdWorkGroup { get; set; }
        public int IdProfileWork { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime CompletionTime { get; set; }
        public WorkEnum.Status Status { get; set; }

        public WorkEnum.Priority priority { get; set; }

        public WorkEnum.JobLabel jobLabel { get; set; }

        public long UserId { get; set; }

        public int HostId { get; set; }
        public string WorkUser { get; set; }
        public string HostUser { get; set; }
        public string Attachment { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public int? statusId { get; set; }
        public IFormFile[] FileUpload { get; set; }

        public IReadOnlyList<ProfileWorkListDto> profileWorkGroupListDtos { get; set; }
        public IReadOnlyList<WorkGroupListDto> workGroupListDtos { get; set; }

        public List<User> Users { get; set; }

        public List<SelectListItem> GetSelectListItemWorkGroup()
        {
            var list = new List<SelectListItem>();
            var groups = workGroupListDtos.Where(u => u.ParentId == null);
            var items = workGroupListDtos.Where(u => u.ParentId != null);
            var item1s = workGroupListDtos.Where(u => u.ParentId != null);
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

      
        public List<SelectListItem> GetSelectUser()
        {
            var list = new List<SelectListItem>();

            foreach (var item1 in Users)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.Surname +" "+ item1.Name,
                    Value = item1.Id.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }
    }
}
