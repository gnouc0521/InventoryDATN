(function () {
    var _$supplierTable = $('#supplierTable');
    var _supplierService = abp.services.app.supplier;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Supplier/CreateSupplier',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Suppliers/_CreateModal.js',
        modalClass: 'SupplierCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Supplier/EditSupplierModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Suppliers/_EditModal.js',
        modalClass: 'SupplierEditModal',
        modalType: 'modal-xl'
    });

    var _ViewDetails = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Supplier/ViewDetails',
        modalClass: 'ViewModal',
        modalType: 'modal-xl'
    });

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val();
        return dataFilter;
    }


    var dataTable = _$supplierTable.DataTable({
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
            ajaxFunction: _supplierService.getAll,
            inputFilter: getFilter
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
                data: "code"
            },
            {
                orderable: false,
                targets: 2,
                data: "name",
                render: function (data, type, row, meta) {
                    return `
                        <a class="viewsupplier" data-viewid='` + row.id + `' href='javascript:void(0); '>` + row.name + `</a>
                    `
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "adress"
            },
            {
                orderable: true,
                targets: 4,
                data: "phoneNumber",
            },
            {
                orderable: true,
                targets: 5,
                data: "email",
            },
            {

                targets: 6,
                data: null,
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `<div class=''> 
                                <a class='dropdown-item doceditfunc' data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
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
        $('#supplierTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });


    $('#supplierTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#supplierTable tbody input[type="checkbox"]:checked').each(function () {
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
                            _supplierService
                                .delete(
                                    $(value).attr("data-value")
                                ).done(function () {
                                    // getUsers(true);
                                    $('#example-select-all').prop('checked', false);
                                    abp.notify.success(app.localize('Xóa nhà sản xuất thành công'));
                                    getDocs();
                                });
                        }
                    }
                    );

                }
            });
    })





    $('#supplierTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });


    //$('#supplierTable').on('click', '.viewsupplier', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = { id: btnClick[0].dataset.viewid };
    //    _ViewDetails.open(dataFilter);
    //});


    $('#supplierTable').on('click', '.viewsupplier', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.viewid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Supplier/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Supplier/ViewDetails?id=" + results.id;
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