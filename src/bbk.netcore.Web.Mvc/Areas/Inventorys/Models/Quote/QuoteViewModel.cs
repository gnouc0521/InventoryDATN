using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.Quote
{
    public class QuoteViewModel
    {
        public List<IFormFile> FileUpload { get; set; }
        public string SupplierName { get; set; }
        public string SupplierEmail { get; set; }
        public string Title { get; set; }
    }
}
