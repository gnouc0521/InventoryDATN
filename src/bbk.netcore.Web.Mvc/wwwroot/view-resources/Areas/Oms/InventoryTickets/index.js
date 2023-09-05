(function () {
    var _$inventoryTicketTable = $('#InventoryTicketTable');
    var _inventoryTicketService = abp.services.app.inventoryTicketService;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/InventoryTicket/CreateImportRequest',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/InventoryTickets/_CreateModal.js',
        modalClass: 'ImportRequestCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/InventoryTicket/EditImportRequestModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/InventoryTickets/_EditModal.js',
        modalClass: 'ImportRequestEditModal',
        modalType: 'modal-xl'
    });

    var _ViewDetails = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ImportReqInventoryTicketuest/ViewDetails',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/InventoryTickets/ViewDetail.js',
        modalClass: 'ViewModal',
        modalType: 'modal-xl'
    });

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    var _permissions = {
        edit: abp.auth.hasPermission('Inventorys.InventoryTickets.Edit'),
    };

    $('.date-picker').datepicker({
        rtl: false,
        format: 'dd/mm/yyyy',
        orientation: "left",
        autoclose: true,
        language: abp.localization.currentLanguage.name,

    });


    /* DEFINE TABLE */
    var getFilter = function () {
        var Date = $("#RequestDate").datepicker("getDate");
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val();
        dataFilter.creatorById = $("#NameRequest :selected").val();
        dataFilter.warehouseId = $("#WarehouseDestinationId :selected").val(); 
        if (Date != null) {
            dateStart = moment(Date).format('L');
            dataFilter.resquestDate = dateStart
        }
        return dataFilter;
    }




    var dataTable = _$inventoryTicketTable.DataTable({
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
            ajaxFunction: _inventoryTicketService.getAll,
            inputFilter: getFilter
        },
        order : [[1,'asc']],
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
                data: "code"
            },
            {
                orderable: false,
                targets: 2,
                data: "createdBy"
            },
            {
                orderable: false,
                targets: 3,
                data: "status",
                render: function (data, type, row, meta) {
                    if (row.status == 0) {

                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed">Chờ phê duyệt</span>`
                    } else {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed">Đã phê duyệt</span>`
                    }
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
                            		<a class='dropdown-item docprintfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-print"></i> In phiếu</a>
                            		<a class='dropdown-item docview'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-eye"></i> Chi tiết </a>
                                    <a class='dropdown-item doctransfer'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-check"></i> Duyệt </a>
                                    <a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' ><i class="fal fa-edit"></i> Chỉnh Sửa </a>
                            	</div>
                            </div>`;
                }
            }
        ]
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });



    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#InventoryTicketTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });


    $('#InventoryTicketTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#InventoryTicketTable tbody input[type="checkbox"]:checked').each(function () {
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
            app.localize('Xóa yêu cầu'),
            app.localize('Bạn có chắc không'),
            function (isConfirmed) {
                if (isConfirmed) {
                    // Iterate over all checkboxes in the table
                    dataTable.$('input[type="checkbox"]').each(function (index, value) {
                        if ($(value).is(":checked")) {
                            console.log("valưe", $(value).attr("data-value"))
                            _inventoryTicketService
                                .delete(
                                    $(value).attr("data-value")
                                ).done(function () {
                                    // getUsers(true);
                                    $('#example-select-all').prop('checked', false);
                                    abp.notify.success(app.localize('Xóa yêu cầu nhập kho thành công'));
                                    getDocs();
                                });
                        }
                    }
                    );

                }
            });
    })


    $('#InventoryTicketTable').on('click', '.doceditfunc', function (e) {
        if (_permissions.edit) {
            var btnClick = $(this);
            var dataFilter = { id: btnClick[0].dataset.objid };
            _EditModal.open(dataFilter);
        }
        else {
            abp.notify.error('Bạn không có quyền vào mục này');
        }
    });


    $('#InventoryTicketTable').on('click', '.docview', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/InventoryTicket/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/InventoryTicket/ViewDetails?id=" + results.id;
            },
        });
    });

    $('#InventoryTicketTable').on('click', '.docprintfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/InventoryTicket/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/InventoryTicket/Print?id=" + results.id;
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