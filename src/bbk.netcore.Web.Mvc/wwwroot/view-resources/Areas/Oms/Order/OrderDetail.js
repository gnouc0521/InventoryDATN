(function () {
    var _$OrderDetailTable = $("#OrderDetailTable");
    var _orderService = abp.services.app.order;
    moment.locale(abp.localization.currentLanguage.name);


    var getFilterQuoteApprove = function () {
        let dataFilter = {};
        dataFilter.id = $('#Id').val();
        return dataFilter;
    }

    var ContractdataTable = _$OrderDetailTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": false,
        searching: false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,

        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _orderService.getAllDetail,
            inputFilter: getFilterQuoteApprove
        },
        order: [[0, 'asc']],
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
                data: "code",
                render: function (data, type, row, meta) {
                    return `<div class=''> 
                                <span class='doceditfunc' data-objid='` + row.id + `'href='javascript:void(0); ' > ` + row.itemName + ` </span>
                            </div>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "specifications",

            },
            {
                targets: 3,
                orderable: false,
                autoWidth: false,
                data: 'supplierName',

            },
            {

                targets: 4,
                data: 'unitName',
                orderable: false,
                autoWidth: false,
            },
            {

                targets: 5,
                data: 'quantityQuote',
                orderable: false,
                autoWidth: false,
            },
            {

                orderable: false,
                targets: 6,
                data: 'quotePrice',
            }, {

                orderable: false,
                targets: 7,
                data: null,
                render: function (data, type, row, meta) {
                    var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
                    return numFormat(parseInt(row.quantityQuote, 10) * parseInt(row.quotePrice, 10));
                }
            }, {

                orderable: false,
                targets: 8,
                data: 'note',
            }

        ],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            // Total over all pages
            total = api
                .column(6)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b.quantityQuote * b.quotePrice);
                }, 0);

            pageTotal = api
                .column(7, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    var b = b.quantityQuote * b.quotePrice
                    console.log(a, b.quantityQuote, b.quotePrice)
                    console.log(intVal(b))
                    debugger
                }, 0)
            // Total over this page
            pageTotal = api
                .column(7, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    var c = b.quantityQuote * b.quotePrice
                    return intVal(a) + intVal(c);
                }, 0);
            var numFormat = $.fn.dataTable.render.number(',', ',', '').display;
            $('tr:eq(0) td:eq(1)', api.table().footer()).html(numFormat(pageTotal));
            $('tr:eq(1) td:eq(1)', api.table().footer()).html(numFormat(pageTotal * 0.1));
            $('tr:eq(2) td:eq(1)', api.table().footer()).html(numFormat((pageTotal * 0.1) + pageTotal));
        },
    });

    abp.event.on('app.reloadgetDocContact', function () {
        getDocContact();
    });

    function getDocContact() {
        ContractdataTable.ajax.reload();
    }




    $("#btnHoanThanh").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        abp.libs.sweetAlert.config = {
            confirm: {
                icon: 'warning',
                buttons: ['Không', 'Có']
            }
        };
      
           
            abp.message.confirm(
                app.localize('Xác nhận'),
                app.localize('Bạn có chắc không'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        dataFilter.status = 0;
                        dataFilter.id = btnClick[0].dataset.action;
                        _orderService.update(dataFilter).done(function () { })
                        abp.notify.success(app.localize('Xác nhận đơn hàng thành công'));
                        getDocContact();
                        $("#btnHoanThanh").prop("hidden", true);
                    }
        });
    })

    ////---------------------------------------------------  / Bảng Danh Sách trình Báo Giá ĐƯỢC DUYỆT ------------------------


})(jQuery);