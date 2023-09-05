namespace bbk.netcore.Authorization
{
    public static class InventoryPermission
    {
        #region Inventory Pages Name

        public const string WareHouse = "Inventorys.WareHouse";
        public const string WareHouse_Create = "Inventorys.WareHouse.Create";
        public const string WareHouse_Edit = "Inventorys.WareHouse.Edit";
        public const string WareHouse_Delete = "Inventorys.WareHouse.Delete";
        public const string WareHouse_View = "Inventorys.WareHouse.View";
        public const string WareHouse_Layout = "Inventorys.WareHouse.Layout";
        


        public const string Supplier = "Inventorys.Supplier";
        public const string Supplier_Create = "Inventorys.Supplier.Create";
        public const string Supplier_Edit = "Inventorys.Supplier.Edit";
        public const string Supplier_Delete = "Inventorys.Supplier.Delete";

        public const string Producer = "Inventorys.Producer";
        public const string Producer_Create = "Inventorys.Producer.Create";
        public const string Producer_Edit = "Inventorys.Producer.Edit";
        public const string Producer_Delete = "Inventorys.Producer.Delete";

        public const string ImportRequest = "Inventorys.ImportRequest";
        public const string ImportRequest_Create = "Inventorys.ImportRequest.Create";
        public const string ImportRequest_CreateTP = "Inventorys.ImportRequest.CreateTP";
        public const string ImportRequest_CreateYCNK = "Inventorys.ImportRequest.CreateYCNK";
        public const string ImportRequest_Edit = "Inventorys.ImportRequest.Edit";
        public const string ImportRequest_Delete = "Inventorys.ImportRequest.Delete";
        public const string ImportRequestEdit = "Inventorys.ImportRequest.EditIMP";

        public const string ExportRequests = "Inventorys.ExportRequests";
        public const string ExportRequests_Create = "Inventorys.ExportRequests.Create";
        public const string ExportRequests_Requirement = "Inventorys.ExportRequests.Requirement";
        public const string ExportRequests_Edit = "Inventorys.ExportRequests.Edit";
        public const string ExportRequests_Delete = "Inventorys.ExportRequests.Delete";
        public const string ExportRequests_Update = "Inventorys.ExportRequests.Update";
        public const string ExportRequests_Approve = "Inventorys.ExportRequests.Approve";


        public const string InventoryTicket = "Inventorys.InventoryTickets";
        public const string InventoryTicket_Create = "Inventorys.InventoryTickets.Create";
        public const string InventoryTicket_Edit = "Inventorys.InventoryTickets.Edit";
        public const string InventoryTickett_Delete = "Inventorys.InventoryTickets.Delete";
        public const string InventoryTickett_View = "Inventorys.InventoryTickets.View";


        public const string ArrangeItems = "Inventorys.ArrangeItems";
        public const string ArrangeItems_Create = "Inventorys.ArrangeItems.Create";
        public const string ArrangeItems_Edit = "Inventorys.ArrangeItems.Edit";
        public const string ArrangeItems_Delete = "Inventorys.ArrangeItems.Delete";


        public const string Items = "Inventorys.Items";
        public const string Items_Create = "Inventorys.Items.Create";
        public const string Items_Edit = "Inventorys.Items.Edit";
        public const string Items_Delete = "Inventorys.Items.Delete";


        public const string WarehouseTypes = "Inventorys.WarehouseTypes";
        public const string WarehouseTypes_Create = "Inventorys.WarehouseTypes.Create";
        public const string WarehouseTypes_Edit = "Inventorys.WarehouseTypes.Edit";
        public const string WarehouseTypes_Delete = "Inventorys.WarehouseTypes.Delete";


        public const string Units = "Inventorys.Units";
        public const string Units_Create = "Inventorys.Units.Create";
        public const string Units_Edit = "Inventorys.Units.Edit";
        public const string Units_Delete = "Inventorys.Units.Delete";


        public const string Rules = "Inventorys.Rules";
        public const string Rules_Create = "Inventorys.Rules.Create";
        public const string Rules_Edit = "Inventorys.Rules.Edit";
        public const string Rules_Delete = "Inventorys.Rules.Delete";

        public const string Stock = "Inventorys.Stocks";


        public const string PurchasesRequest = "Inventorys.PurchasesRequest";
        public const string PurchasesRequest_Create = "Inventorys.PurchasesRequest.Create";
        public const string PurchasesRequest_Edit = "Inventorys.PurchasesRequest.Edit";
        public const string PurchasesRequest_Delete = "Inventorys.PurchasesRequest.Delete";
        public const string PurchasesRequest_Confirm = "Inventorys.PurchasesRequest.Confirm";
        public const string PurchasesRequest_send = "Inventorys.PurchasesRequest.send";
        public const string PurchasesRequest_feedback = "Inventorys.PurchasesRequest.feedback";

        public const string Subsidiary = "Inventorys.Subsidiary";
        public const string Subsidiary_Create = "Inventorys.Subsidiary.Create";
        public const string Subsidiary_Edit = "Inventorys.Subsidiary.Edit";
        public const string Subsidiary_Delete = "Inventorys.Subsidiary.Delete";

        public const string PurchaseAssignment = "Inventorys.PurchaseAssignment";
        public const string PurchaseAssignment_Assign = "Inventorys.PurchaseAssignment.Create";
       

        public const string Expert = "Inventorys.Expert";
        public const string Expert_Create = "Inventorys.Expert.Create";
        public const string Expert_Edit = "Inventorys.Expert.Edit";
        public const string Expert_Delete = "Inventorys.Expert.Delete";

        public const string MyWork = "Inventorys.MyWork";

        //public const string Compare = "Inventorys.Compare";

        public const string Contract = "Inventorys.Contract";
        public const string Contract_Create = "Inventorys.Contract.Create";
        public const string Contract_Edit = "Inventorys.Contract.Edit";
        //public const string Contract_Feedback = "Inventorys.Contract.Feedback";

        public const string Transfer = "Inventorys.Transfer";
        public const string Transfer_Create = "Inventorys.Transfer.Create";
        public const string TransferManagement = "Inventorys.TransferManagement";

        public const string Quote = "Inventorys.Quote";
        public const string Quote_Create = "Inventorys.Quote.Create";
        public const string Quote_Edit = "Inventorys.Quote.Edit";
        public const string Quote_Feedback = "Inventorys.Quote.Feedback";
        public const string Quote_Update = "Inventorys.Quote.Update";


        public const string Order = "Inventorys.Order";
        public const string Order_Create= "Inventorys.Order.Create";
        public const string Order_comfirm = "Inventorys.Order.Confirm";


        public const string YCNK_aLL = "Inventorys.YCNK";
        public const string YCNK_Create = "Inventorys.YCNK.Create";
        public const string YCNK = "Inventorys.YCNK.Confirm";
        public const string YCNK_send = "Inventorys.YCNK.send";
        public const string YCNK_feedback = "Inventorys.YCNK.feedback";
        #endregion
    }
}
