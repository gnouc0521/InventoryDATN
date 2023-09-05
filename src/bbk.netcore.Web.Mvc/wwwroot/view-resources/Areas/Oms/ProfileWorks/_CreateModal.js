(function ($) {
    app.modals.CreatDelModal = function () {

        var _profileWorkService = abp.services.app.profileWork;
        var _modalManager;
        var _frmProfileWork = null;


        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmProfileWork = _modalManager.getModal().find('form[name=frmCreate]');
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
            _frmProfileWork.addClass('was-validated');
            if (_frmProfileWork[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmProfileWork.serializeFormToObject();
            data.ParentId = null;
            data.Order = 10;
            data.WorkProfileLevel = 1;
         
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