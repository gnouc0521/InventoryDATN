(function ($) {
    app.modals.EditWarModal = function () {


        var _warehouseItemsService = abp.services.app.warehouseItem
        var _modalManager;
        var _frmWarehouseItem = null;

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmWarehouseItem = _modalManager.getModal().find('form[name=FrmEdit]');

            $("#Description").summernote({ height: 270, forcus: true });

            var unitid = $("#UnitIdValue").val();
            var category = $("#CategoryCodeValue").val();
            $('[name=UnitId]').val(unitid).trigger('change');
            $('[name=CategoryCode]').val(category).trigger('change');
        }
        this.save = function () {
            // validation: jquery validate
            _frmWarehouseItem.addClass('was-validated');
            if (_frmWarehouseItem[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            var url = window.location.href;
            var id = url.substring(url.lastIndexOf('=') + 1);

            var markupDescription = $("#Description").summernote("code");
            var data = _frmWarehouseItem.serializeFormToObject();
            data.WarehouseId = id;
            data.Description = markupDescription;

            _modalManager.setBusy(true);
            _warehouseItemsService.update(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Cập nhật thành công!');
                    data.Id = result;
                    _modalManager.setResult(data);
                    if (data.ParrentId != 0) {
                        _warehouseItemsService.updateCount(result);
                    }
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);