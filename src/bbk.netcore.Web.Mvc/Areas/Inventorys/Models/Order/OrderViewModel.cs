using bbk.netcore.mdl.OMS.Application.Contracts.Dto;
using bbk.netcore.mdl.OMS.Application.Orders.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.Order
{
    public class OrderViewModel
    {

       public ContractListDto Contract { get; set; }

        public SupplierListDto Supplier { get; set; }
        public OrderListDto orderListDto { get; set; }

    }
}
