using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.WarehouseLocationItem.Dto
{
    public class WarehouseLocationItemsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string SearchTerm { get; set; }
        public int WarehouseId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string SupplierId { get; set; }
        public string ProducerId { get; set; }

        public long ItemId { get; set; }
        public long ImportId { get; set; }
        public long ImportRequestDetailId { get; set; }
        public string Block { get; set; }   
        public string Shelf { get; set; }   
        public string Floor { get; set; }
        public int Year { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}
