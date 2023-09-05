
(function () {
    var _$quoteSynTable = $("#QuoteSynTable");
    var _$quoteSynTableApprove = $("#QuoteSynTableApprove");
    var _$ContractTable = $("#ContractTable");

    var _quotesService = abp.services.app.quotesService;
    var _contractService = abp.services.app.contract;
    var _quoteSynService = abp.services.app.quotesSynthesise;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/CreateContract',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModal.js',
        modalClass: 'ContractsCreateModal',
        modalType: 'modal-xl'

    });
    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Contract/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Contracts/_CreateModalReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });

    ////--------------------------------------------------- Bảng Danh Sách trình Báo Giá ------------------------
    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val().trim();
        return dataFilter;
    }


    var SyndataTable = _$quoteSynTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        searching: true,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "zeroRecords": "Không tìm thấy dữ liệu",
            searchPlaceholder: "Tìm kiếm"
        },
        "bInfo": false,
        "bLengthChange": true,

        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],

        pageLength: 10,
        listAction: {
            ajaxFunction: _quoteSynService.getAll,
        },
        order: [[0, 'asc']],
        columnDefs: [
            {
                orderable: false,
                targets: 0,
                width: "5%",
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
                                <a class='doceditfunc' data-objid='` + row.id + `'href='javascript:void(0); ' > ` + row.code + ` </a>
                            </div>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: null,
                render: function (data, type, row, meta) {
                    var html = "";
                    for (var i = 0; i < row.suppliersNames.length; i++) {
                        html += `<span>` + row.suppliersNames[i] + `</span><br>`
                    }
                    return html;
                }
            },
            {

                targets: 3,
                orderable: false,
                autoWidth: false,
                data: 'creationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                targets: 4,
                orderable: false,
                autoWidth: false,
                // data: 'QuoteSynthesiseDate',
                render: function (data, type, row, meta) {
                    if (row.status == 0) { return '' } else {
                        return moment(row.quoteSynthesiseDate).format('L');
                    }
                }
            },
            {

                targets: 5,
                data: 'status',
                orderable: false,
                autoWidth: false,
                className: "align-middle text-center",
                render: function (status) {
                    if (status == 1) {
                        return `<span class="span_status span-defaut"> Chưa xử lý </span>`
                    } else if (status == 2) {
                        return `<span class="span_status span-approve"> Đã Phê Duyệt </span>`
                    } else if (status == 3) {
                        return `<span class="span_status span-reject"> Từ chối </span>`
                    } else if (status == 4) {
                        return `<span class="span_status span-contract"> Đã tạo hợp đồng </span>`
                    }
                }
            },
            {

                orderable: false,
                targets: 6,
                data: null,
                render: function (data, type, row, meta) {
                    return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item viewdetail' data-QuoteSyn='` + row.id + `' href='javascript:void(0);'>` + app.localize('Xem chi tiết') + `</a>
                            	</div>
                            </div>`;

                },
            }
        ]
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    // xem chi tiet
    $('#QuoteSynTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Quote/CompareDetail?SynthesiseId=" + results.id;
            },
        });
    });

    $('#QuoteSynTable').on('click', '.viewdetail', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quotesyn };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Quote/CompareDetail?SynthesiseId=" + results.id;
            },
        });
    });



    $('#QuoteSynTable').on('click', '.doccreatefunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.QuoteSyn };
        _createModal.open(dataFilter);
    });


    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {
        SyndataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });
    ////--------------------------------------------------- / Bảng Danh Sách trình Báo Giá ---------------------------------

})(jQuery);