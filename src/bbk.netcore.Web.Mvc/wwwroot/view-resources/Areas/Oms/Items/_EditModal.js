(function ($) {
    app.modals.EditItemsModal = function () {

        var _itemsServiceService = abp.services.app.itemsService;
        var _modalManager;
        var _frmDelivery = null;

        this.init = function (modalManager) {

            $('.date-picker').datepicker({});
            for (i = new Date().getFullYear(); i > 1900; i--) {
                $('#yearpicker').append($('<option />').val(i).html(i));
            }
            $("#Remark").summernote({ height: 270, forcus: true });
            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=FrmEdit]');

            $("#CategoryCode").val($(".CategoryCode").val());
            $("#GroupCode").val($(".GroupCode").val());
            $("#KindCode").val($(".KindCode").val());
            $("#ProducerCode").val($(".ProducerCode").val());
            $("#SupplierCode").val($(".SupplierCode").val());
            $("#Unit").val($(".Unit").val());
            $("#yearpicker").val($(".MFG").val());
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
            console.log(data)
            _modalManager.setBusy(true);
            _itemsServiceService.update(data)
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