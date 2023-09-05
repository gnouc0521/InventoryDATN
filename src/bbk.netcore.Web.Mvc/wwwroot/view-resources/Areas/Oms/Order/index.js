(function () {
    var _$ContractTable = $("#ContractTable");
    var _$OrderTable = $("#OrderTable");
    var _contractService = abp.services.app.contract;
    var _orderService = abp.services.app.order;
    var _orderdetailService = abp.services.app.ordersDetail;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Order/CreateOrder',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Order/_CreateModal.js',
        modalClass: 'OrderCreateModal',
        modalType: 'modal-xl'

    });
    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModalReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });

    var _sendMailModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/MyWork/Sendmail',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/MyWork/Sendmail.js',
        modalClass: 'SendMailModal',
        modalType: 'modal-xl'
    });

    var getFilterQuoteApprove = function () {
        let dataFilter = {};
        dataFilter.status = 6;
        dataFilter.searchTerm = $('#SearchTerm').val();
        dataFilter.exportStatus = 0;
        return dataFilter;
    }



    var ContractdataTable = _$ContractTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": false,
        searching: false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,

        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _contractService.getAll,
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
                data: "code",
                render: function (data, type, row, meta) {
                    return `<div class=''> 
                                <a class='doceditfunc' data-objid='` + row.id + `'href='javascript:void(0); ' > ` + row.code + ` </a>
                            </div>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "supplierName",

            },
            {
                targets: 3,
                orderable: false,
                autoWidth: false,
                data: 'creationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {

                targets: 4,
                orderable: false,
                autoWidth: false,
                data: 'lastModificationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {

                orderable: false,
                targets: 5,
                data: null,
                className: "text-center",
                render: function (data, type, row, meta) {

                    return `
                            <button class="btn btn-primary btn-create" data-objid="`+ row.id + `">Tạo đơn đặt hàng</button>
                           `;
                },
            }

        ]
    });


    $('#ContractTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick.attr("data-objid") };



        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Contract/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Contract/Print?Id=" + results.id;
            },
        });
    });

    abp.event.on('app.reloadgetDocContact', function () {
        getDocContact();
    });

    function getDocContact() {
        ContractdataTable.ajax.reload();
        OrderdataTable.ajax.reload();
    }

    $('#Search').click(function (e) {
        e.preventDefault();
        getDocContact();
    });
    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });

    $('#SearchTerm').bind("enterKey", function (e) {
        getDocContact()
    });
    $('#SearchTerm').keyup(function (e) {
        if (e.keyCode == 13) {
            $(this).trigger("enterKey");
        }
    });

    $('#ContractTable').on('click', '.btn-create', function (e) {
        var btnClick = $(this);
        var dataFilter = { Id: btnClick[0].dataset.objid };
        _createModal.open(dataFilter);
    });



    ////---------------------------------------------------  / Bảng Danh Sách đơn đặt hàng ------------------------


    var OrderdataTable = _$OrderTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": false,
        searching: false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,

        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _orderService.getAll,
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
                data: "orderCode",
                render: function (data, type, row, meta) {
                    return `<div class=''> 
                                <a class='doceditfunc' data-objid='` + row.id + `'href='javascript:void(0); ' > ` + row.orderCode + ` </a>
                            </div>`;
                }
            }
            , {
                orderable: false,
                targets: 2,
                data: "contractCode",

            },
            {
                orderable: false,
                targets: 3,
                data: "supplierName",

            },
            {
                targets: 4,
                orderable: false,
                autoWidth: false,
                data: 'creationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {

                targets: 5,
                data: 'orderStatus',
                orderable: false,
                autoWidth: false,
                render: function (status) {
                    if (status == 1) {
                        return `<span class="span_status span-defaut"> Chờ xác nhận </span>`
                    }
                    else
                    {
                        return `<span class="span_status span-approve"> Đã xác nhân từ NCC </span>`
                    }

                },
            },
            {

                targets: 6,
                data: 'orderStatus',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item docexportfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-print"></i> Xuất file excel </a>
                            		<a class='dropdown-item docview'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-eye"></i> Chi tiết </a>
                            	</div>
                            </div>`;

                },
            },


        ]
    });

    $('#OrderTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Order/OrderDetail?id=" + results.id;
            },
        });
    });

    $('#OrderTable').on('click', '.docexportfunc', function (e) {
        var btnClick = $(this);
        exportReport( btnClick[0].dataset.objid)
        
    });


    function exportReport(id) {
        var filterObj = {
            id: id
        };
        _orderdetailService.getPOListDto(filterObj)
            .done(function (fileResult) {
                location.href = abp.appPath + 'File/DownloadReportFile?fileType=' + fileResult.fileType + '&fileToken=' + fileResult.fileToken + '&fileName=' + fileResult.fileName;
            });
    }

    $('#OrderTable').on('click', '.docview', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Order/OrderDetail?id=" + results.id;
            },
        });
    });



})(jQuery);