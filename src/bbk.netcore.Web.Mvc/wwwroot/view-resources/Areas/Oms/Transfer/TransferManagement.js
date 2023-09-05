(function () {
    var _$ExportTable = $('#ExportTable');
    var _$TransferTable = $("#TransferTable");
    var _exportRequests = abp.services.app.exportRequests;
    var _transferService = abp.services.app.transfer;

    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Transfer/CreateTransfer',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Transfer/_CreateModal.js',
        modalClass: 'TransferCreateModal',
        modalType: 'modal-xl'

    });

 

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    //--------------------------- Code Cuong -------------------------

    /* DEFINE TABLE */
    //var getFilter = function () {
    //    let dataFilter = {};
    //    dataFilter.status = 0;
    //    return dataFilter;
    //}


    //var dataTable = _$ExportTable.DataTable({
    //    paging: true,
    //    serverSide: false,
    //    processing: true,
    //    "searching": false,
    //    "language": {
    //        "emptyTable": "Không tìm thấy dữ liệu",
    //        "lengthMenu": "Hiển thị _MENU_ bản ghi",
    //    },
    //    "bInfo": false,
    //    "bLengthChange": true,
    //    //"dom": 'Rltip',
    //    lengthMenu: [
    //        [5, 10, 25, 50, -1],
    //        [5, 10, 25, 50, 'Tất cả'],
    //    ],

    //    pageLength: 10,
    //    listAction: {
    //        ajaxFunction: _exportRequests.getAll,
    //        inputFilter: getFilter
    //    },
    //    order: [[1, 'asc']],
    //    columnDefs: [

    //        {
    //            targets: 0,
    //            orderable: false,
    //            className: 'dt-body-center text-center',
    //            render: function (data, type, row, meta) {
    //                return meta.row + 1;
    //            }
    //        },
    //        {
    //            orderable: false,
    //            targets: 1,
    //            data: "code"
    //        },
    //        {
    //            orderable: false,
    //            targets: 2,
    //            data: "warehouseDestinationName",
    //        },
    //        {
    //            orderable: false,
    //            targets: 3,
    //            data: "creationTime",
    //            render: function (data, type, row, meta) {
    //                var html = "";
    //                for (var i = 0; i < row.listWarehouseSourceName.length; i++) {
    //                    if (row.listWarehouseSourceName[i] == null) {
    //                        row.listWarehouseSourceName[i] =""
    //                    }
    //                    html += `<span>` + row.listWarehouseSourceName[i] + `</span><br>`
    //                }
    //                return html;
    //            }
    //        },
    //        {
    //            orderable: false,
    //            targets: 4,
    //            data: "createBy",
    //        },
    //        {
    //            orderable: false,
    //            targets: 5,
    //            data: "creationTime",
    //            render: function (creationTime) {
    //                return moment(creationTime).format('L');
    //            }
              
    //        },
    //        {
    //            orderable: false,
    //            targets: 6,
    //            data: null,
    //            render: function (data, type, row, meta) {
    //                return `                        
    //                        <div class='dropdown d-inline-block'>
    //                        	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
    //                        		<i class="fal fa-ellipsis-v"></i>
    //                        	</a>
    //                        	<div class='dropdown-menu'>
    //                                <a class='dropdown-item doctransfer'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-check"></i> Duyệt </a>
    //                                <a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-edit"></i> Từ chối </a>
    //                        	</div>
    //                        </div>`;
    //            }
    //        },
    //    ]
    //});

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    //$('#ExportTable').on('click', '.doctransfer', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.objid, status: 1 };
    //    _exportRequests.updateStatus(dataFilter).done(function () {
    //        abp.notify.info('Phê duyệt phiếu xuất thành công!');
    //        getDocs();
    //    })
    //});


    //$('#ExportTable').on('click', '.doceditfunc', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.objid, status: 2 };
    //    _exportRequests.updateStatus(dataFilter).done(function () {
    //        abp.notify.info('Từ chối phiếu xuất thành công!');
    //        getDocs();
    //    })
    //});



    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {

        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });

    // --------------------------------------------/ Code cuong -----------------------------

     var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = "";
        return dataFilter;
    }

    var dataTable = _$TransferTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
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
        listAction: {
            ajaxFunction: _transferService.getAllApprove,
            inputFilter: getFilter
        },
        order: [[1, 'asc']],
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "transferCode"
            },
            {
                orderable: false,
                targets: 2,
                /*data: "listNameWareHouseReceiving",*/
                render: function (data, type, row, meta) {
                    var html = "";
                    for (var i = 0; i < row.listNameWareHouseReceiving.length; i++) {
                        html += `<span>` + row.listNameWareHouseReceiving[i] + `</span><br>`
                    }
                    return html;
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "nameWareHouseExport",
            },
            {
                orderable: false,
                targets: 4,
                data: "creationTime",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }

            },
            {
                orderable: false,
                targets: 5,
                /*data: "browsingTime",*/
                render: function (data, type, row, meta) {
                    if (row.status == 2) { return '' } else {
                        return moment(row.browsingTime).format('L');
                    }
                }

            },
            {
                orderable: false,
                targets: 6,
                data: "status",
                render: function (status) {
                    if (status == 2) {
                        return `<span class="span_status span-defaut"> Chờ xử lý </span>`
                    } else if (status == 1) {
                        return `<span class="span_status span-reject"> Từ chối </span>`
                    } else if (status == 3) {
                        return `<span class="span_status span-approve"> Phê duyệt </span>`
                    }
                }

            },
            {
                orderable: false,
                targets: 7,
                data: null,
                render: function (data, type, row, meta) {
                    return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                                    <a class='dropdown-item viewDetail'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-check"></i> Xem chi tiết </a>
                            	</div>
                            </div>`;
                }
            },
        ]
    });

    $('#TransferTable').on('click', '.viewDetail', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Transfer/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Transfer/ViewDetails?id=" + results.id;
            },
        });

    })


})(jQuery);