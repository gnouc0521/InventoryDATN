(function () {
    var _$warehouseLocationTable = $('#ViewItemsTable');
    var _warehouseLocationService = abp.services.app.warehouseLocationItemService;
    var _ItemsService = abp.services.app.itemsService;
    moment.locale(abp.localization.currentLanguage.name);

    $('.date-picker').datepicker({});
    for (i = new Date().getFullYear(); i > 1900; i--) {
        $('#yearpicker').append($('<option />').val(i).html(i));
    }

    console.log("aaaa",$('#WarehouseId').val());
    /* DEFINE TABLE */
    var getFilter = function () {
        let idWarehouse = document.getElementById("WarehouseId");
        let year = document.getElementById("yearpicker");

        fromDate = $("#FromDate").datepicker("getDate");
        toDate = $("#ToDate").datepicker("getDate");

        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val().trim();
        dataFilter.warehouseId = idWarehouse.options[idWarehouse.selectedIndex].value;
        dataFilter.supplierId = $('#SupplierId').val();
        dataFilter.producerId = $('#ProducerId').val();
        dataFilter.year = year.options[year.selectedIndex].value;

        if (fromDate != null) {
            dateStart = moment(fromDate).format('L');
            dataFilter.fromDate = dateStart
        }
        if (toDate != null) {
            dateEnd = moment(toDate).format('L');
            dataFilter.toDate = dateEnd;
        }
        return dataFilter;
    }


    var dataTable = _$warehouseLocationTable.DataTable({
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
            ajaxFunction: _warehouseLocationService.getAll,
            inputFilter: getFilter
        },

        columnDefs: [

            {
                targets: 0,
                width: "6%",
                orderable: false,
                className: 'dt-body-center text-center',
                render: function (data, type, row, meta) {
                    var order = meta.row + 1;
                    return '<span>' + order + '</span>';
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
                            <a class='itemsviewfunc'  data-objid='` + row.id + `' data-inventoryId='` + row.inventoryId + `' data-warehouseId='` + row.warehouseId  +`' href='javascript:void(0);'>` + row.name + ` </a>`;
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "exp",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
        ]
    });

    

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    $('#ViewItemsTable').on('click', '.itemsviewfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.inventoryid, warehouseId: btnClick[0].dataset.warehouseid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id + "&WarehouseId=" + dataFilter.warehouseId,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Items/DetailItemSearch?id=" + results.id + "&WareHouseId=" + results.warehouseId;
            },
        });
    });

    function getDocs() {

        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });


})(jQuery);