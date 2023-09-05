using bbk.netcore.mdl.OMS.Application.ProfileWorks.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseItems.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseTypes.Dto;
using bbk.netcore.mdl.OMS.Core.Enums;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace bbk.netcore.Web.Areas.Inventorys.Models.WareHouse
{
    public class WareHouseAddressModel
    {
        public int Id { get; set; }
        public Address provinces { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string WardsId { get; set; }
        public string Number { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string TypeCode { get; set; }
        public bool DeleteFlag { get; set; }
        public decimal Length { get; set; }
        public string rolename { get; set; }
        public decimal Width { get; set; }

        public decimal Height { get; set; }
        public WarehouseEnum.TypeWarehouse TypeWarehouse { get; set; }


        public List<WarehouseTypeListDto> ListWarehouseType { get; set; }

        public List<SelectListItem> GetWarehouseType()
        {
            var list = new List<SelectListItem>();

            foreach (var item in ListWarehouseType)
            {
                var listItem = new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                };
                list.Add(listItem);
            }
            return list;
        }
    }
}
