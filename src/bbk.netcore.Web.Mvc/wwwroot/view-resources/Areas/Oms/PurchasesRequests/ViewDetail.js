(function () {
    var _$itemTable = $('#ItemTable');
    var _printTable = $('#PrintTable')
    var _purchasesRequestService = abp.services.app.purchasesRequestsService;
    var _purchasesRequestsdetail = abp.services.app.purchasesRequestDetailService;
    var _subsidiaryService = abp.services.app.subsidiaryService;
    var _print = new app.ModalManager({
        viewUrl: abp.appPath + 'PersonalProfile/ImportRequest/Print',
        modalClass: 'PrintModal',
        modalType: 'modal-xl'
    });


    var getFilter = function () {
        let dataFilter = {};
        var url = window.location.href;
        var id = url.substring(url.lastIndexOf('=') + 1);
        dataFilter.purchasesRequestId = $('#Id').val();
        return dataFilter;
    }


    //------------------------- Load Name Address -----------------
    fullPathProvince = 'province.json';
    fullPathDistrict = 'district.json';
    fullPathVillage = 'village.json';

    function LoadAddress(filePath, idAddress, idSet, divview) {
        _subsidiaryService.getAddress(filePath, idAddress).done((result) => {
            for (let i = 0; i < result.addresses.length; i++) {
                if (result.addresses[i].id == idSet) {
                    $(divview).html(result.addresses[i].name);
                }
            }
        })
    }

    LoadAddress(fullPathProvince, "", $("#CityId").val(), "#CityName");
    LoadAddress(fullPathDistrict, $("#CityId").val(), $("#DistrictId").val(), "#DistrictName");
    LoadAddress(fullPathVillage, $("#DistrictId").val(), $("#WardsId").val(), "#VillageName");

    var dataTable = _$itemTable.DataTable({
        paging: false,
        serverSide: false,
        processing: true,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": false,
        //"dom": 'Rltip',
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
     
        pageLength: 10,
        listAction: {
            ajaxFunction: _purchasesRequestsdetail.getAll,
            inputFilter: getFilter
        },

        columnDefs: [
            {
                orderable: false,
                targets: 0,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                orderable: false,
                targets: 1,
                data: "nameItem"
            },
            {
                orderable: false,
                targets: 2,
                data: "nameNCC"
            },
            {
                orderable: false,
                targets: 3,
                data: "nameUnit"
            },
            {
                orderable: false,
                targets: 4,
                data: "quantity",
                 render: $.fn.dataTable.render.number(',', ',','')
            },
            {
                orderable: false,
                targets: 5,
                data: "uses",
            },
            {
                orderable: false,
                targets: 6,
                data: "timeNeeded",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 7,
                data: "note",
            },
        ],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            // Total over all pages
            total = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            pageTotal = api
                .column(4, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            // Update footer
            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
            $(api.column(4).footer()).html(numFormat(pageTotal));
        },
    });

})(jQuery);