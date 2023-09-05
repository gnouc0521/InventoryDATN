(function () {
    var _$itemTable = $('#ItemTable');
    var impsub = abp.services.app.importRequestSubidiaryService;
    var impdSup = abp.services.app.importRequestDetailSubidiaryService;


    var _createModalReject = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ImportRequestSubsidiary/EditModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ImportRequestDetailSubsidiarys/_CreateModalContractReject.js',
        modalClass: 'CreateRejectModal',
        modalType: 'modal-xl'

    });

    var getFilter = function () {
        let dataFilter = {};
        dataFilter.importRequesSubtId = $('#Id').val().trim();
        return dataFilter;
    }


    var DataTable = _$itemTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "zeroRecords": "Không tìm thấy dữ liệu",
            searchPlaceholder: "Tìm kiếm"
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
            ajaxFunction: impdSup.getAll,
            inputFilter: getFilter
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
                orderable: false,
                targets: 1,
                data: "itemcode"
            },
            {
                orderable: false,
                targets: 2,
                data: "price"
            },
            {
                orderable: false,
                targets: 3,
                data: "quantity"
            },

            {
                orderable: false,
                targets: 4,
                data: "timeNeeded",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 5,
                data: "unitName"
            },
        ]
    });
    $("#SendTo").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.importStatus = 1;
        dataFilter.id = btnClick[0].dataset.obj;
        dataFilter.email = $("#email").val();
        dataFilter.link = window.location.href;
        dataFilter.name = $("#name").val();
        impsub.updateStatus(dataFilter).done(function () {
            abp.notify.info('Gửi phiếu yêu cầu nhập kho thành công!');
            $("#SendTo").prop("hidden", true);
        })
    })

    $(".btn-browse").click(function () {
        var btnClick = $(this);
        let dataFilter = {};
        dataFilter.importStatus = 2;
        dataFilter.id = btnClick[0].dataset.quo;
        dataFilter.link = window.location.href;
        impsub.updateStatus(dataFilter).done(function () {
            abp.notify.info('Phê duyệt phiếu yêu cầu nhập thành công!');
            $(".btn-browse").prop("hidden", true);
            $(".btn-reject").prop("hidden", true);
        })
    })

    $(".btn-reject").click(function () {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.quo };
        _createModalReject.open(dataFilter, function (callback) {
            btnClick.hide();
            $(".btn-browse").hide()

        });
    });

    function getDocs() {

        DataTable.ajax.reload();
    }
    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });
})(jQuery);