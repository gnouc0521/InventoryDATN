(function () {
  //  var _$DoneStaffTable = $('#DoneStaffTable');
    var _$StaffTable = $('#StaffTable');
    var _orderService = abp.services.app.order;
    var _orderDetail = abp.services.app.ordersDetail;
    var _purchaseAssignment = abp.services.app.purchaseAssignmentService;
    var _useworkcount = abp.services.app.userWorkCountService;
    moment.locale(abp.localization.currentLanguage.name);


    var _sendMailModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/MyWork/Sendmail',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/MyWork/Sendmail.js',
        modalClass: 'SendMailModal',
        modalType: 'modal-xl'
    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/MyWork/UpdateQuantity',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/MyWork/_EditQuantityModal.js',
        modalClass: 'MyWorkEditModal',
        modalType: 'modal-xl'
    });
    $('#SendmailNCC').click(function () {
        _sendMailModal.open();
    });

    
    var dataTable = _$StaffTable.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": true,
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

        pageLength: 5,
        listAction: {
            ajaxFunction: _orderDetail.getItemMission,
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
                orderable: true,
                targets: 1,
                data: "orderCode",
                render: function (data, type, row, meta) {
                    return `<div class=''> 
                                <a class='doceditfunc' data-objid='` + row.id + `'href='javascript:void(0); ' > ` + row.orderCode + ` </a>
                            </div>`;
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "contractCode"
            },
            {
                orderable: false,
                targets: 3,
                data: "supplierName"
            },
            {
                orderable: false,
                targets: 4,
                data: 'creationTime',
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: false,
                targets: 5,
                data: "note",
            },
            {
                orderable: false,
                targets: 6,
                data: "orderStatus",
                className: "align-middle text-center",
                render: function (data, type, row, meta) {
                    if (row.orderStatus == 1) {
                        return `<span class="span_status span-defaut"> Chưa xử lý </span>`
                    } else if (row.orderStatus == 0) {
                        return `<span class="span_status span-approve"> NCC Xác nhận </span>`
                    } else if (row.orderStatus == 2) {
                        debugger
                        row.userId = row.userId
                        row.workStatus = 0;
                        _useworkcount.updateUserId(row);

                        return `<span class="span_status span-approve"> Đã hoàn thành </span>`
                    } 
                    else if (row.orderStatus == 3) {
                        return `<span class="span_status span-reject"> Sắp đến hạn </span>`
                    }
                    else {
                        return `<span class="span_status span-reject"> Qúa hạn </span>`
                    }
                }
            },
        ],
    });


    $('#StaffTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Items/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/MyWork/OrderDetail?id=" + results.id;
            },
        });
    });
    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });
 
    function getDocs() {
        dataTable.ajax.reload();

    }

   

})(jQuery);
