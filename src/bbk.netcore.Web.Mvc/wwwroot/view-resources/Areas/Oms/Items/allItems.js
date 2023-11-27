(function () {
    var _$ItemsTable = $('#ItemsTable');
    var _ItemsService = abp.services.app.itemsService;
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
        var dataFilter = { wareHouseId: $('#WarehouseId').val() };
        _createModal.open(dataFilter);
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val().trim();
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
            ajaxFunction: _ItemsService.getAllItems,
            inputFilter: getFilter
        },
        order: [[1, 'asc']],
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
                            <a class='itemsviewfunc'  data-objid='` + row.id + `' href='javascript:void(0);'>` + row.name + ` </a>`;
                }
            },
          
            {
                orderable: false,
                targets: 3,
                data: "supplierName"
            },
            {

                targets: 4,
                data: 'id',
                orderable: false,
                autoWidth: false,
                className: "text-center",
                render: function (data, type, row, meta) {
                    return `<a class='btn btn-warning doceditfunc text-white' data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>`;
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
                            _ItemsService.delete(
                                $(value).val()
                            ).done(function () {
                                // getUsers(true);
                                $('#example-select-all').prop('checked', false);
                                abp.notify.success(app.localize('Xóa hàng hóa thành công'));
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

    $('#ItemsTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid, warehouseId: $('#WarehouseId').val() };
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
                    "/Inventorys/Items/DetailItem1?Id=" + results.id;
            },
        });
    });
    function onClickHandler(ev) {
        var el = window._protected_reference = document.createElement("INPUT");
        el.type = "file";
        el.accept = "file/*";
        el.multiple = "multiple"; // remove to have a single file selection

        // (cancel will not trigger 'change')
        el.addEventListener('change', function (ev2) {
            // access el.files[] to do something with it (test its length!)

            // add first image, if available
            if (el.files.length) {
                // document.getElementById('out').src = URL.createObjectURL(el.files[0]);
                console.log(el.files[0])
                var ext = el.files[0].name.split('.').pop().toLowerCase();
                console.log(ext)
                if ($.inArray(ext, ['xlsx', 'xls']) == -1) {
                    // alert('File không hợp lệ \nVui lòng nhập lại file');
                    abp.message.error('Vui lòng chọn lại file ', 'File không hợp lệ');
                } else {
                    fodata = el.files[0];
                    console.log(fodata)
                    if (fodata != null) {
                        loadExcel();
                    }
                }
            }
            // test some async handling
            new Promise(function (resolve) {
                setTimeout(function () { console.log(el.files); resolve(); }, 1000);
            })
                .then(function () {
                    // clear / free reference
                    el = window._protected_reference = undefined;
                });

        });

        el.click(); // open
    }

    $("#btnIpItem").on("click", function () {

        onClickHandler()

        //var fileData = $("#fileupload").prop("files")[0];
        //var ext = $('#fileupload').val().split('.').pop().toLowerCase();
        //console.log(ext)
        //if ($.inArray(ext, ['xlsx', 'xls']) == -1) {
        //    alert('File không hợp lệ \nVui lòng nhập lại file');
        //} else {
        //    fodata = fileData;
        //    console.log(fodata)
        //    if (fodata != null) {
        //        loadExcel();
        //    }
        //}

    })
    function loadExcel() {
        let formData = new FormData();
        formData.append("file", fodata);
        abp.ajax({
            url: '/Inventorys/Items/ImportExcel',
            type: 'post',
            cache: false,
            contentType: false,
            processData: false,
            data: formData,
            dataType: "json",
            success: (function (response) {
            })
        }).done(function () {
            abp.notify.info('Cập nhật file báo giá thành công!');
            getDocs();
        })
    }
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