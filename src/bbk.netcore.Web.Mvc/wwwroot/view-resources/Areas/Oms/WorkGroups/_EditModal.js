(function ($) {
    app.modals.WorkGroupEditModal = function () {


        var _workgroup = abp.services.app.workGroup;
        var _modalManager;
        var _frmDelivery = null;



        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

        }
        this.save = function () {
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmDelivery.serializeFormToObject();

            console.log("aaaaaaaaaaaaa", data);
            _modalManager.setBusy(true);
            _workgroup.update(data)
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