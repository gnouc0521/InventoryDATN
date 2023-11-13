using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using bbk.netcore.Authorization;
using bbk.netcore.Web.Areas.App.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.DocumentEnum;

namespace bbk.netcore.mdl.PersonalProfile.Web.Navigation
{
  public class InventorysNavigationProvider : NavigationProvider
  {
    public const string MenuName = "PersonalProfileStaffsMenu";
    //public const string MenuName = "App";
    public override void SetNavigation(INavigationProviderContext context)
    {
      var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Quản lý kho"));

      menu
          .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileDashboard,
                   new FixedLocalizableString("Trang chủ"),
                  url: "/Inventorys/Dashboard",
                  icon: "fal fa-info-circle",
                  requiresAuthentication: true
              )
          )
          //// Phân hệ quản lý kho
          .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileStatistics,
                   new FixedLocalizableString("Quản Lý Chung"),
                  //url: "/PersonalProfile/Work",
                  icon: "fal fa-tasks",
                  requiresAuthentication: true
              )
              .AddItem(
                  new MenuItemDefinition(
                      InventorysPageNames.InventoryStaff.PersonalProfileDocuments,
                      new FixedLocalizableString("QL Loại Kho"),
                      url: "/Inventorys/WarehouseTypes",
                      icon: "fal fa-credit-card-front",
                      requiresAuthentication: true,
                      permissionDependency: new SimplePermissionDependency(InventoryPermission.WarehouseTypes)
                  ))
               .AddItem(
                  new MenuItemDefinition(
                      InventorysPageNames.InventoryStaff.PersonalProfileDocuments,
                      new FixedLocalizableString("QL Quy Cách"),
                      url: "/Inventorys/Rules",
                      icon: "fal fa-credit-card-front",
                      requiresAuthentication: true,
                      permissionDependency: new SimplePermissionDependency(InventoryPermission.Rules)
                  ))
              .AddItem(
                  new MenuItemDefinition(
                      InventorysPageNames.InventoryStaff.PersonalProfileDocuments,
                      new FixedLocalizableString("QL ĐVT"),
                      url: "/Inventorys/Units",
                      icon: "fal fa-credit-card-front",
                      requiresAuthentication: true,
                      permissionDependency: new SimplePermissionDependency(InventoryPermission.Units)
                  )))
            //////////////////////////////////////////////////////////////////////////////////////////////
            .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileStatistics,
                   new FixedLocalizableString("Quản Lý Kho"),
                  //url: "/PersonalProfile/Work",
                  icon: "fal fa-tasks",
                  requiresAuthentication: true
              )
               .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileReports,
                   new FixedLocalizableString("Kho"),
                  url: "/Inventorys/WareHouse",
                  icon: "fal fa-code",
                  requiresAuthentication: true,
                  permissionDependency: new SimplePermissionDependency(InventoryPermission.WareHouse)
              ))
                .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileReports,
                   new FixedLocalizableString("Phiếu nhập kho"),
                  url: "/Inventorys/ImportRequest/ImportRequest",
                  icon: "fal fa-book",
                  requiresAuthentication: true,
                  permissionDependency: new SimplePermissionDependency(InventoryPermission.ImportRequest_CreateYCNK)
              ))
               .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileReports,
                   new FixedLocalizableString("Phiếu xuất kho"),
                  url: "/Inventorys/ExportRequests",
                  icon: "fal fa-paper-plane",
                  requiresAuthentication: true,
                  permissionDependency: new SimplePermissionDependency(InventoryPermission.ExportRequests_Create)
                   ))

                   .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileReports,
                   new FixedLocalizableString("Phiếu kiểm kho"),
                  url: "/Inventorys/InventoryTicket",
                  icon: "fal fa-ticket",
                  requiresAuthentication: true,
                  permissionDependency: new SimplePermissionDependency(InventoryPermission.InventoryTicket)
              ))
                     .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileReports,
                   new FixedLocalizableString("Sắp xếp kho"),
                  url: "/Inventorys/ArrangeItems",
                  icon: "fal fa-sort",
                  requiresAuthentication: true,
                  permissionDependency: new SimplePermissionDependency(InventoryPermission.ArrangeItems)
              ))
           .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileReports,
                   new FixedLocalizableString("Nhà cung cấp"),
                  url: "/Inventorys/Supplier",
                  icon: "fal fa-calendar",
                  requiresAuthentication: true,
                  permissionDependency: new SimplePermissionDependency(InventoryPermission.Supplier)
              ))
             .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileReports,
                   new FixedLocalizableString("Nhà sản xuất"),
                  url: "/Inventorys/Producer",
                  icon: "fal fa-user",
                  requiresAuthentication: true,
                  permissionDependency: new SimplePermissionDependency(InventoryPermission.Producer)
              ))
             .AddItem(
              new MenuItemDefinition(
                  InventorysPageNames.InventoryStaff.PersonalProfileReports,
                   new FixedLocalizableString("Hàng hóa"),
                  url: "/Inventorys/Items/AllItems",
                  icon: "fal fa-inventory",
                  requiresAuthentication: true,
                  permissionDependency: new SimplePermissionDependency(InventoryPermission.Items)
              )));
    }

    private static ILocalizableString L(string name)
    {
      return new LocalizableString(name, bbk.netcore.netcoreConsts.LocalizationSourceName);
    }
  }
}
