(function ($) {
    app.modals.WorkitemCreateModal = function () {


        var _profileWorkService = abp.services.app.profileWork;
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
           
            var workGroupLevel = parseInt($("#WorkProfileLevel").val()) + 1;
            console.log("11", workGroupLevel)
            if (workGroupLevel > 5) {
                abp.message.error('Bạn không thể tạo thêm phần con nữa!');
                event.preventDefault();
                event.stopPropagation();
                _modalManager.close();
                return;
            }
            data.Order = 10;
            data.NumItemsChild = 0;
            data.WorkProfileLevel = workGroupLevel;
            _modalManager.setBusy(true);
            _profileWorkService.create(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Thêm mới thành công!');
                    data.Id = result;
                   
                    _modalManager.setResult(data);

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);