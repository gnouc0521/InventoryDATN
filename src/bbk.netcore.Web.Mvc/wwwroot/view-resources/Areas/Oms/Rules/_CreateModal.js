(function ($) {

    app.modals.RulesCreateModal = function () {


        var _rulesService = abp.services.app.rulesService;
        var _modalManager;
        var _frmDelivery = null;
        var dataDaysOff = [];
        

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');
            document.querySelectorAll('input[type="number"]').forEach(input => {
                input.oninput = () => {
                    if (input.value.length > input.maxLength) input.value = input.value.slice(0, input.maxLength);
                }
            })
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

            console.log("aaaaaaaaaaaaa",data);
            _modalManager.setBusy(true);
            _rulesService.create(data)
                .done(function () {
                    _modalManager.close();
                    abp.notify.info('Thêm mới loại kho thành công!');
                    abp.event.trigger('app.reloadDocTable');

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);