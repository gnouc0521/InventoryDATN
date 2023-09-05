(function ($) {
    moment.locale(abp.localization.currentLanguage.name);
    var lstContractExpiration = null;
    var lstReappointed = null;
    var lstSalaryIncrease = null;
    var lstRetirement = null;

    var _fluctuationService = abp.services.app.fluctuation;

    _fluctuationService.getContractExpiration().done(function (result) {
        lstContractExpiration = result;
        renderFluctuationTable(lstContractExpiration);
        $('#ContractExpirationCount').text(`+${lstContractExpiration.length}`);
    });

    _fluctuationService.getReappointed().done(function (result) {
        lstReappointed = result;
        $('#ReappointedCount').text(`+${lstReappointed.length}`);
    });

    _fluctuationService.getSalaryIncrease().done(function (result) {
        lstSalaryIncrease = result;
        $('#SalaryIncreaseCount').text(`+${lstSalaryIncrease.length}`);
    });

    _fluctuationService.getRetirement().done(function (result) {
        lstRetirement = result;
        $('#RetirementCount').text(`+${lstRetirement.length}`);
    });


    function renderFluctuationTable(data) {
        let _$body = $('#FluctuationTable tbody');
        _$body.empty();
        for (let i = 0; i < data.length; i++) {
            let row =
                `<tr role="row">
                    <td>${i+1}</td>
                    <td>${data[i].fullName || ""}</td>
                    <td>${data[i].workingTitle || ""}</td>
                    <td>${data[i].organ || ""}</td>
                    <td>${data[i].decisionNumber || ""}</td>
                    <td>${moment(data[i].toDate).format('L') || ""}</td>
                    <td>${data[i].fluctuationTypeString || ""}</td>
                </tr>`
            _$body.append(row);
        }
    }

    $("#FluctiationContractExpiration").click(function () {
        renderFluctuationTable(lstContractExpiration);
    });

    $("#FluctiationReappointed").click(function () {
        renderFluctuationTable(lstReappointed);
    });

    $("#FluctiationSalaryIncrease").click(function () {
        renderFluctuationTable(lstSalaryIncrease);
    });

    $("#FluctiationRetirement").click(function () {
        renderFluctuationTable(lstRetirement);
    });
})(jQuery);
