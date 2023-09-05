(function () {
    var _$expertTable = $('#ExpertTable');
    var _expertService = abp.services.app.expert;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Expert/CreateExpert',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Experts/_CreateModal.js',
        modalClass: 'ExpertCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Expert/EditExpectModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Experts/_EditModal.js',
        modalClass: 'ExpertEditModal',
        modalType: 'modal-xl'
    });

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val().trim();
        return dataFilter;
    }


    var dataTable = _$expertTable.DataTable({
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
            ajaxFunction: _expertService.getAll,
            inputFilter: getFilter
        },

        columnDefs: [
            {

                targets: 0,
                data: null,
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                            	</div>
                            </div>`;
                }
            },
            //{
            //    orderable: true,
            //    targets: 1,
            //    render: function (data, type, row, meta) {
            //        console.log(typeof (meta.row));
            //        var order = meta.row + 1;
            //        return '<span>' + order + '</span>';
            //    }
            //},
            {
                orderable: false,
                targets: 1,
                data: "expertCode"
            },
            {
                orderable: false,
                targets: 2,
                data: "name",
                render: function (data, type, row, meta) {
                    return `
                            <a class='expertviewfunc'  data-objid='` + row.id + `' href='javascript:void(0);'>` + row.name + ` </a>`;
                }
            },
            {
                orderable: false,
                targets: 3,
                data: "address",
            },
            {
                orderable: false,
                targets: 4,
                data: "phoneNumber",
            },
            {
                orderable: false,
                targets: 5,
                data: "email",
            },

        ]
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#ExpertTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });

    $('#ExpertTable').on('click', '.expertviewfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/Expert/OverView?Id=" + dataFilter.id,
            success: function (results) {
                window.location.href =
                    "/Inventorys/Expert/ViewDetail?Id=" + results.id + "";
            },
        });
    });

    //$('#dayOffTable').on('click', '.docdeletefunc', function (e) {
    //    var btnClick = $(this);
    //    var dataFilter = btnClick[0].dataset.objid;

    //    deleteDayOff(dataFilter);
    //});

    //function deleteDayOff(DayOffID) {
    //    abp.message.confirm(
    //        app.localize('Xóa Ngày nghỉ'),
    //        app.localize('Bạn có chắc không'),
    //        (isConfirmed) => {
    //            if (isConfirmed) {
    //                _dayOffService
    //                    .delete(DayOffID)
    //                    .done(() => {
    //                        abp.notify.success(app.localize('Xóa Ngày nghỉ thành công'));
    //                        getDocs();
    //                    });
    //            }
    //        }
    //    );
    //}

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