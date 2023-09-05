﻿
(function () {
    var _$quoteSynTable = $("#QuoteSynTable");
    var _$quoteSynTableApprove = $("#QuoteSynTableApprove"); 
    var _$ContractTable= $("#ContractTable");

    var _quotesService = abp.services.app.quotesService;
    var _contractService = abp.services.app.contract;
    var _quoteSynService = abp.services.app.quotesSynthesise;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/CreateContract',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModal.js',
        modalClass: 'ContractsCreateModal',
        modalType: 'modal-xl'

    });
    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModalReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });

    var _createModalContractReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModalContractReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });

    ////--------------------------------------------------- Bảng Danh Sách trình Báo Giá ĐƯỢC DUYỆT ------------------------
    var getFilterQuoteApprove = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val().trim();
        return dataFilter;
    }

    var ApprovedataTable = _$quoteSynTableApprove.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        searching: true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "zeroRecords": "Không tìm thấy dữ liệu",
            searchPlaceholder: "Tìm kiếm"
        },
        "bInfo": false,
        "bLengthChange": true,

        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _quoteSynService.getAllQuoteApprove,
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                orderable: false,
                targets: 0,
                width: "5%",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "code",
                width: "10%",
                render: function (data, type, row, meta) {
                    return `<div class=''> 
                                <a class='doceditfunc' data-objid='` + row.id + `'href='javascript:void(0); ' > ` + row.code + ` </a>
                            </div>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: null,
                width: "20%",
                render: function (data, type, row, meta) {
                    var html = "";
                    for (var i = 0; i < row.suppliersNames.length; i++) {
                        html += `<span>` + row.suppliersNames[i] + `</span><br>`
                    }
                    return html;
                }
            },
            {

                targets: 3,
                width: "10%",
                orderable: false,
                autoWidth: false,
                data: 'creationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                targets: 4,
                width: "10%",
                orderable: false,
                autoWidth: false,
                data: 'quoteSynthesiseDate',
                render: function (quoteSynthesiseDate) {
                    return moment(quoteSynthesiseDate).format('L');
                }
            },
            {

                targets: 5,
                width: "20%",
                data: 'status',
                orderable: false,
                className: "align-middle text-center",
                autoWidth: false,
                render: data => `<span class="span_status span-approve"> Đã phê duyệt </span>`
            },
            {

                orderable: false,
                targets: 6,
                width: "15%",
                data: null,
                className : "text-center",
                render: function (data, type, row, meta) {

                    return `
                            <button class="btn btn-primary btn-create" data-objid="`+ row.id +`">Tạo hợp đồng</button>
                           `;
                },
            }
        ]
    });

    abp.event.on('app.reloadgetDocApprove', function () {
        getDocApprove();
    });

    function getDocApprove() {
        ApprovedataTable.ajax.reload();
    }

    $('#QuoteSynTableApprove').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Quote/CompareDetail?SynthesiseId=" + results.id;
            },
        });
    });

    $('#QuoteSynTableApprove').on('click', '.btn-create', function (e) {
        var btnClick = $(this);
        var dataFilter = { Id: btnClick[0].dataset.objid };
        _createModal.open(dataFilter);
    });

    ////---------------------------------------------------  / Bảng Danh Sách trình Báo Giá ĐƯỢC DUYỆT ------------------------


    ////---------------------------------------------------  Bảng Danh Sách Hợp đồng ---------------------------------------
    //var getFilter = function () {
    //    let dataFilter = {};
    //    dataFilter.status = ""; 
    //    return dataFilter;
    //}

    var ContractdataTable = _$ContractTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        searching: true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "zeroRecords": "Không tìm thấy dữ liệu",
            searchPlaceholder: "Tìm kiếm"
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
            /*inputFilter: getFilter*/
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                orderable: false,
                targets: 0,
                width: "5%",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                width: "10%",
                data: "code",
                render: function (data, type, row, meta) {
                    return `<div class=''> 
                                <a class='doceditfunc' data-objid='` + row.id + `' data-view="` + row.id + `" href='javascript:void(0); ' > ` + row.code + ` </a>
                            </div>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                width: "15%",
                data: "quoteSynCode",

            },
            {
                orderable: false,
                targets: 3,
                width: "15%",
                data: "supplierName",

            },
            {
                targets: 4,
                width: "15%",
                orderable: false,
                autoWidth: false,
                data: 'creationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 5,
                width: "15%",
                className: "text-center",
                render: function (data, type, row, meta) {
                    
                    return `<span class="total" data-contact="` + row.id + `" data-sup="` + row.supplierId + `" data-quoSyn="` + row.quoteSynId + `"></span>
                            <span class="viewtotal`+row.id+`"></span>`;
                }
            },
            {

                targets: 6,
                data: 'status',
                width: "20%",
                orderable: false,
                autoWidth: false,
                className: "align-middle text-center",
                render: function (status) {
                    if (status == 0) {
                        return `<span class="span_status span-defaut"> Bản nháp </span>`
                    } else if (status == 1) {
                        return `<span class="span_status span-defaut"> Chờ xử lý </span>`
                    }
                    else if(status == 2) {
                        return `<span class="span_status span-subcontract"> Chờ Phê duyệt </span>`
                    }
                    else if (status == 3 || status == 4) {
                        return `<span class="span_status span-reject"> Từ chối</span>`
                    }
                    else if (status == 5) {
                        return `<span class="span_status span-approve"> Đã duyệt </span>`
                    }
                    else if (status == 6) {
                        return `<span class="span_status span-approve"> Đã kí hợp đồng </span>`
                    }
                }
            },
            {

                orderable: false,
                targets: 7,
                data: null,
                width: "5%",
                render: function (data, type, row, meta) {
                    if (row.status == 0 || row.status == 6 || row.status == 2 || row.status == 1 || row.status == 5) {
                        return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item view-detail' data-view='` + row.id + `' href='javascript:void(0);'>` + app.localize('Xem chi tiết') + `</a>
                            	</div>
                            </div>`;
                    } if (row.status == 3 || row.status == 4) {
                        return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item view-detail' data-view='` + row.id + `' href='javascript:void(0);'>` + app.localize('Xem lý do từ chối') + `</a>
                            	</div>
                            </div>`;
                    }

                },
            }
        ],
        "initComplete": function (settings, json) {
            ToTalNumber();
        },
    });

    $('#ContractTable').on('page.dt', function () {
        ToTalNumber()
    });

    var ToTalNumber = function (e) {
        var totalval = $(".total");
        var datafilter = {};
        $.each(totalval, function (index, value1) {

            console.log((value1));
            datafilter.quoSyn = (value1).dataset.quosyn;
            datafilter.Supper = (value1).dataset.sup;
            _contractService.totalNumber(datafilter).done(function (result) {
                var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
                var tong = result + (result * 10) / 100;
                $(value1).parent('td').find('span:eq(1)').text(numFormat(tong));

                
            })
        })
        
    }


    abp.event.on('app.reloadgetDocContact', function () {
        getDocContact();
    });

    function getDocContact() {
        ContractdataTable.ajax.reload();
    }

    $('#ContractTable').on('click', '.Approve', function (e) {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.status = 1;
        dataFilter.id = btnClick[0].dataset.quotesyn;
        debugger
        _contractService.updateExportStatus(dataFilter).done(function () {
            abp.notify.success(app.localize('Phê duyệt Hợp dồng thành công'));
            getDocContact();
        });
    });
    $('#ContractTable').on('click', '.Reject', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quotesyn };
        _createModalContractReject.open(dataFilter);
        //_quoteSynService.update(dataFilter).done(function () {
        //    abp.notify.success(app.localize('Phê duyệt trình báo giá thành công'));
        //    getDocs()
        //});
    });

    $('#ContractTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick.attr("data-view") };

        

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Contract/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Contract/Print?Id=" + results.id;
            },
        });
    });

    $('#ContractTable').on('click', '.view-detail', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick.attr("data-view") };



        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Contract/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Contract/Print?Id=" + results.id;
            },
        });
    });

    ////---------------------------------------------------  / Bảng Danh Sách Hợp đồng ---------------------------------------



})(jQuery);