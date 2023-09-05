using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace bbk.netcore.Authorization
{
    public class InventoryAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
           
            var warhouse = context.CreatePermission(InventoryPermission.WareHouse, L("Quản lý kho"));
            warhouse.CreateChildPermission(InventoryPermission.WareHouse_Create, L("Thêm kho"));
            warhouse.CreateChildPermission(InventoryPermission.WareHouse_Edit, L("Sửa kho"));
            warhouse.CreateChildPermission(InventoryPermission.WareHouse_Delete, L("Xóa kho"));
            warhouse.CreateChildPermission(InventoryPermission.WareHouse_View, L("Xem kho"));
            warhouse.CreateChildPermission(InventoryPermission.WareHouse_Layout, L("Chỉnh sửa Layouts"));



            var Supplier = context.CreatePermission(InventoryPermission.Supplier, L("Quản lý NCC"));
            Supplier.CreateChildPermission(InventoryPermission.Supplier_Create, L("Thêm NCC"));
            Supplier.CreateChildPermission(InventoryPermission.Supplier_Edit, L("Sửa NCC"));
            Supplier.CreateChildPermission(InventoryPermission.Supplier_Delete, L("Xóa NCC"));


            var Producer = context.CreatePermission(InventoryPermission.Producer, L("Quản lý NSX"));
            Producer.CreateChildPermission(InventoryPermission.Producer_Create, L("Thêm NSX"));
            Producer.CreateChildPermission(InventoryPermission.Producer_Edit, L("Sửa NSX"));
            Producer.CreateChildPermission(InventoryPermission.Producer_Delete, L("Xóa NSX"));



            var importRequest = context.CreatePermission(InventoryPermission.ImportRequest, L("Tạo Phiếu nhập kho"));
            importRequest.CreateChildPermission(InventoryPermission.ImportRequestEdit, L("Nhập kho"));
            importRequest.CreateChildPermission(InventoryPermission.ImportRequest_CreateTP, L("Nhập kho điều chuyển"));
            importRequest.CreateChildPermission(InventoryPermission.ImportRequest_CreateYCNK, L("Nhập kho YCNK"));

            //var importRequestUpdate = context.CreatePermission(InventoryPermission.ImportRequestEdit, L("Nhập kho"));

            var ExportRequest = context.CreatePermission(InventoryPermission.ExportRequests, L("Phiếu xuất kho"));
            ExportRequest.CreateChildPermission(InventoryPermission.ExportRequests_Create, L("Thêm Phiếu xuất kho"));
            ExportRequest.CreateChildPermission(InventoryPermission.ExportRequests_Requirement, L("Tạo yêu cầu phiếu xuất"));
            ExportRequest.CreateChildPermission(InventoryPermission.ExportRequests_Edit, L("Sửa Phiếu xuất kho"));
            ExportRequest.CreateChildPermission(InventoryPermission.ExportRequests_Delete, L("Xóa Phiếu xuất kho"));
            ExportRequest.CreateChildPermission(InventoryPermission.ExportRequests_Update, L("Cập nhật phiếu xuất"));
            ExportRequest.CreateChildPermission(InventoryPermission.ExportRequests_Approve, L("Phê duyệt phiếu xuất"));


            var inventoryTicket = context.CreatePermission(InventoryPermission.InventoryTicket, L("Phiếu kiểm kho"));
            inventoryTicket.CreateChildPermission(InventoryPermission.InventoryTicket_Create, L("Thêm Phiếu kiểm kho"));
            inventoryTicket.CreateChildPermission(InventoryPermission.InventoryTicket_Edit, L("Sửa Phiếu kiểm kho"));
            inventoryTicket.CreateChildPermission(InventoryPermission.InventoryTickett_Delete, L("Xóa Phiếu kiểm kho"));
            inventoryTicket.CreateChildPermission(InventoryPermission.InventoryTickett_View, L("Xem Phiếu kiểm kho"));

            var ArrangeItem = context.CreatePermission(InventoryPermission.ArrangeItems, L("Sắp xếp kho"));
            ArrangeItem.CreateChildPermission(InventoryPermission.ArrangeItems_Create, L("Thêm Phiếu kiểm kho"));
            ArrangeItem.CreateChildPermission(InventoryPermission.ArrangeItems_Edit, L("Sửa Phiếu kiểm kho"));
            ArrangeItem.CreateChildPermission(InventoryPermission.ArrangeItems_Delete, L("Xóa Phiếu kiểm kho"));


            var item = context.CreatePermission(InventoryPermission.Items, L("Quản lý hàng hoá"));
            item.CreateChildPermission(InventoryPermission.Items_Create, L("Thêm Hàng hoá"));
            item.CreateChildPermission(InventoryPermission.Items_Edit, L("Sửa Hàng hoá"));
            item.CreateChildPermission(InventoryPermission.Items_Delete, L("Xóa Hàng hoá"));

            var WarehouseType = context.CreatePermission(InventoryPermission.WarehouseTypes, L("Quản lý loại kho"));
            WarehouseType.CreateChildPermission(InventoryPermission.WarehouseTypes_Create, L("Thêm loại kho"));
            WarehouseType.CreateChildPermission(InventoryPermission.WarehouseTypes_Edit, L("Sửa loại kho"));
            WarehouseType.CreateChildPermission(InventoryPermission.WarehouseTypes_Delete, L("Xóa loại kho"));

            var Unit = context.CreatePermission(InventoryPermission.Units, L("Quản lý đơn vị tính"));
            Unit.CreateChildPermission(InventoryPermission.Units_Create, L("Thêm ĐVT"));
            Unit.CreateChildPermission(InventoryPermission.Units_Edit, L("Sửa ĐVT"));
            Unit.CreateChildPermission(InventoryPermission.Units_Delete, L("Xóa ĐVT"));


            var Rule = context.CreatePermission(InventoryPermission.Rules, L("Quản lý quy cách"));
            Rule.CreateChildPermission(InventoryPermission.Rules_Create, L("Thêm quy cách"));
            Rule.CreateChildPermission(InventoryPermission.Rules_Edit, L("Sửa quy cách"));
            Rule.CreateChildPermission(InventoryPermission.Rules_Delete, L("Xóa quy cách"));

            var stock = context.CreatePermission(InventoryPermission.Stock, L("Quản lý tồn kho"));

            var PurchasesRequest = context.CreatePermission(InventoryPermission.PurchasesRequest, L("Yêu cầu mua hàng"));
            PurchasesRequest.CreateChildPermission(InventoryPermission.PurchasesRequest_Create, L("Thêm "));
            PurchasesRequest.CreateChildPermission(InventoryPermission.PurchasesRequest_Edit, L("Sửa"));
            PurchasesRequest.CreateChildPermission(InventoryPermission.PurchasesRequest_Delete, L("Xóa"));
            PurchasesRequest.CreateChildPermission(InventoryPermission.PurchasesRequest_send, L("Gửi YCMH"));
            PurchasesRequest.CreateChildPermission(InventoryPermission.PurchasesRequest_Confirm, L("Duyệt YCMH"));
            PurchasesRequest.CreateChildPermission(InventoryPermission.PurchasesRequest_feedback, L("Lý do từ chối"));


            var Subsidiary = context.CreatePermission(InventoryPermission.Subsidiary, L("Đơn vị yêu cầu"));
            Subsidiary.CreateChildPermission(InventoryPermission.Subsidiary_Create, L("Thêm "));
            Subsidiary.CreateChildPermission(InventoryPermission.Subsidiary_Edit, L("Sửa"));
            Subsidiary.CreateChildPermission(InventoryPermission.Subsidiary_Delete, L("Xóa"));


            var Expert = context.CreatePermission(InventoryPermission.Expert, L("Quản lý Chuyên viên"));
            Expert.CreateChildPermission(InventoryPermission.Expert_Create, L("Thêm "));
            Expert.CreateChildPermission(InventoryPermission.Expert_Edit, L("Sửa"));
            Expert.CreateChildPermission(InventoryPermission.Expert_Delete, L("Xóa"));

            var PurchaseAssignment = context.CreatePermission(InventoryPermission.PurchaseAssignment, L("Phân công mua hàng"));
            PurchaseAssignment.CreateChildPermission(InventoryPermission.PurchaseAssignment_Assign, L("Thêm chuyên viên "));


            var MyWork = context.CreatePermission(InventoryPermission.MyWork, L("Nhiệm vụ của tôi"));


            var Transfer = context.CreatePermission(InventoryPermission.Transfer, L("Điều chuyển"));
            Transfer.CreateChildPermission(InventoryPermission.Transfer_Create, L("Tạo phiếu điều chuyển"));

            Transfer.CreateChildPermission(InventoryPermission.TransferManagement, L("Quản lý điều chuyển"));

            var Contract = context.CreatePermission(InventoryPermission.Contract, L("Hợp đồng"));
            Contract.CreateChildPermission(InventoryPermission.Contract_Create, L("Thêm "));
            Contract.CreateChildPermission(InventoryPermission.Contract_Edit, L("Phê duyệt"));

            var ComfirmOrder = context.CreatePermission(InventoryPermission.Order_comfirm, L("Xác nhận đơn hàng từ NCC"));

            var ycnk = context.CreatePermission(InventoryPermission.YCNK_aLL, L("Yêu cầu phiếu nhập"));
            ycnk.CreateChildPermission(InventoryPermission.YCNK_Create, L("Tạo mới"));
            ycnk.CreateChildPermission(InventoryPermission.YCNK, L("Xác nhận YCNK"));
            ycnk.CreateChildPermission(InventoryPermission.YCNK_send, L("Gửi Phê duyệt YCPN"));
            ycnk.CreateChildPermission(InventoryPermission.YCNK_feedback, L("feedback YCPN"));

            var Order = context.CreatePermission(InventoryPermission.Order, L("Đơn đặt hàng"));
            Order.CreateChildPermission(InventoryPermission.Order_Create, L("Tạo đơn đặt hàng"));

            var quote = context.CreatePermission(InventoryPermission.Quote, L("Trình báo giá"));
            quote.CreateChildPermission(InventoryPermission.Quote_Create, L("Thêm"));
            quote.CreateChildPermission(InventoryPermission.Quote_Edit, L("Phê duyệt báo giá"));
            quote.CreateChildPermission(InventoryPermission.Quote_Feedback, L("Phản hồi"));
            quote.CreateChildPermission(InventoryPermission.Quote_Update, L("Cập nhật trình báo giá"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, netcoreConsts.LocalizationSourceName);
        }
    }
}
