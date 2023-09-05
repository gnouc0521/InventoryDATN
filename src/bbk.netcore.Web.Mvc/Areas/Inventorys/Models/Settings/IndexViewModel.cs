
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.Web.Areas.Inventorys.Models.Settings
{
    public class IndexViewModel
    {
        [Required]
        public string Header { get; set; }
        [Required]
        public string Footer { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFile Banner { get; set; }
    }
}
