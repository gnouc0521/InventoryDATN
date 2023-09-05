(function ($) {
    app.modals.WorkitemCreateModal = function () {


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
            /*data.ParentId = null;*/
            var workGroupLevel = parseInt($("#WorkGroupLevel").val()) + 1;
            if (workGroupLevel > 5) {
                abp.message.error('Bạn không thể tạo thêm phần con nữa!');
                event.preventDefault();
                event.stopPropagation();
                _modalManager.close();
                return;
            }
            data.Order = 10;
            data.NumItemsChild = 0;
            data.WorkGroupLevel = workGroupLevel;

            console.log("aaaaaaaaaaaaa", data);
            _modalManager.setBusy(true);
            _workgroup.create(data)
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