(function ($) {
    var _assessedByYearService = abp.services.app.assessedByYear,
        _$modal = $('#Modal');

    function update() {
        var _$form = _$modal.find('form');      
        _$form.validateForm();
        if (!_$form.valid()) {
            return;
        }
        var data = _$form.serializeFormToObject();
          data.PersonId = parseInt($("#PersonId").val());
          abp.ui.setBusy(_$form);
         _assessedByYearService.update(data)
           .done(function () {
             abp.notify.info('Cập nhật thành công!');
            _$modal.modal('hide');
             abp.event.trigger("assessedByYear.updated");
           }).always(function () {
           abp.ui.clearBusy(_$form);
         });   
    }
    $(document).on('click', '.edit-assessedByYears #SaveButton', function (e) {
        update();
    });
})(jQuery);