(function () {
    var _$StaffTable = $('#StaffTableItem');
    var _PurchasesSyn = abp.services.app.purchasesSynthesise;
    moment.locale(abp.localization.currentLanguage.name);
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.id = $('#SynthesiseId').val().trim();
        return dataFilter;
    }

    var dataTable = _$StaffTable.DataTable({
        paging: true,
        serverSide: false,
        processing: false,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": false,
        //"dom": 'Rltip',
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 5,
        listAction: {
            ajaxFunction: _PurchasesSyn.getAllItemByExpert,
            inputFilter: getFilter
        },

        columnDefs: [
            {
                orderable: false,
                targets: 0,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: 'nameStaff'
            },
            {
                orderable: false,
                targets: 2,
                data: 'itemcode'
            },
            {
                orderable: false,
                targets: 3,
                data: "itemsName"
            },
            {
                orderable: false,
                targets: 4,
                data: "supplierName"
            },
            {
                orderable: false,
                targets: 5,
                data: "unitName"
            },
            {
                orderable: false,
                targets: 6,
                data: "quantityItems"
            },
            {
                orderable: false,
                targets: 7,
                data: "dateTimeNeed",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 8,
                data: "note",
            },
        ],
    });


})(jQuery);
