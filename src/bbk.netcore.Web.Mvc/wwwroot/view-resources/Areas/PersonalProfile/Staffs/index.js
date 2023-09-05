(function ($) {
    var _personService = abp.services.app.personalProfile,
        _$table = $('#StaffsTable'),
        _orgId = null;

    var _$staffTable = _$table.DataTable({
        paging: true,
        processing: true,     
        serverSide: true,
        pageLength: 25,
        responsive: true,
        language: {
            info: "Hiển thị _START_ đến _END_ trong số _TOTAL_ kết quả",
            sLengthMenu: "Hiển thị _MENU_ kết quả",
            emptyTable: "Không có dữ liệu hiển thị"
        },
        order: [],
        ajax: function (data, callback, settings) {
            $('.dataTables_filter input').attr("placeholder", "Nhập tên nhân viên");
            var filter = {
                keyword: $('.dataTables_filter input').val(),
                maxResultCount: data.length,
                skipCount: data.start,
                organizationUnitId: _orgId
            };
            abp.ui.setBusy(_$table);
            abp.ui.setBusy($('#js_nested_list'));
            _personService.getAllFilterUserId(filter).done(function (result) {
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
            }).always(function () {
                abp.ui.clearBusy(_$table);
                abp.ui.clearBusy($('#js_nested_list'));
            });
        },
        columnDefs: [
            {
                targets: 0,
                className: 'index',
                sortable: false,
                defaultContent: ''
            },
            {
                targets: 1,
                data: 'fullName',
                sortable: false,
                render: (data, type, row, meta) => {
                    return `<a href="/PersonalProfile/Staffs/Detail/${row.id}">${row.fullName}</a>`
                }
            },
            {
                targets: 2,
                data: 'position',
                sortable: false
            },
            {
                targets: 3,
                data: 'age',
                sortable: false,
            },
            {
                targets: 4,
                sortable: false,
                data: 'civilServant',
                defaultContent: '',
            },
            {
                targets: 5,
                data: 'coefficientsSalary',
                sortable: false,
            },
            {
                targets: 6,
                data: 'startedDate',
                defaultContent: '',
                sortable: false,
                render: (data) => {
                    if (data != null) {
                        return moment(data).format("DD/MM/YYYY");
                    }
                    else {
                        return "";
                    }
                }
            }
        ]
    });

    _$staffTable.on('draw.dt', function () {
        var PageInfo = _$table.DataTable().page.info();
        _$staffTable.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });

    $('#js_nested_list').on('click', '#collapse-item', function () {
        $(this).parent().find('.org-list-menu').slideToggle();
    });

    $('.org-list-menu').on('click', '#org-menu-item', function () {
        $("[id^='collapse-menu']").removeClass('active');
        $(this).parent().parent().addClass('active');
        _orgId = $(this).attr('data-id');
        $('#form-title').html($(this).attr('data-name'));
        $('.org-list-menu li.active').removeClass('active');
        $(this).addClass('active');
        _$staffTable.ajax.reload();
    });
})(jQuery);
