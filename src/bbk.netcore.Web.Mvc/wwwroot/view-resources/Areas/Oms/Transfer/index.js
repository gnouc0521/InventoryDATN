(function () {
    var _$tranferTable = $('#TranferTable');
    var _transferService = abp.services.app.transfer;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Transfer/CreateTransfer',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Transfer/_CreateModal.js',
        modalClass: 'TransferCreateModal',
        modalType: 'modal-xl'

    });

    var _editModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Transfer/EditTransfer',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Transfer/_EditModal.js',
        modalClass: 'TransferEditModal',
        modalType: 'modal-xl'

    });

    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Transfer/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Transfer/_CreateModalReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });

    //var _EditModal = new app.ModalManager({
    //    viewUrl: abp.appPath + 'Inventorys/Supplier/EditSupplierModal',
    //    scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Suppliers/_EditModal.js',
    //    modalClass: 'SupplierEditModal',
    //    modalType: 'modal-xl'
    //});

    //var _ViewDetails = new app.ModalManager({
    //    viewUrl: abp.appPath + 'Inventorys/Supplier/ViewDetails',
    //    modalClass: 'ViewModal',
    //    modalType: 'modal-xl'
    //});

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val();
        return dataFilter;
    }


    var dataTable = _$tranferTable.DataTable({
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
            ajaxFunction: _transferService.getAll,
            inputFilter: getFilter
        },
        order: [[1, 'asc']],
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                width: "5%",
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    if (row.status == 0) {
                        return '<input type="checkbox" name="" data-value="' + row.id + '">';
                    }
                    else {
                        return '<input type="checkbox" disabled name="" data-value="' + row.id + '">';
                    }
                }
            },
            {
                orderable: false,
                width: "10%",
                targets: 1,
                data: "transferCode"
               
            },
            {
                orderable: false,
                width: "15%",
                targets: 2,
                data: "nameWareHouseExport"

            },
            {
                orderable: false,
                targets: 3,
                width: "15%",
                data: "createTime",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 4,
                width: "15%",
               /* data: "browsingTime",*/
                render: function (data, type, row, meta) {
                    if (row.status == 0) { return '' } else {
                        return moment(row.browsingTime).format('L');
                    }
                }
            },
            {
                orderable: false,
                width: "15%",
                targets: 5,
                data: "status",
                render: function (status) {
                    if (status == 0) {
                        return `<span class="span_status span-defaut"> Chờ xử lý </span>`
                    } else if (status == 1) {
                        return `<span class="span_status span-reject"> Từ chối </span>`
                    } else if (status == 2) {
                        return `<span class="span_status span-subcontract"> Đã gửi </span>`
                    } else if (status == 3) {
                        return `<span class="span_status span-approve"> Phê duyệt </span>`
                    }
                }
            },
            {
                orderable: false,
                targets: 6,
                width: "10%",
                className: "text-center",
                render: function (data, type, row, meta) {
                    if (row.status == 0 || row.status == 2 || row.status == 3) {
                        return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item viewDetail' data-QuoteSyn='` + row.id + `' href='javascript:void(0);'>` + app.localize('Xem chi tiết') + `</a>
                            	</div>
                            </div>`;
                    }
                    else {
                        return `
                            <div class='dropdown d-inline-block' >
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item viewDetail' data-QuoteSyn='` + row.id + `' href='javascript:void(0);'>` + app.localize('Xem lý do') + `</a>
                            		<a class='dropdown-item editTranssfer' data-QuoteSyn='` + row.id + `' href='javascript:void(0);'>` + app.localize('Sửa') + `</a>
                            	</div>
                            </div>`;
                    }
                }
            },
        ]
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });


    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).not(':disabled').prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#TranferTable tbody input[type="checkbox"]:checked').not(':disabled').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });


    $('#TranferTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#TranferTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }

        if (!this.checked) {
            var el = $('#example-select-all').get(0);
            // If "Select all" control is checked and has 'indeterminate' property
            if (el && el.checked && ('indeterminate' in el)) {
                // Set visual state of "Select all" control
                // as 'indeterminate'
                el.indeterminate = true;
            }
        }
    });



    dataTable.$('input[type="checkbox"]').each(function (index, value) {
        if (value > 0) {
            $('#DeleteAll').removeAttr('disabled');
        }
    })


    $('#DeleteAll').on('click', function (e) {
        abp.message.confirm(
            app.localize('Xóa nhà sản xuất'),
            app.localize('Bạn có chắc không'),
            function (isConfirmed) {
                if (isConfirmed) {
                    // Iterate over all checkboxes in the table
                    dataTable.$('input[type="checkbox"]').each(function (index, value) {
                        if ($(value).is(":checked")) {
                            console.log("valưe", $(value).attr("data-value"))
                            _transferService
                                .delete(
                                    $(value).attr("data-value")
                                ).done(function () {
                                    // getUsers(true);
                                    $('#example-select-all').prop('checked', false);
                                    abp.notify.success(app.localize('Xóa phiếu chuyển giao thành công'));
                                    getDocs();
                                });
                        }
                    }
                    );

                }
            });
    })

    //$('#TranferTable').on('click', '.Approve', function (e) {
    //    var btnClick = $(this);
    //    let dataFilter = {};
    //    dataFilter.status = 2;
    //    dataFilter.id = btnClick[0].dataset.quotesyn;
    //    _transferService.update(dataFilter).done(function () {
    //        abp.notify.success(app.localize('Phê duyệt phiếu chuyển giao thành công'));
    //        getDocs();

    //    });
    //});

    $('#TranferTable').on('click', '.editTranssfer', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quotesyn };
        _editModal.open(dataFilter);

    });

    $('#TranferTable').on('click', '.viewDetail', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quotesyn };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Transfer/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Transfer/ViewDetails?id=" + results.id;
            },
        });

    })



    //$('#supplierTable').on('click', '.doceditfunc', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.objid };
    //    _EditModal.open(dataFilter);
    //});


    //$('#supplierTable').on('click', '.viewsupplier', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.viewid };
    //    _ViewDetails.open(dataFilter);
    //});


    //$('#supplierTable').on('click', '.viewsupplier', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.viewid };

    //    abp.ajax({
    //        contentType: "application/x-www-form-urlencoded",
    //        url: abp.appPath + "Inventorys/Supplier/OverView?Id=" + dataFilter.id,
    //        success: function (results) {
    //            window.location.href =
    //                "/Inventorys/Supplier/ViewDetails?id=" + results.id;
    //        },
    //    });
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


})(jQuery);