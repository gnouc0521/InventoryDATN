(function () {
    var _$ExportRequirementTable = $('#ExportRequirementTable');
    var _ExportService = abp.services.app.exportRequests;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ExportRequests/CreateExportrequirement',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_CreateExportRequirement.js',
        modalClass: 'ExportRequirementCreateModal',
        modalType: 'modal-xl'

    });

     var _createModalCraft = new app.ModalManager({
         viewUrl: abp.appPath + 'Inventorys/ExportRequests/CreateExportCraft',
         scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_CreateModalCraft.js',
         modalClass: 'ExportRequirementEditModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ExportRequests/EditExportrequirement',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ExportRequests/_EditExportRequirement.js',
        modalClass: 'ExportRequirementEditModal',
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
        //var Date = $("#RequestDate").datepicker("getDate");
        //if (Date != null) {
        //    dateStart = moment(Date).format('L');
        //    dataFilter.requestDate = dateStart
        //}
        //dataFilter.searchTerm = $('#SearchTerm').val().trim();
        //dataFilter.warehouseDestinationId = $('#ProducerCode').val();
        if (abp.auth.isGranted("Inventorys.ExportRequests.Approve")) {
            dataFilter.status = 0
        } else {
            dataFilter.userIdCreate = abp.session.userId;
        }
        return dataFilter;
    }



    var dataTable = _$ExportRequirementTable.DataTable({
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
            ajaxFunction: _ExportService.getAllRequirement,
            inputFilter: getFilter
        },
        columnDefs: [

            {
                targets: 0,
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    return '<input type="checkbox" name="" value="' + row.id + '">';
                }
            },
            {
                orderable: true,
                targets: 1,
                data: 'code',
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
                data: "subsidiaryName",
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
                data: "lastModificationTime",
                render: function (data, type, row, meta) {
                    if (row.status != 0 && row.status != 1) {
                        return moment(row.lastModificationTime).format('L');
                    } else {
                        return `<span></span>`;
                    }
                }
            },
            {
                orderable: true,
                class: 'text-center',
                targets: 6,
                data: "status",
                render: function (data, type, row, meta) {
                    if (row.status == 0 || (row.status == 1 && abp.session.userId != row.creatorUserId)) {

                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa xử lý</span>`
                    } else if (row.status == 3) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-approve">Đã phê duyệt</span>`
                    } else if (row.status == 1 && abp.session.userId == row.creatorUserId) {
                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Đã gửi</span>`
                    } else if (row.status == 2) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-reject">Bị từ chối</span>`
                    } else {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-contract">Hoàn thành</span>`
                    }
                }
            },
            {

                targets: 7,
                data: 'id',
                class: 'text-center',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    if (row.status == 0 || row.status == 2 ) {
                        var html = '';
                        var htmlcreate = '';

                        if (abp.auth.isGranted('Inventorys.ExportRequests.Edit')) {
                            html = `<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-edit"></i> Chỉnh Sửa </a>`
                        }
                        if (abp.auth.isGranted('Inventorys.ExportRequests.Create')) {
                            htmlcreate = `<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-edit"></i> Chỉnh Sửa </a>`
                        }
                        return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item docprintfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-print"></i> In phiếu</a>
                            		<a class='dropdown-item docview'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-eye"></i> Chi tiết </a>
                                    `+ html +`
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

   

    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#ExportRequirementTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#ExportRequirementTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#ExportRequirementTable tbody input[type="checkbox"]:checked').each(function () {
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
                                abp.notify.success(app.localize('Xóa phiếu xuất kho thành công'));
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


    $('#ExportRequirementTable').on('click', '.docview', function (e) {
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
    });
    //$('#ExportRequirementTable').on('click', '.Exportviewfunc', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.objid };

    //    abp.ajax({
    //        contentType: "application/x-www-form-urlencoded",
    //        url: abp.appPath + "Inventorys/ExportRequests/OverView?Id=" + dataFilter.id,
    //        success: function (results) {
    //            window.location.href =
    //                "/Inventorys/ExportRequests/ExportRequirementDetail?id=" + results.id;
    //        },
    //    });
    //});
    $('#ExportRequirementTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });
    $('#ExportRequirementTable').on('click', '.Exportviewfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _createModalCraft.open(dataFilter);
    });

    $('#ExportRequirementTable').on('click', '.docprintfunc', function (e) {
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
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });


})(jQuery);