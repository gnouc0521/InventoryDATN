(function () {
    var _$wareHouseTable = $('#wareHouseTable');
    var _wareHouse = abp.services.app.wareHouse;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/WareHouse/CreateWareHouse',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/WareHouses/_CreateModal.js',
        modalClass: 'WareHouseCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/WareHouse/EditWareHouseModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/WareHouses/_EditModal.js',
        modalClass: 'WareHouseEditModal',
        modalType: 'modal-xl'
    });

    var _permissions = {

        edit: abp.auth.hasPermission('Inventorys.WareHouse.Edit'),
    };

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val();
        return dataFilter;
    }

    var dataTable = _$wareHouseTable.DataTable({
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
            ajaxFunction: _wareHouse.getAll,
            inputFilter: getFilter
        },
        order: [[1, 'asc']],
        columnDefs: [
            //{

            //    targets: 0,
            //    data: null,
            //    orderable: false,
            //    autoWidth: false,
            //    render: function (data, type, row, meta) {
            //        return `                        
            //                <div class='dropdown d-inline-block'>
            //                	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
            //                		<i class="fal fa-ellipsis-v"></i>
            //                	</a>
            //                	<div class='dropdown-menu'>
            //                		<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
            //                        <a class='dropdown-item docdeletefunc'  data-objid='` + row.id + `' href='javascript:void(0);'> Xóa </a>
            //                	</div>
            //                </div>`;
            //    }
            //},
            {
                orderable: false,
                targets: 0,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    return '<input type="checkbox" name="" value="' + row.id + '">';
                }
            },
            //{
            //    orderable: true,
            //    targets: 1,
            //    render: function (data, type, row, meta) {
            //        console.log(typeof (meta.row));
            //        var order = meta.row + 1;
            //        return '<span>' + order +'</span>';
            //    }
            //},
            {
                targets: 1,
                orderable: false,
                data: "code"
            },
            {
                orderable: false,
                targets: 2,
                data: "name",
                render: function (data, type, row, meta) {
                    return `
                            <a class='docviewfunc'  data-objid='` + row.id + `' href='javascript:void(0);'>`+ row.name +` </a>`;
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "cityName",
                render: function (data, type, row, meta) {
                    return `
                            <span>`+ row.number + ` , ` + row.wardsName + ` , ` + row.districtName +` , `+ row.cityName +`</span>`
                }
            },
            {
                targets: 4,
                orderable: false,
                data: "typeWarehouse",
                render: data => `<span>${data == 0 ? 'Đang hoạt động' : data == 1 ? 'Kiểm kê' : 'Đầy kho'}</span>`
            },
            {

                targets: 5,
                data: null,
                orderable: false,
                autoWidth: false,
                className: "text-center",
                render: function (data, type, row, meta) {
                    return `
                            <a class='btn btn-warning text-white doceditfunc'  data-objid='` + row.id + `' href='javascript:void(0);'> Sửa </a>`;
                }
            },

        ],
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    //-------------------------------------- select checkbox while Delete All --------------------------------------------------------

    $('#example-select-all').on('click', function () {
        // Get all rows with search applied
        var rows = dataTable.rows({ 'search': 'applied' }).nodes();
        // Check/uncheck checkboxes for all rows in the table
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
        $('#DeleteAll').removeAttr('disabled');
        var selected = new Array();
        $('#wareHouseTable tbody input[type="checkbox"]:checked').each(function () {
            selected.push($(this));
        });
        if (selected.length > 0) {
            $('#DeleteAll').removeAttr('disabled');
        } else {
            $('#DeleteAll').prop('disabled', true);
        }
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#wareHouseTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        var selected = new Array();
        $('#wareHouseTable tbody input[type="checkbox"]:checked').each(function () {
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
            app.localize('Xóa kho', "Kho"),
            app.localize('Bạn có chắc không'),
            function (isConfirmed) {
                if (isConfirmed) {
                    // Iterate over all checkboxes in the table
                    dataTable.$('input[type="checkbox"]').each(function (index, value) {
                        if ($(value).is(":checked")) {
                            _wareHouse.delete(
                                $(value).val()
                            ).done(function () {
                                // getUsers(true);
                                $('#example-select-all').prop('checked', false);
                                abp.notify.success(app.localize('Xóa kho thành công'));
                                getDocs();
                            });
                        }
                    }
                    );

                }
            });
    })

    //-------------------------------------- / select checkbox while Delete All --------------------------------------------------------

    //------------ Edit---------------------
    $('#wareHouseTable').on('click', '.doceditfunc', function (e) {
        if (_permissions.edit) {
            var btnClick = $(this);
            var dataFilter = { id: btnClick[0].dataset.objid };
            _EditModal.open(dataFilter);
        }
        else {
            abp.notify.error('Bạn không có quyền vào mục này');
        }
       
    });

    //----------------- View Warehouse ----------------
    $('#wareHouseTable').on('click', '.docviewfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/WareHouse/OverView?Id=" + dataFilter.id,
            success: function (results) {
                console.log(results);
                window.location.href =
                    "/Inventorys/WareHouse/ViewWareHouse?warehouseId=" + results.id ;
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