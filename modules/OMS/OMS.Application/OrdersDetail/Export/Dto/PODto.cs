using bbk.netcore.mdl.OMS.Application.OrdersDetail.Dto;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.OrdersDetail.Export.Dto
{
    public class PODto
    {
        public string OrderCode { get; set; }   
        public long OrderId { get; set; }
        public string SubsidiaryName { get; set; }  

        public List<OrdersDetailListDto> ordersDetailListDtos { get; set; } 
    }
    
}
