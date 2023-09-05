(function () {
    var _$ExportTable = $('#ExportTable');
    var _$ExportRequimentTable = $('#ExportRequimentTable');
    var _ExportService = abp.services.app.exportRequests;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ExportRequests/CreateExportRequests',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_CreateModal.js',
        modalClass: 'ExportRequestsCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ExportRequests/EditExportRequests',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_EditModal.js',
        modalClass: 'ExportRequestsEditModal',
        modalType: 'modal-xl'
    });

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });


    $('.date-picker').datepicker({
        rtl: false,
        format: 'dd/mm/yyyy',
        orientation: "left",
        autoclose: true,
        language: abp.localization.currentLanguage.name,

    });

    ///* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
       // var Date = $("#RequestDate").datepicker("getDate");
        //if (Date != null) {
        //    dateStart = moment(Date).format('L');
        //    dataFilter.requestDate = dateStart
        //}
       // dataFilter.searchTerm = $('#SearchTerm').val().trim();
      //  dataFilter.warehouseDestinationId = $('#ProducerCode').val();
        dataFilter.status = 2;
        dataFilter.exportStatus = 0;
        return dataFilter;
    }



    var dataTable = _$ExportTable.DataTable({
        paging: true,
        serverSide: false,
        processing: false,
        "searching": true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "ordering": false,
        "bInfo": false,
        "bLengthChange": true,
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
        pageLength: 10,
        listAction: {
            ajaxFunction: _ExportService.getAll,
            inputFilter: getFilter
        },
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
                orderable: true,
                targets: 1,
                data: 'code',
                render: function (data, type, row, meta) {
                    
                    return `
                            <a class='Exportviewfunc'  data-objid='` + row.id + `' href='javascript:void(0);'>` + row.code + ` </a>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "codeRequirement",
                render: function (data, type, row, meta) {
                    if (row.codeTransfer == null || row.codeTransfer == "") {
                        return row.codeRequirement
                    } else {
                        return row.codeTransfer
                    }
                    //if (row.requestType == 0) {
                    //    return row.tranferCode
                    //} else {
                    //    return row.codeRequirement
                    //}

                }
            },
            {
                orderable: false,
                targets: 3,
                data: "warehouseDestinationName",

            },
            {
                orderable: false,
                targets: 4,
                data: null,
                render: function (data, type, row, meta) {
                    if (row.requestType == 0) {
                        return row.listWarehouseSourceName
                    }
                    if (row.requestType == 1) {
                        if (row.subsidiaryName == null) {
                            return `
                            <a class='Exportviewfunc'  data-objid='` + row.id + `' href='javascript:void(0);'>` + row.code + ` </a>`;
                        } else {
                            return row.subsidiaryName;
                        }
                    }

                }
            },
            {
                orderable: false,
                targets: 5,
                data: "creationTime",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 6,
                data: null,
                render: function (data, type, row, meta) {
                    if (row.status == 4) {
                        return moment(row.lastModificationTime).format('L');
                    } else {
                        return `<span></span>`
                    }

                }
            },
            {
                orderable: true,
                class: 'text-center',
                targets: 7,
                data: "status",
                render: function (data, type, row, meta) {
                    if (row.exportStatus == 1) {
                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut"> Chưa xuất hàng</span>`
                    } else {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-approve">Đã xuất hàng</span>`
                    }
                }
            },
            {

                targets: 8,
                data: 'id',
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    if (row.status == 0) {
                        var html = ''
                        if (abp.auth.isGranted('Inventorys.ExportRequests.Edit')) {
                            html = `<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-edit"></i> Chỉnh Sửa </a>`
                        }
                        return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item docprintfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-print"></i> In phiếu</a>
                            		<a class='dropdown-item docview'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-eye"></i> Chi tiết </a>
                                    `+ html + `
                            	</div>
                            </div>`;
                    } else {
                        return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item docprintfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-print"></i> In phiếu</a>
                            		<a class='dropdown-item docview'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-eye"></i> Chi tiết </a>
                            	</div>
                            </div>`;
                    }

                }
            },

        ]
    })

    ///* DEFINE TABLE */
    var getFilterRequiment = function () {
        let dataFilter = {};
        //var Date = $("#RequestDate").datepicker("getDate");
        //if (Date != null) {
        //    dateStart = moment(Date).format('L');
        //    dataFilter.requestDate = dateStart
        //}
        //dataFilter.searchTerm = $('#SearchTerm').val().trim();
        //dataFilter.warehouseDestinationId = $('#ProducerCode').val();
        dataFilter.status = 2;
        dataFilter.exportStatus = 0;
        return dataFilter;
    }



    var RequimentTable = _$ExportRequimentTable.DataTable({
        paging: true,
        serverSide: false,
        processing: false,
        "searching": true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "ordering": false,
        "bInfo": false,
        "bLengthChange": true,
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
        pageLength: 10,
        listAction: {
            ajaxFunction: _ExportService.getAllRequirementApprove,
            inputFilter: getFilterRequiment
        },
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
                orderable: true,
                targets: 1,
                data: 'codeRequirement',
                render: function (data, type, row, meta) {
                    return `
                            <a class='Exportviewfunc'  data-objid='` + row.id + `' href='javascript:void(0);'>` + row.codeRequirement + ` </a>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "warehouseDestinationName",

            },
            {
                orderable: false,
                targets: 3,
                data: null,
                render: function (data, type, row, meta) {
                        return row.subsidiaryName;
                }
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
                data: null,
                render: function (data, type, row, meta) {
                    if (row.status == 3) {
                        return moment(row.lastModificationTime).format('L');
                    } else {
                        return `<span></span>`
                    }

                }
            },

            {

                targets: 6,
                data: 'id',
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return ` <button id="Create-Export" name="CreateExport" class="btn btn-sm btn-toolbar-full btn-primary ml-auto Create-Export" data-objid=` + row.id + `>
                                Tạo phiếu xuất
                                </button>`
                }
            },

        ]
    })




    $('#ExportRequimentTable').on('click', '.Create-Export', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ExportRequests/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ExportRequests/ExportRequirementDetail?id=" + results.id;
            },
        });

        //abp.message.confirm(
        //    app.localize('Bạn có chắc chắn muốn tạo phiếu xuất'),
        //    app.localize('Tạo phiếu xuất'),
        //    function (isConfirmed) {
        //        if (isConfirmed) {
        //            _ExportService.updateExportStatus({ id: dataFilter.id, exportStatus : 1 }).done(function () {
        //                abp.notify.success(app.localize('Xóa phiếu xuất kho thành công'));
        //                getDocs();
        //            })
        //        }
        //    }
        //);

        // If checkbox doesn't exist in DOM
        // If checkbox is checked
    })
    $('#ExportRequimentTable').on('click', '.Exportviewfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ExportRequests/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ExportRequests/ExportRequirementDetail?id=" + results.id;
            },
        });



        //abp.message.confirm(
        //    app.localize('Bạn có chắc chắn muốn tạo phiếu xuất'),
        //    app.localize('Tạo phiếu xuất'),
        //    function (isConfirmed) {
        //        if (isConfirmed) {
        //            _ExportService.updateExportStatus({ id: dataFilter.id, exportStatus : 1 }).done(function () {
        //                abp.notify.success(app.localize('Xóa phiếu xuất kho thành công'));
        //                getDocs();
        //            })
        //        }
        //    }
        //);

        // If checkbox doesn't exist in DOM
        // If checkbox is checked
    })



    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#ExportTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#ExportTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#ExportTable tbody input[type="checkbox"]:checked').each(function () {
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
            app.localize('Bạn có chắc chắn muốn xóa hàng hóa'),
            app.localize('Xóa hàng hóa'),
            function (isConfirmed) {
                if (isConfirmed) {
                    // Iterate over all checkboxes in the table
                    dataTable.$('input[type="checkbox"]').each(function (index, value) {
                        if ($(value).is(":checked")) {
                            _ExportService.delete(
                                $(value).val()
                            ).done(function () {
                                // getUsers(true);
                                $('#example-select-all').prop('checked', false);
                                abp.notify.info(app.localize('Tạo phiếu xuất thành công'));
                                getDocs();
                            });
                        }
                        // If checkbox doesn't exist in DOM
                        // If checkbox is checked

                    }
                    );

                }
            });
    })

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    //$('#ExportTable').on('click', '.doceditfunc', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.objid, warehouseId : $('#WarehouseId').val() };
    //    _EditModal.open(dataFilter);
    //});
    //$('#ExportTable').on('click', '.Exportviewfunc', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.objid };

    //    abp.ajax({
    //        contentType: "application/x-www-form-urlencoded",
    //        url: abp.appPath + "PersonalProfile/Export/OverView?Id=" + dataFilter.id,
    //        success: function (results) {
    //            window.location.href =
    //                "/PersonalProfile/Export/Detail?id=" + results.id;
    //        },
    //    });
    //});

    $('#ExportTable').on('click', '.docview', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ExportRequests/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ExportRequests/ViewDetails?id=" + results.id;
            },
        });
    });
    $('#ExportTable').on('click', '.Exportviewfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ExportRequests/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ExportRequests/ViewDetails?id=" + results.id;
            },
        });
    });
    $('#ExportTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });

    $('#ExportTable').on('click', '.docprintfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ExportRequests/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ExportRequests/Print?id=" + results.id;
            },
        });
    });

    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {


        dataTable.ajax.reload();
        RequimentTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });


})(jQuery);