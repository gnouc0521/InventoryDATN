using Abp.Domain.Entities.Auditing;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises.Dto;
using bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace bbk.netcore.Web.Areas.Inventorys.Models.PurchsesRequest
{
    public class PurchsesRequestViewModel :FullAuditedEntity
    {
        public string SubsidiaryCompany { get; set; }

        public long SubsidiaryCompanyId { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public DateTime RequestDate { get; set; }

        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardsId { get; set; }

        public PurchasesRequestListDto purchasesRequest { get; set; }
        public SubsidiaryListDto subsidiaryList { get; set; }
        public PurchasesSynthesisListDto purchasesSynthesis { get; set; }
        public PurchasesRequestEnum.MyworkStatus RequestStatus { get; set; }

        public List<SubsidiaryListDto> subsidiaryListDto { get; set; }

        public List<SelectListItem> Getsubsidiary()
        {
            var list = new List<SelectListItem>();
            foreach (var item1 in subsidiaryListDto)
            {
                var listItems1 = new SelectListItem
                {
                    Text = item1.NameCompany,
                    Value = item1.Id.ToString(),
                };
                list.Add(listItems1);
            }
            return list;
        }
    }
}
