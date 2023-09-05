(function ($) {
    var _statisticService = abp.services.app.statistic,
        _$table = $('#StatisticsTable');
    var $age = $("#filter-age"),
        ionskin = "flat",
        from = 18,
        to = 65;
    $age.ionRangeSlider(
        {
            skin: ionskin,
            type: "double",
            grid: true,
            min: 18,
            max: 100,
            from: 18,
            to: 65,
            prefix: "tuổi ",
            onLoad: function (data) {
                from = data.from;
                to = data.to;
            },
            onChange: function (data) {
                from = data.from;
                to = data.to;
            },
        });

    var _$statisticTable = _$table.DataTable({
        paging: true,
        processing: true,
        serverSide: true,
        responsive: true,
        searching: false,
        pageLength: 25,
        language: {
            info: "Hiển thị _START_ đến _END_ trong số _TOTAL_ kết quả",
            sLengthMenu: "Hiển thị _MENU_ kết quả",
            emptyTable: "Không có dữ liệu hiển thị"
        },
        order: [],
        ajax: function (data, callback, settings) {
            var filter = $('#search-form').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            filter.minAge = from;
            filter.maxAge = to;
            filter.position = $('#position').val();
            filter.politicsTheoReticalLevel = $('#politicsTheoReticalLevel').val();
            filter.academicTitle = $('#academicTitle').val();
            filter.allowance = $('#allowance').val();
            abp.ui.setBusy(_$table);
            _statisticService.getAll(filter).done(function (result) {
                dataStatistics = result.items;
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
                $('#StatisticsTable_wrapper .justify-content-start').html("Tổng số: " + result.totalCount);
            }).always(function () {
                abp.ui.clearBusy(_$table);
            });
        },
        columnDefs: [
            {
                targets: 0,
                className: 'index',
                defaultContent: '',
                sortable: false,
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
                defaultContent: '',
                sortable: false
            },
            {
                targets: 3,
                data: 'age',
                defaultContent: '',
                sortable: false,
            },
            {
                targets: 4,
                data: 'civilServant',
                sortable: false,
                defaultContent: '',
            },
            {
                targets: 5,
                sortable: false,
                data: 'coefficientsSalary',
                defaultContent: '',
            },
            {
                targets: 6,
                data: 'recruitmentDate',
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
            },
        ]
    });

    _$statisticTable.on('draw.dt', function () {
        var PageInfo = _$table.DataTable().page.info();
        _$statisticTable.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });

    $(document).on('click', '.search-click', function (e) {
        e.preventDefault();
        Search();
    });

    $('#ExportExcel').click(function () {
        var filter = $('#search-form').serializeFormToObject(true);
        filter.minAge = from;
        filter.maxAge = to;
        filter.position = $('#position').val();
        filter.politicsTheoReticalLevel = $('#politicsTheoReticalLevel').val();
        filter.academicTitle = $('#academicTitle').val();
        filter.allowance = $('#allowance').val();
        abp.ui.setBusy(_$table);
        _statisticService.exportStatistic(filter)
            .done(function (fileResult) {
                location.href = abp.appPath + 'File/DownloadReportFile?fileType=' + fileResult.fileType + '&fileToken=' + fileResult.fileToken + '&fileName=' + fileResult.fileName;
                abp.ui.clearBusy(_$table);
            });
    });

    function Search() {
        _$statisticTable.ajax.reload();
    }
})(jQuery);