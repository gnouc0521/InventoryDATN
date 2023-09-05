(function ($) {
    app.modals.MyWorkEditModal = function () {

        var _purchaseAssignment = abp.services.app.purchaseAssignmentService;
        moment.locale(abp.localization.currentLanguage.name);
        var _modalManager;
        var _frmDelivery = null;

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');
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
            debugger
            _modalManager.setBusy(true);
            _purchaseAssignment.update(data)
                .done(function () {
                    _modalManager.close();
                    abp.notify.info('Cập nhật thành công!');
                    abp.event.trigger('app.reloadDocTable');

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);