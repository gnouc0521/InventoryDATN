(function () {
  //  var _$DoneStaffTable = $('#DoneStaffTable');
    var _$StaffTable = $('#StaffTable');
    var _PurchasesSyn = abp.services.app.purchasesSynthesise;
    var _purchaseAssignment = abp.services.app.purchaseAssignmentService;
    moment.locale(abp.localization.currentLanguage.name);

    var getFilter = function () {
        let dataFilter = {};
        dataFilter.id = $('#SynthesiseId').val().trim();
        return dataFilter;
    }

    var dataTable = _$StaffTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "zeroRecords": "Không tìm thấy dữ liệu",
            searchPlaceholder: "Tìm kiếm"
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
            ajaxFunction: _PurchasesSyn.getItemByStaff,
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
                data: 'purchasesSynthesiseCode',
                render: function (data, type, row, meta) {
                    return `
                        <a class="purchasesSynthesiseCode" data-objid='` + row.purchasesSynthesiseId + `' href='javascript:void(0); '>` + row.purchasesSynthesiseCode + `</a>
                    `
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "getPriceStatus",
                className: "align-middle text-center",
                render: function (data, type, row, meta) {
                    if (row.getPriceStatus == 2) {
                        return `<span class="span_status span-defaut"> Mới </span>`
                    } else if (row.getPriceStatus == 0) {
                        debugger
                        if (row.creatorUserId != abp.session.userId) {
                            return `<span class="span_status span-approve"> Hoàn thành </span>`
                        }
                        else {
                            return `<span class="span_status span-approve"> NCC Xác nhận </span>`
                        }

                    } else if (row.getPriceStatus == 3) {
                        return `<span class="span_status span-reject"> Từ chối </span>`
                    }
                }
            },
        ],
    });


    $('#StaffTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        debugger
        _purchaseAssignment.changGetPriceStatus(dataFilter)
            .done(function () {
                abp.notify.info('Cập nhật thành công!');
                getDocs();
               
            })
    });

    $('#StaffTable').on('click', '.purchasesSynthesiseCode', function (e) {
        var btnClick = $(this);
        var dataFilter = { purchasesSynthesiseId: btnClick[0].dataset.objid };
        debugger
        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.purchasesSynthesiseId,
            success: function (results) {
                debugger
                window.location.href =
                    "/Inventorys/MyWork/Update?id=" + results.id;
            },
        });
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

 
    function getDocs() {
        dataTable.ajax.reload();

    }

   

})(jQuery);
