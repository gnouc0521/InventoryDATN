(function ($) {
    var _assessedByYearService = abp.services.app.assessedByYear,
        _$modal = $('#Modal');
    function create() {
        var _$form = _$modal.find('form');
        _$form.validateForm();
        if (!_$form.valid()) {
            return;
        }
        var
        data = _$form.serializeFormToObject();
        data.PersonId = parseInt($("#PersonId").val());
        abp.ui.setBusy(_$form);
        _assessedByYearService.create(data)
            .done(function () {
                _$modal.modal('hide');
                abp.notify.info('Thêm mới thành công!');
                abp.event.trigger("assessedByYear.updated");
            }).always(function () {
                abp.ui.clearBusy(_$form);
            });
    }

    $(document).on('click', '#Modal .create-assessedByYears #SaveButton', function (e) {
        create();
    });
})(jQuery);