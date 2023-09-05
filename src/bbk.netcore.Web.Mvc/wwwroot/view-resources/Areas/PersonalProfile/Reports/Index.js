(function () {
    $(function () {
        $('.date-picker').datepicker({
            rtl: false,
            orientation: "left",
            autoclose: true,
            orientation: 'bottom left',
            language: abp.localization.currentLanguage.name,
        });
        var _reportService = abp.services.app.report;

        $('#exportBM01aBtn').click(function () {
            exportReport(16);
        });

        $('#exportBM02Btn').click(function () {
            exportReport(2);
        });

        $('#exportBM03Btn').click(function () {
            exportReport(3);
        });

        $('#exportBM04Btn').click(function () {
            exportReport(4);
        });

        $('#exportBM05Btn').click(function () {
            exportReport(5);
        });

        $('#exportBM07Btn').click(function () {
            exportReport(7);
        });

        $('#exportBM08Btn').click(function () {
            exportReport(8);
        });

        $('#exportBM09Btn').click(function () {
            exportReport(9);
        });

        $('#exportBM10Btn').click(function () {
            exportReport(10);
        });

        $('#exportBM12Btn').click(function () {
            exportReport(12);
        });

        $('#exportBM13Btn').click(function () {
            exportReport(13);
        });

        $('#exportBM15Btn').click(function () {
            exportReport(15);
        });

        function exportReport(reportEnum) {
            var filterObj = {
                reportEnum: reportEnum,
                orgId: $('#OrgId').val(),
                fromDate: $('#FromDate').val().toString(),
                toDate: $('#ToDate').val().toString()
            };
            _reportService.report(filterObj)
                .done(function (fileResult) {
                    location.href = abp.appPath + 'File/DownloadReportFile?fileType=' + fileResult.fileType + '&fileToken=' + fileResult.fileToken + '&fileName=' + fileResult.fileName;
                });
        }
    });
})();
