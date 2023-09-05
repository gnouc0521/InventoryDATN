(function () {
    var _$PurchasesRequestTable = $('#PurchasesRequestTable');
    var _$AllRequestTable = $('#AllRequestTable');
    var _alltableConfirm = $('#AllRequestTableConfirm');
    var _purchasesRequestService = abp.services.app.purchasesRequestsService;
    var _purchasesSynthesisService = abp.services.app.purchasesSynthesise;
    var _useworkcount = abp.services.app.userWorkCountService;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/PurchasesRequest/CreatePurchasesRequest',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/PurchasesRequests/_CreateModal.js',
        modalClass: 'PurchasesRequestCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/PurchasesRequest/UpdatePurchasesRequest',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/PurchasesRequests/_EditModal.js',
        modalClass: 'PurchasesRequestEditModal',
        modalType: 'modal-xl'
    });

    var _ViewDetails = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/PurchasesRequest/ViewDetails',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/PurchasesRequests/ViewDetail.js',
        modalClass: 'ViewModal',
        modalType: 'modal-xl'
    });



    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    $('.date-picker').datepicker({
        rtl: false,
        format: "dd/mm/yyyy",
        orientation: "left",
        autoclose: true,
        language: abp.localization.currentLanguage.name,

    });


    /* DEFINE TABLE */
    var getFilter = function () {
        dataFilter.searchTerm = $('#SearchTerm').val();
        return dataFilter;
    }


    var dataTable = _$PurchasesRequestTable.DataTable({
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


        pageLength: 5,
        listAction: {
            ajaxFunction: _purchasesRequestService.getAll,
        },
        order: [[1, 'asc']],
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    if (row.requestStatus == 0) {
                        return '<input type="checkbox" name="" data-value="' + row.id + '" disabled>';
                    } else {
                        return '<input type="checkbox" name="" data-value="' + row.id + '">';
                    }
                }
            },
            {
                searchable: false,
                orderable: false,
                targets: 1,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "subsidiaryCompany",
                render: function (data, type, row, meta) {
                    return `
                        <a class="viewsubsidiaryCompany" data-viewid='` + row.id + `' href='javascript:void(0); '>` + row.subsidiaryCompany + `</a>
                    `
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "address"
            },
            {
                orderable: false,
                targets: 4,
                data: "phoneNumber"
            },
            {
                orderable: false,
                targets: 5,
                data: "emailAddress"
            },
            {
                targets: 6,
                data: "requestDate",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 7,
                data: "requestStatus",
                render: function (status) {
                    if (status == 2) {
                        return `<span class="span_status span-defaut"> Chưa tổng hợp </span>`
                    } else if (status == 0) {
                        return `<span class="span_status span-approve"> Đã tổng hợp </span>`
                    }
                }
            },
            {

                targets: 8,
                data: null,
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                                    <a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-edit"></i> Chỉnh Sửa </a>
                            	</div>
                            </div>`;
                }
            }
        ]
    });
    //dataTable.on('order.dt search.dt', function () {
    //    let i = 1;

    //    dataTable.cells(null, 1, { search: 'applied', order: 'applied' }).every(function (cell) {
    //        this.data(i++);
    //    });
    //}).draw();

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#PurchasesRequestTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });

    $('#PurchasesRequestTable').on('click', '.viewsubsidiaryCompany', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.viewid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/PurchasesRequest/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/PurchasesRequest/ViewDetails?id=" + results.id;
            },
        });
    });

    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {
        AllRequestTable.ajax.reload();
        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });

    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).not(':disabled').prop('checked', this.checked);
        $('#RequestAll').removeAttr('disabled');
        var selected = new Array();
        $('#PurchasesRequestTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#RequestAll').removeAttr('disabled');
        } else {
            $('#RequestAll').prop('disabled', true);
        }
        console.log(selected)
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#PurchasesRequestTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#PurchasesRequestTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#RequestAll').removeAttr('disabled');
        } else {
            $('#RequestAll').prop('disabled', true);
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
            $('#RequestAll').removeAttr('disabled');
        }
    })

    $('#RequestAll').on('click', function (e) {
        abp.message.confirm(
            app.localize('Bạn có chắc chắn muốn tổng hợp yêu cầu mua hàng không'),
            app.localize('Tổng hợp yêu cầu'),
            function (isConfirmed) {
                if (isConfirmed) {
                    _purchasesSynthesisService.create({}).done(function (result) {
                        dataTable.$('input[type="checkbox"]').each(function (index, value) {
                            if ($(value).is(":checked")) {
                                let data = {};
                                data.Id = $(value).attr('data-value');
                                data.PurchasesSynthesiseId = result
                                _purchasesRequestService.updateSynId(data).done(function () {
                                    getDocs()
                                    data.purchasesRequestId = $(value).attr('data-value')
                                    data.workStatus = 0;
                                    _useworkcount.updateSys(data);
                                })
                            };
                        })


                    });


                }
            });
    })



    ///xoa

    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).not(':disabled').prop('checked', this.checked);
        $('#DeleteP').removeAttr('disabled');
        var selected = new Array();
        $('#PurchasesRequestTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteP').removeAttr('disabled');
        } else {
            $('#DeleteP').prop('disabled', true);
        }
        console.log(selected)
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#PurchasesRequestTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#PurchasesRequestTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteP').removeAttr('disabled');
        } else {
            $('#DeleteP').prop('disabled', true);
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


    $('#DeleteP').on('click', function (e) {
        abp.message.confirm(
            app.localize('Bạn có chắc chắn muốn xoá cầu mua hàng không'),
            app.localize('Xoá yêu cầu'),
            function (isConfirmed) {
                if (isConfirmed) {
                    dataTable.$('input[type="checkbox"]').each(function (index, value) {
                        if ($(value).is(":checked")) {
                            console.log(value)
                            _purchasesRequestService.delete(
                                $(value).attr('data-value')
                            ).done(function (results) {
                                abp.notify.success(app.localize('Xóa phiếu thành công'));
                                getDocs();
                            });
                        }
                    }
                    );

                }
            });
    })


    var getFilter = function () {
        let dataFilter;
        return dataFilter;
    }


    var AllRequestTable = _$AllRequestTable.DataTable({
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
            ajaxFunction: _purchasesSynthesisService.getAll,
            inputFilter: getFilter
        },
        'rowsGroup': {
            dataSrc: 2,
        },
        order: [[1, 'asc']],
        columnDefs: [

            {

                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    return '<input type="checkbox" name="" data-value="' + row.id + '">';
                }
            },
            {

                orderable: false,
                targets: 1,
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                name: 'first',
                orderable: false,
                targets: 2,
                data: "purchasesSynthesiseCode",
                render: function (data, type, row, meta) {
                    return `
                        <a class="purchasesSynthesiseCode" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.purchasesSynthesiseCode + `</a>
                    `
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "subsidiaries",
                render: function (data, type, row, meta) {
                    var html = "";
                    for (var i = 0; i < row.subsidiaries.length; i++) {
                        html += `<span class="mb-1"> ` + row.subsidiaries[i] + `</span><br>`;
                    }

                    return html;
                },
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
                data: "purchasesRequestStatus",
                render: function (data, type, row, meta) {



                    if (row.purchasesRequestStatus == 0) {
                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa xử lý</span>`
                    } else if (row.purchasesRequestStatus == 2) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-approve">Đã phê duyệt</span>`
                    } else if (row.purchasesRequestStatus == 1) {
                        debugger
                        if (abp.session.userId != row.creatorUserId) {
                            return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa xử lý</span>`

                        } else {
                            return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Đã gửi</span>`
                        }
                    }
                    else if (row.purchasesRequestStatus == 3) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-reject">Bị từ chối</span>`
                    }
                }
            },
            {

                targets: 6,
                data: null,
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item docview'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-eye"></i> Chi tiết </a>
                            	</div>
                            </div>`;
                }
            }
        ],
    });


    var AllRequestTableCofirm = _alltableConfirm.DataTable({
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
            ajaxFunction: _purchasesSynthesisService.getAllConfirm,
            inputFilter: getFilter
        },
        'rowsGroup': {
            dataSrc: 2,
        },
        order: [[0, 'asc']],
        columnDefs: [
            {

                orderable: false,
                targets: 0,
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                name: 'first',
                orderable: false,
                targets: 1,
                data: "purchasesSynthesiseCode",
                render: function (data, type, row, meta) {
                    return `
                        <a class="purchasesSynthesiseCode" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.purchasesSynthesiseCode + `</a>
                    `
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "subsidiaries",
                render: function (data, type, row, meta) {
                    var html = "";
                    for (var i = 0; i < row.subsidiaries.length; i++) {
                        html += `<span class="mb-1"> ` + row.subsidiaries[i] + `</span><br>`;
                    }

                    return html;
                },
            },
            {
                orderable: false,
                targets: 3,
                data: "creationTime",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 4,
                data: "purchasesRequestStatus",
                render: function (data, type, row, meta) {



                    if (row.purchasesRequestStatus == 0) {
                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa xử lý</span>`
                    } else if (row.purchasesRequestStatus == 2) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-approve">Đã phê duyệt</span>`
                    } else if (row.purchasesRequestStatus == 1) {
                        debugger
                        if (abp.session.userId != row.creatorUserId) {
                            return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa xử lý</span>`

                        } else {
                            return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Đã gửi</span>`
                        }
                    }
                    else if (row.purchasesRequestStatus == 3) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-reject">Bị từ chối</span>`
                    }
                }
            },
            {

                targets: 5,
                data: null,
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item docview'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-eye"></i> Chi tiết </a>
                            	</div>
                            </div>`;
                }
            }
        ],
    });


    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });


    $('#AllRequestTableConfirm').on('click', '.docview,.purchasesSynthesiseCode', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        console.log("aa")

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequest/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/PurchasesRequest/DetailSynthesise?SynthesiseId=" + results.id
            },
        });
    });


    $('#AllRequestTable').on('click', '.docview,.purchasesSynthesiseCode', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        console.log("aa")

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequest/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/PurchasesRequest/DetailSynthesise?SynthesiseId=" + results.id
            },
        });
    });


    $('#example-select-all1').on('click', function () {
        // Get all rows with search applied
        var rows = AllRequestTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#AllRequestTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
        console.log(selected)
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#AllRequestTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#AllRequestTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }

        if (!this.checked) {
            var el = $('#example-select-all1').get(0);
            // If "Select all" control is checked and has 'indeterminate' property
            if (el && el.checked && ('indeterminate' in el)) {
                // Set visual state of "Select all" control
                // as 'indeterminate'
                el.indeterminate = true;
            }
        }
    });

    AllRequestTable.$('input[type="checkbox"]').each(function (index, value) {
        if (value > 0) {
            $('#DeleteAll').removeAttr('disabled');
        }
    })

    $('#DeleteAll').on('click', function (e) {
        abp.message.confirm(
            app.localize('Bạn có chắc chắn muốn xóa phiếu tổng hợp không '),
            app.localize('Xóa phiếu tổng hợp'),
            function (isConfirmed) {
                if (isConfirmed) {
                    AllRequestTable.$('input[type="checkbox"]').each(function (index, value) {
                        if ($(value).is(":checked")) {
                            console.log(value)
                            _purchasesSynthesisService.delete(
                                $(value).attr('data-value')
                            ).done(function (results) {
                                $.each(results, function (index, value) {
                                    let data = {};
                                    data.Id = value;
                                    _purchasesRequestService.updateSynId(data)
                                })
                                // getUsers(true);

                                $('#example-select-all1').prop('checked', false);
                                abp.notify.success(app.localize('Xóa phiếu tổng hợp thành công'));
                                getDocs();
                            });
                        }
                        // If checkbox doesn't exist in DOM
                        // If checkbox is checked
                        console.log($(value).val())

                    }
                    );
                    // Iterate over all checkboxes in the table


                }
            });





     


    })


})(jQuery);