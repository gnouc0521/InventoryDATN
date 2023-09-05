(function () {
    var _$ExportTable = $('#ExportTable');
    var _ExportDetailService = abp.services.app.exportRequestDetails;
    var _exportRequests = abp.services.app.exportRequests;
    moment.locale(abp.localization.currentLanguage.name);


    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ExportRequests/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_CreateModalReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });
    ///* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.exportRequestId = $('#Id').val()
        dataFilter.warehouseId = $('#WarehouseId').val()
        return dataFilter;
    }

    var dataTable = _$ExportTable.DataTable({
        paging: false,
        serverSide: false,
        processing: false,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        listAction: {
            ajaxFunction: _ExportDetailService.getAll,
            inputFilter: getFilter
        },
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    var stt = meta.row + 1;
                    return `<span>` + stt + `</span>`;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: 'itemsCode',
                render: function (data, type, row, meta) {
                    return `<span>` + row.itemsCode + `-` + row.itemsName + `</span>`
                }
            },
            {
                orderable: false,
                targets: 2,
                data: null,
                render: function (data, type, row, meta) {
                    return `<span>` + row.unitName +`</span>`
                }
            },
            {
                orderable: false,
                targets: 3,
                data: 'quantityExport',
            },
            {
                orderable: false,
                targets: 4,
                data: 'exportPrice',
            },
           
            
        ],
      
    })

    $("#SendTo").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.status = 1 ;
        dataFilter.id = btnClick[0].dataset.obj;
        dataFilter.email = $("#email").val();
        dataFilter.link = window.location.href;
        dataFilter.name = $("#name").val();
        _exportRequests.updateStatus(dataFilter).done(function () {
            abp.notify.info('Gửi phiếu yêu cầu xuất kho thành công!');
            btnClick.hide();
        })
    })
    $("#CreateExport").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.status = 1 ;
        dataFilter.id = btnClick[0].dataset.obj;
         abp.message.confirm(
            app.localize('Bạn có chắc chắn muốn tạo phiếu xuất kho'),
            app.localize('Tạo phiếu xuất'),
            function (isConfirmed) {
                if (isConfirmed) {
                    _exportRequests.updateExportStatus({ id: dataFilter.id, exportStatus: 1 }).done(function () {
                        abp.notify.success(app.localize('Xóa phiếu xuất kho thành công'));
                        btnClick.hide()
                    })
                }
            }
        );

    })

    $(".btn-browse").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.status = 3;
        dataFilter.id = btnClick[0].dataset.quo;
        dataFilter.link = window.location.href;
        _exportRequests.updateStatus(dataFilter).done(function () {
            abp.notify.info('Phê duyệt phiếu yêu cầu xuất thành công!');
            $(".btn-browse").prop("disabled", true);
            $(".btn-reject").prop("disabled", true);
        })
    })

    $(".btn-reject").click(function () {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quo };
        _createModalReject.open(dataFilter);
    });
    

})(jQuery);