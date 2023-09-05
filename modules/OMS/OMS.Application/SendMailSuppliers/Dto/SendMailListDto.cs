using AutoMapper;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.SendMailSuppliers.Dto
{
    [AutoMap(typeof(SendMailSupplier))]
    public class SendMailListDto
    {
        public long SupplierId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileUrl { get; set; }

        public string SubJect { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public string NameSup { get; set; }

        public string EmailSup { get; set; }

        public List<IFormFile> FileUpload { get; set; }

        public List<SupplierListDto> Suppliers { get; set; }

        public List<SelectListItem> GetSuppliers()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in Suppliers)
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
