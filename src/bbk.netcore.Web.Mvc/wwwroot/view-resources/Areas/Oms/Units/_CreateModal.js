(function ($) {

    app.modals.UnitsCreateModal = function () {


        var _unitService = abp.services.app.unitService;
        var _modalManager;
        var _frmDelivery = null;
        var callback = {};
        

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
            console.log("aaaaaaaaaaaaa");
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                console.log(_frmDelivery[0])
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmDelivery.serializeFormToObject();

            
            if (data.ParrentId == 0) {
                data.ParrentId = null
            }
            _modalManager.setBusy(true);
            _unitService.create(data)
                .done(function (result) {
                    console.log(result)
                    callback.id = result;
                    callback.name = data.Name;
                    callback.description = data.Description;
                    callback.parrentId == data.ParrentId
                    _modalManager.close();
                    abp.notify.info('Thêm mới thành công!');
                    abp.event.trigger('app.reloadDocTable');

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);