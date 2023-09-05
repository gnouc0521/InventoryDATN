(function () {
    var _$StaffTable = $('#StaffTable');
    var _PurchasesSyn = abp.services.app.purchasesSynthesise;
    var _puS = abp.services.app.purchaseAssignmentService;
    var _useworkcount = abp.services.app.userWorkCountService;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/MyWork/CreateExport',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/MyWork/_CreateModal.js',
        modalClass: 'ExportCreateModal',
        modalType: 'modal-xl'

    });


    var getFilterQuoteApprove = function () {
        let dataFilter = {};
        dataFilter.id = $('#SynthesiseId').val();
        return dataFilter;
    }

    var ContractdataTable = _$StaffTable.DataTable({
        paging: false,
        serverSide: false,
        processing: true,
        "searching": false,
        searching: false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": false,

        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _PurchasesSyn.getViewCV,
            inputFilter: getFilterQuoteApprove
        },
        order: [[0, 'asc']],
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
                data: 'itemcode'
            },
            {
                orderable: false,
                targets: 2,
                data: "itemsName"
            },
            {
                orderable: false,
                targets: 3,
                data: "supplier",
                render: function (data, type, row, meta) {
                    var html = "";
                    for (var i = 0; i < row.supplier.length; i++) {
                        html += `<span class="mb-1"> ` + row.supplier[i] + `</span><br>`;
                    }

                    return html;
                },
            },
            {
                orderable: false,
                targets: 4,
                data: "unitName"
            },
            {
                orderable: false,
                targets: 5,
                data: "quantityItems",
                render: $.fn.dataTable.render.number(',', ',', '')
            },
            {
                orderable: false,
                targets: 6,
                data: "price",
                //render: $.fn.dataTable.render.number(',', ',', '')
                render: function (data, type, row, meta) {
                    if (row.price == null || row.price == 0 || row.getPriceStatus == 0) {
                        $("#btnHoanThanh").prop("hidden", true);
                    }

                    $.fn.dataTable.render.number(',', ',', '')
                    return row.price;
                }
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

    abp.event.on('app.reloadgetDocContact', function () {
        getDocContact();
    });

    function getDocContact() {
        ContractdataTable.ajax.reload();
    }

    $("#btnHoanThanh").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
        abp.message.confirm(
            app.localize('Xác nhận'),
            app.localize('Bạn có chắc không'),
            function (isConfirmed) {
                if (isConfirmed) {
                    dataFilter.purchasesSynthesiseId = btnClick[0].dataset.action;
                    _puS.updateUserId(dataFilter).done(function () {


                    });
                    abp.notify.success(app.localize('Hoàn thành nhiệm vụ thành công'));
                    getDocContact();
                    debugger
                    dataFilter.purchasesSynthesisesId = btnClick[0].dataset.action;
                    dataFilter.workStatus = 0;
                    _useworkcount.updateSys(dataFilter);
                    $("#btnHoanThanh").prop("hidden", true);

                }
            }
        );
      
    })



    //$('#OrderTable').on('click', '.docexportfunc', function (e) {
    //    var btnClick = $(this);
    //    exportReport(btnClick[0].dataset.objid)

    //});

    $("#export_file").on("click", function (e) {
        var btnClick = $(this);
        _createModal.open({ id: btnClick[0].dataset.objid });
    })


    function exportReport(id) {
        var filterObj = {
            id: id
        };
        _orderdetailService.getPOListDto(filterObj)
            .done(function (fileResult) {
                location.href = abp.appPath + 'File/DownloadReportFile?fileType=' + fileResult.fileType + '&fileToken=' + fileResult.fileToken + '&fileName=' + fileResult.fileName;
            });
    }


})(jQuery);