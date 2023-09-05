(function ($) {
    app.modals.WorkRootCreateModal = function () {


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
            data.ParentId = null;
            data.WorkGroupLevel = 1;
            data.Order = 10;

            console.log("aaaaaaaaaaaaa",data);
            _modalManager.setBusy(true);
            _workgroup.create(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Thêm mới thành công!');
                    //$("#tree_workGroup").empty();
                    //abp.event.trigger('app.reloadParentTree');
                    data.Id = result;
                    _modalManager.setResult(data);

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);