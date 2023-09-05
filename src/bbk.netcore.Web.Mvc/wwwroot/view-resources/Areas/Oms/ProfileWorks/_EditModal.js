(function ($) {
    app.modals.DeliveryEditModal = function () {

        var _profileWorkService = abp.services.app.profileWork;
        var _modalManager;
        var _frmProfileWork = null;

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmProfileWork = _modalManager.getModal().find('form[name=FrmEdit]');
        }
        this.save = function () {
            // validation: jquery validate
            _frmProfileWork.addClass('was-validated');
            if (_frmProfileWork[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmProfileWork.serializeFormToObject();
            _modalManager.setBusy(true);
            _profileWorkService.update(data)
                .done(function () {
                    _modalManager.close();
                    abp.notify.info('Cập nhật thành công!');
                    _modalManager.setResult(data);

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);