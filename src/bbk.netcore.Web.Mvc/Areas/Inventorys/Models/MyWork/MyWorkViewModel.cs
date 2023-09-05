using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.MyWork
{
    public class MyWorkViewModel
    {

        public long SupplierId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileUrl { get; set; }

        public string Comment { get; set; }

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
