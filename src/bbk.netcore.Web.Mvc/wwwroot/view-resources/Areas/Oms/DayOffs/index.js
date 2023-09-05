(function () {
    var _$dayOffTable = $('#dayOffTable');
    var _dayOffService = abp.services.app.dayOff;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/DayOff/CreateDayOff',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/DayOffs/_CreateModal.js',
        modalClass: 'DayOffCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/DayOff/EditDayOffModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/DayOffs/_EditModal.js',
        modalClass: 'DayOffEditModal',
        modalType: 'modal-xl'
    });

    $('#CreateNewButtonxx').click(function () {
        _createModal.open();
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        dataFilter.searchTerm = $('#SearchTerm').val();
        return dataFilter;
    }


    var dataTable = _$dayOffTable.DataTable({
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
            ajaxFunction: _dayOffService.getAll,
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
                                    <a class='dropdown-item docdeletefunc'  data-objid='` + row.id + `' href='javascript:void(0);'> Xóa </a>
                            	</div>
                            </div>`;
                }
            },
            //{
            //    targets: 1,
            //    orderable: false,
            //    className: 'dt-body-center text-center',
            //    render: function (data, type, row, meta) {
            //        return '<input type="checkbox" name="" value="' + row.id + '">';
            //    }
            //},
            {
                orderable: true,
                targets: 1,
                render: function (data, type, row, meta) {
                    var order = meta.row + 1;
                    return '<span>' + order +'</span>';
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "title"
            },
            {
                orderable: false,
                targets: 3,
                data: "startDate",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                targets: 4,
                data: "endDate",
                render: function (creationTime) {
                    return moment(creationTime).format('L');
                }
            },
            {
                orderable: true,
                targets: 5,
                data: "sumDayOff",
            },
            {
                orderable: true,
                targets: 6,
                data: "typeDayOff",
                render: data => `<span>${data == 0 ? 'Nghỉ thường' : data == 1 ? 'Nghỉ phép' : 'Nghỉ lễ'}</span>`
            },


        ]
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#dayOffTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });

    $('#dayOffTable').on('click', '.docdeletefunc', function (e) {
        var btnClick = $(this);
        var dataFilter = btnClick[0].dataset.objid;
        
        deleteDayOff(dataFilter);
    });

    function deleteDayOff(DayOffID) {
        abp.message.confirm(
            app.localize('Xóa Ngày nghỉ'),
            app.localize('Bạn có chắc không'),
            (isConfirmed) => {
                if (isConfirmed) {
                    _dayOffService
                        .delete(DayOffID)
                        .done(() => {
                            abp.notify.success(app.localize('Xóa Ngày nghỉ thành công'));
                            getDocs();
                        });
                }
            }
        );
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