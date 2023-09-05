(function ($) {
    app.modals.CreatWarModal = function () {

        var _warehouseItemsService = abp.services.app.warehouseItem
        var _modalManager;
        var _frmWarehouseItem = null;


        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmWarehouseItem = _modalManager.getModal().find('form[name=frmCreate]');

            $("#Description").summernote({ height: 270, forcus: true });

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
            data.ParrentId = 0;
            data.WarehouseId = id;
            data.WarehouseLevel = 1;
            /*data.Code = "tesst";*/
            data.Description = markupDescription;
            _modalManager.setBusy(true);
            _warehouseItemsService.create(data)
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