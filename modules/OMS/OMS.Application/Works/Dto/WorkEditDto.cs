using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto;
using bbk.netcore.mdl.OMS.Application.UserWorks.Dto;
using bbk.netcore.mdl.OMS.Application.WorkGroups.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bbk.netcore.mdl.OMS.Application.Works.Dto
{
    [AutoMap(typeof(bbk.netcore.mdl.OMS.Core.Entities.Work))]
    public class WorkEditDto : EntityDto<long>
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string WorkGroup { get; set; }
        public int IdWorkGroup { get; set; }
        public int IdProfileWork { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime CompletionTime { get; set; }
        public WorkEnum.Status Status { get; set; }

        public WorkEnum.Priority priority { get; set; }

        public WorkEnum.JobLabel jobLabel { get; set; }
      
        public int UserId { get; set; }

        public int HostId { get; set; }
        public string WorkUser { get; set; }
        public string HostUser { get; set; }

        public string FilePath { get; set; }
        public string FileUrl { get; set; }

        public IFormFile[] FileUpload { get; set; }

        public IReadOnlyList<ProfileWorkListDto> profileWorkGroupListDtos { get; set; }
        public IReadOnlyList<WorkGroupListDto> workGroupListDtos { get; set; }
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
                        foreach (var item2 in item1s.Where(i => i.ParentId == item1.Id))
                        {
                            var listItem2s = new SelectListItem
                            {
                                Text = "----" + item2.Title,
                                Value = item2.Id.ToString(),
                            };
                            list.Add(listItem2s);
                            foreach (var item3 in item1s.Where(i => i.ParentId == item2.Id))
                            {
                                var listItem3s = new SelectListItem
                                {
                                    Text = "-----" + item3.Title,
                                    Value = item3.Id.ToString(),
                                };
                                list.Add(listItem3s);
                            }
                        }
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
                            Text = "---" + item1.Title,
                            Value = item1.Id.ToString(),
                        };
                        list.Add(listItem1s);
                        foreach (var item2 in item1s.Where(i => i.ParentId == item1.Id))
                        {
                            var listItem2s = new SelectListItem
                            {
                                Text = "----" + item2.Title,
                                Value = item2.Id.ToString(),
                            };
                            list.Add(listItem2s);
                            foreach (var item3 in item1s.Where(i => i.ParentId == item2.Id))
                            {
                                var listItem3s = new SelectListItem
                                {
                                    Text = "-----" + item3.Title,
                                    Value = item3.Id.ToString(),
                                };
                                list.Add(listItem3s);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
