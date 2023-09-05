(function () {
    var _warehouseLocationService = abp.services.app.warehouseLocationItemService;
    var _assignmentService = abp.services.app.assignments;
    var _$wareHouseTable = $("#ExpertTable");
    moment.locale(abp.localization.currentLanguage.name);

    _assignmentService.getAllItemByUserId($("#UserId").val()).done(function (result) {

        var dataTable = _$wareHouseTable.DataTable({
            paging: true,
            serverSide: false,
            data: result.items,
            processing: false,
            "searching": false,
            "language": {
                "emptyTable": "Không tìm thấy dữ liệu",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
            },
            "bInfo": false,
            "bLengthChange": true,
            //"dom": 'Rltip',
            lengthMenu: [
                [5, 10, 25, 50, -1],
                [5, 10, 25, 50, 'Tất cả'],
            ],

            pageLength: 10,
            //listAction: {
            //    ajaxFunction: _assignmentService.getAllItemByUserId,
            //    inputFilter: getFilter
            //},
            order: [[1, 'asc']],
            columnDefs: [
                {
                    orderable: false,
                    targets: 0,
                    data: "name"
                },

                {
                    targets: 1,
                    orderable: false,
                    data: "supplierName",
                    render: data => `<span>${data != "" ? data : 'không có'}</span>`
                },

            ],
        });
    });

    


})(jQuery);