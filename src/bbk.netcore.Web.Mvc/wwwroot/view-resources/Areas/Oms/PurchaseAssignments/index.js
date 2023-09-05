(function () {

    var _$AllRequestTable = $('#AllRequestTable');
    var _$AllRequestTableDone = $('#AllRequestTableDone');
    var _purchasesSynthesisService = abp.services.app.purchasesSynthesise;

    var _ViewDetails = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/PurchaseAssignment/ViewPurchaseAssignment',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/PurchaseAssignments/ViewPurchaseAssignment.js',
        modalClass: 'ViewModal',
        modalType: 'modal-xl'
    });




    var getFilter = function () {
        let dataFilter;
        //dataFilter.searchTerm = $('#SearchTerm').val();
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
            ajaxFunction: _purchasesSynthesisService.getAllDone,
            inputFilter: getFilter
        },
        'rowsGroup': {
            dataSrc: 2,
        },
        order: [[1, 'asc']],
        columnDefs: [

            {
                orderable: false,
                className: 'dt-body-center text-center',
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
                data: 'subsidiaries',
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
                data: "statusAssignment",
                render: function (data, type, row, meta) {
                    if (row.statusAssignment == true) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-approve">Đã phân công</span>`
                    }
                    else {
                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa phân công</span>`
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
                          <a class="btn btn-primary docviewfunc"  data-objid='` + row.id + `'href='javascript:void(0); ' role="button">  <span class="fa fa-edit"></span> Phân công</a>
                                `;
                }
            }
        ],
        "initComplete": function (settings, json) {
            // MergeGridCells()
        }

    });

    var AllRequestTableDone = _$AllRequestTableDone.DataTable({
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
            ajaxFunction: _purchasesSynthesisService.getAllAssignment,
            inputFilter: getFilter
        },
        'rowsGroup': {
            dataSrc: 2,
        },
        order: [[0, 'asc']],
        columnDefs: [

            {
                orderable: false,
                className: 'dt-body-center text-center',
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
                        <a class="detailview" data-objid='` + row.id + `' href='javascript:void(0); '>` + row.purchasesSynthesiseCode + `</a>
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
                data: "dateAssignment",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 5,
                className: 'dt-body-center text-center',
                data: "statusAssignment",
                render: function (data, type, row, meta) {
                    if (row.statusAssignment == true) {
                        return `<span class="btn btn-outline-secondary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-approve">Đã phân công</span>`
                    }
                    else {
                        return `<span class="btn btn-outline-primary btn-pills btn-sm btn-w-m  mr-1 waves-effect waves-themed span_status span-defaut">Chưa phân công</span>`
                    }
                }
            },

        ],
    });
    $('#AllRequestTable').on('click', '.docviewfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _ViewDetails.open(dataFilter);
    })

    $('#AllRequestTable').on('click', '.docview,.purchasesSynthesiseCode', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/PurchaseAssignment/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/PurchaseAssignment/Detail?Id=" + results.id
            },
        });
    });

    $('#AllRequestTableDone').on('click', '.detailview', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/PurchaseAssignment/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/PurchaseAssignment/DetailPurchaseAssignment?Id=" + results.id
            },
        });
    });


    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    abp.event.on('app.reloadTable', function () {
        getAlls();
    });

    function getDocs() {
        AllRequestTable.ajax.reload();

    }

    function getAlls() {
        AllRequestTableDone.ajax.reload();
    }


})(jQuery);