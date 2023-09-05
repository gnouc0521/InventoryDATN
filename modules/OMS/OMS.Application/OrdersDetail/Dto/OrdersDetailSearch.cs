using Abp.Configuration;
using Abp.Runtime.Validation;
using bbk.netcore.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto
{
   public  class OrdersDetailSearch : PagedAndSortedInputDto, IShouldNormalize
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long OrderId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "OrderId";
            }
        }
    }

}
