(function () {
    var _$importRequestTable = $('#ImportRequestTable');
    var _importRequestService = abp.services.app.importRequest;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/WarehouseCard/Create',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/WarehouseCard/_CreateImpModal.js',
        modalClass: 'ImportRequestCreateModal',
        modalType: 'modal-xl'
    });

    var _EditModal = new app.ModalManager({
      viewUrl: abp.appPath + 'Inventorys/WarehouseCard/Edit',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ImportRequests/_EditImpModal.js',
        modalClass: 'ImportRequestEditModal',
        modalType: 'modal-xl'
    });

    var _ViewDetails = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ImportRequest/ViewDetails',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ImportRequests/ViewDetail.js',
        modalClass: 'ViewModal',
        modalType: 'modal-xl'
    });

    //$('.date-picker').datepicker({
    //    rtl: false,
    //    format: "dd/mm/yyyy",
    //    orientation: "left",
    //    autoclose: true,
    //    language: abp.localization.currentLanguage.name,

    //});


    /* DEFINE TABLE */
    //var getFilter = function () {
    //    let idWarehouse = document.getElementById("WarehouseDestinationId");
    //  /*  let idName = document.getElementById("NameRequest");*/
    //    var Date = $("#RequestDate").datepicker("getDate");
    //    let dataFilter = {};
    //    dataFilter.searchTerm = $('#SearchTerm').val();
    //    dataFilter.warehouseDestinationId = $("#WarehouseDestinationId :selected").val(); 
    //    dataFilter.creatorById = $("#NameRequest :selected").val();
    //    dataFilter.status = $("#Status :selected").val();
    //    dataFilter.warehouseDestinationId = idWarehouse.options[idWarehouse.selectedIndex].value;
    //   /* dataFilter.nameRequest = idName.options[idName.selectedIndex].value;*/
    //    if (Date != null) {
            
    //        dateStart = moment(Date).format('L');
    //        dataFilter.resquestDate = dateStart;
    //    }
    //    return dataFilter;
    //}

    //var dataTable = _$importRequestTable.DataTable({
    //    paging: true,
    //    serverSide: false,
    //    processing: true,
    //    "searching": false,
    //    "language": {
    //        "emptyTable": "Không tìm thấy dữ liệu",
    //        "lengthMenu": "Hiển thị _MENU_ bản ghi",
    //        "zeroRecords": "Không tìm thấy dữ liệu",
    //        searchPlaceholder: "Tìm kiếm"
    //    },
    //    "bInfo": false,
    //    "bLengthChange": true,
    //    lengthMenu: [
    //        [5, 10, 25, 50, -1],
    //        [5, 10, 25, 50, 'Tất cả'],
    //    ],
       

    //    pageLength: 10,
    //    listAction: {
    //        ajaxFunction: _importRequestService.getAll,
            
    //    },
    //    order: [[0,'asc']],
    //    columnDefs: [

    //        {
    //            targets: 0,
    //            orderable: false,
    //            className: 'dt-body-center text-center',
    //            render: function (data, type, row, meta) {
    //                return '<input type="checkbox" name="" data-value="' + row.id + '">';
    //            }
    //        },
    //        {
    //            orderable: false,
    //            targets: 1,
    //            data: "code",
    //            render: function (data, type, row, meta) {
    //                return `
    //                    <a class="viewwork" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.code + `</a>
    //                `
    //            }
    //        },
    //        {
    //            orderable: false,
    //            targets: 2,
    //            data: "createdBy"
    //        },
    //        {
    //            orderable: false,
    //            targets: 3,
    //            data: "importStatus",
    //            render: function (data, type, row, meta) {
    //                if (row.importStatus == 0) {

    //                    return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed">Đã nhận hàng</span>`
    //                } else {
    //                    return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed">Chờ nhận hàng</span>`
    //                }
    //            }
    //        },
    //        {
    //            orderable: false,
    //            targets: 4,
    //            data: "requestDate",
    //            render: function (creationTime) {
    //                return moment(creationTime).format('L');
    //            }
    //        },
    //        {

    //            targets: 5,
    //            data: null,
    //            class: 'text-center',
    //            orderable: false,
    //            autoWidth: false,
    //          render: function (data, type, row, meta) {
    //            var CanEdit = "";
    //            if (row.importStatus != 0) {
    //              CanEdit = `<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa - edit"></i> Chỉnh Sửa </a>`
    //            }
    //                return `                        
    //                        <div class='dropdown d-inline-block'>
    //                        	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
    //                        		<i class="fal fa-ellipsis-v"></i>
    //                        	</a>
    //                        	<div class='dropdown-menu'>
    //                        		<a class='dropdown-item docprintfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-print"></i> In phiếu</a>
    //                            `+ CanEdit +`
    //                        		<a class='dropdown-item docview'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-eye"></i> Chi tiết </a>
                                    
    //                        	</div>
    //                        </div>`;
    //            }
    //        }
    //    ]
    //});

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });


    $('#example-select-all').on('click', function () {
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#ImportRequestTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });


    $('#ImportRequestTable tbody').on('change', 'input[type="checkbox"]', function () {
        var selected = new Array();
        $('#ImportRequestTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }

        if (!this.checked) {
            var el = $('#example-select-all').get(0);
            if (el && el.checked && ('indeterminate' in el)) {
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
            app.localize('Xóa phiếu nhập'),
            app.localize('Bạn có chắc không'),
            function (isConfirmed) {
                if (isConfirmed) {
                    dataTable.$('input[type="checkbox"]').each(function (index, value) {
                        if ($(value).is(":checked")) {
                            _importRequestService
                                .delete(
                                    $(value).attr("data-value")
                                ).done(function () {
                                    $('#example-select-all').prop('checked', false);
                                    abp.notify.success(app.localize('Xóa phiếu nhập thành công'));
                                    getDocs();
                                });
                        }
                    }
                    );

                }
            });
    })


    $('#ImportRequestTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });


    $('#ImportRequestTable').on('click', '.viewwork', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequest/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ImportRequest/ViewDetails?id=" + results.id;
            },
        });
    });

    $('#ImportRequestTable').on('click', '.docview', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequest/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ImportRequest/ViewDetails?id=" + results.id;
            },
        });
    });

    $('#ImportRequestTable').on('click', '.docprintfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/ImportRequest/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/ImportRequest/Print?id=" + results.id;
            },
        });
    });
    abp.event.on('app.reloadDocTable', function () {
        getDocs();
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