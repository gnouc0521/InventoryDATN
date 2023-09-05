
(function () {
    var _$ContractTable = $("#ContractTable");

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

    ////---------------------------------------------------  Bảng Danh Sách Hợp đồng ---------------------------------------
    if (abp.auth.isGranted('Inventorys.Quote.Edit')) {
        var getFilter = function () {
            let dataFilter = {};
            dataFilter.status = 1;
            return dataFilter;
        }
    }
    else {
        var getFilter = function () {
            let dataFilter = {};
            dataFilter.status = 2;
            return dataFilter;
        }
    }
    

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
            ajaxFunction: _contractService.getAllInApprove,
            inputFilter: getFilter
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
                            <span class="viewtotal`+ row.id + `"></span>`;
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
                    if (abp.auth.isGranted('Inventorys.Quote.Edit')) {
                        if (status == 1) {
                            return `<span class="span_status span-defaut"> Chờ xử lý </span>`
                        } else if (status == 2) {
                            return `<span class="span_status span-subcontract"> Đã gửi </span>`
                        } else if (status == 3 || status == 4) {
                            return `<span class="span_status span-reject"> Từ chối </span>`
                        }
                        else if (status == 6) {
                            return `<span class="span_status span-approve"> Đã kí hợp đồng</span>`
                        }
                    }
                    else {
                        if (status == 2) {
                            return `<span class="span_status span-defaut"> Chờ xử lý </span>`
                        } else if (status == 5) {
                            return `<span class="span_status span-approve"> Đã duyệt </span>`
                        } else if (status == 3) {
                            return `<span class="span_status span-reject"> Từ chối </span>`
                        }
                        else if (status == 6) {
                            return `<span class="span_status span-approve"> Đã kí hợp đồng</span>`
                        }
                    }
                }
            },
            {

                orderable: false,
                targets: 7,
                data: null,
                width: "5%",
                render: function (data, type, row, meta) {
                    return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item view-detail' data-view='` + row.id + `' href='javascript:void(0);'>` + app.localize('Xem chi tiết') + `</a>
                            	</div>
                            </div>`;

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
        dataFilter.link = window.location.href;
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