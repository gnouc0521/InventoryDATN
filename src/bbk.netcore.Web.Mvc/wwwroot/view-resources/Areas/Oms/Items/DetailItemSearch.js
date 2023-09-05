(function () {
    var _$warehouseLocationTable = $('#ViewItemsTable');
    var _warehouseLocationService = abp.services.app.warehouseLocationItemService;
    moment.locale(abp.localization.currentLanguage.name);

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $("#ItemcodeText").text().trim();
        return dataFilter;
    }

    console.log($("#ItemcodeText").text().trim());
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
            ajaxFunction: _warehouseLocationService.getAllListItem,
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
                data: null,
                render: function (data, type, row, meta) {
                    return `
                            <span>`+ $("#ItemName").text() + `</span>`
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "warehouseName"
            },
            {
                orderable: false,
                targets: 3,
                data: "blockName"
            },
            {
                orderable: false,
                targets: 4,
                data: "quantity"
            },
            {
                orderable: false,
                targets: 5,
                data: "importDate",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 6,
                data: "expireDate",
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


    function getDocs() {

        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });

    var textma = $("#ItemcodeText").text().trim();

    console.log("sâdasdasd");
   
    //Qr Code
    var qrcode = new QRCode(document.getElementById("id-qrcode"), {
        text: "textma",
        width: 100,
        height: 100,
        colorDark: "#000000",
        colorLight: "#ffffff",
        /*correctLevel: QRCode.CorrectLevel.M*/
    });

    qrcode.makeCode(textma);

    JsBarcode("#barcode", textma);

})(jQuery);