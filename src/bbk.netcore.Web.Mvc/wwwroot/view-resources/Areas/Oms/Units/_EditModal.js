(function ($) {
    app.modals.UnitsEditModal = function () {

        var _dayOffService = abp.services.app.unitService;
        var _modalManager;
        var _frmDelivery = null;
        var dataDaysOff = [];

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=FrmEdit]');

        }

        //sự kiện khi đóng modal
        $(".close-modal").on("click", function () {
            abp.libs.sweetAlert.config = {
                confirm: {
                    icon: 'warning',
                    buttons: ['Không', 'Có']
                }
            };
            abp.message.confirm(
                app.localize('Đóng'),
                app.localize('Bạn có chắc không'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _modalManager.close();
                        return true;

                    }
                }
            );
        })
        this.save = function () {
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmDelivery.serializeFormToObject();
            _modalManager.setBusy(true);
            _dayOffService.update(data)
                .done(function () {
                    _modalManager.close();
                    _modalManager.setResult(data);
                    abp.notify.info('Cập nhật thành công!');
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);