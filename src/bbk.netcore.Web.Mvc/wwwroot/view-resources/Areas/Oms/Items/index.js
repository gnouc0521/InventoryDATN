(function () {
    var _$ItemsTable = $('#ItemsTable');
    var _ItemsService = abp.services.app.itemsService;
    var _inventoryService = abp.services.app.inventoryService;
    var _warehouseLocation = abp.services.app.warehouseLocationItemService;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Items/CreateItems',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Items/_CreateModal.js',
        modalClass: 'ItemsCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Items/EditItemsModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Items/_EditModal.js',
        modalClass: 'EditItemsModal',
        modalType: 'modal-xl'
    });

    $('#CreateNewButtonxx').click(function () {
        var dataFilter = { wareHouseId: $('#WarehouseId').val()};
        _createModal.open(dataFilter);
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val().trim();
        dataFilter.wareHouseId = $('#WarehouseId').val();
        return dataFilter;
    }


    var dataTable = _$ItemsTable.DataTable({
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
            ajaxFunction: _ItemsService.getAll,
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
                data: 'itemCode'
            },
            {
                orderable: false,
                targets: 2,
                data: "name",
                render: function (data, type, row, meta) {
                    return `
                            <a class='itemsviewfunc'  data-objid='` + row.inventoryId + `' href='javascript:void(0);'>` + row.name + ` </a>`;
                }
            },
            //{
            //    orderable: false,
            //    targets: 3,
            //    data: "entryPrice"
            //},
            {
                orderable: false,
                targets: 3,
                data: "quantity"
            },
            //{
            //    orderable: false,
            //    class: 'text-center',
            //    targets: 5,
            //    data: "stauts",
            //    render: function (data, type, row, meta) {
            //        if (row.stauts == true) {
            //            return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed">Còn Hàng</span>`
            //        } else {
            //            return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed">Hết hàng</span>`
            //        }
            //    }
            //},
            {

                targets: 4,
                data: 'id',
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    console.log(row)
                    return `<div class='text-right'> 
                                <a class='doceditfunc text-right' data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                            </div>`;
                }
            },

        ]
    });

    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#ItemsTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#ItemsTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#ItemsTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }

        if (!this.checked) {
            var el = $('#example-select-all').get(0);
            console.log(el)
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
                            var dataLocaDelete = {};
                            var dataInvenDelete = {};
                            dataLocaDelete.itemId = $(value).val();
                            dataLocaDelete.warehouseId = $("#WarehouseId").val();

                            dataInvenDelete.itemId = $(value).val();
                            dataInvenDelete.wareHouseId = $("#WarehouseId").val();

                            _inventoryService.deleteByHa(dataInvenDelete).done(function () { })
                            _warehouseLocation.delete(dataLocaDelete).done(function () {
                                $('#example-select-all').prop('checked', false);
                                abp.notify.success(app.localize('Xóa đơn hàng hóa thành công'));
                                getDocs();
                            })
                            /* getUsers(true);*/
                                
                            //_ItemsService.delete(
                            //    $(value).val()
                            //).done(function () {
                            //    // getUsers(true);
                            //    $('#example-select-all').prop('checked', false);
                            //    abp.notify.success(app.localize('Xóa đơn hàng hóa thành công'));
                            //    getDocs();
                            //});
                        }
                        // If checkbox doesn't exist in DOM
                        // If checkbox is checked
                        console.log($(value).val())

                    }
                    );

                }
            });
    })

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#ItemsTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid, warehouseId : $('#WarehouseId').val() };
        _EditModal.open(dataFilter);
    });
    $('#ItemsTable').on('click', '.itemsviewfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Items/Detail?id=" + results.id + "&WareHouseId=" + $('#WarehouseId').val();
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