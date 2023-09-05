(function ($) {

    app.modals.CreateRejectModal = function () {


        var _exportRequests = abp.services.app.exportRequests;
        var _modalManager;
        var _frmDelivery = null;
        var callback = {};


        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=FrmCreateReject]');

            $(document).ready(function () {
                $("#Comment").summernote({
                    height: 270,
                    forcus: true,
                });
            });




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
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }


            var markupCommnet = $("#Comment").summernote("code");
            var data = _frmDelivery.serializeFormToObject();
            data.status = 2;
            data.Comment = markupCommnet;
            data.link = window.location.href;
            _modalManager.setBusy(true);
            _exportRequests.updateStatus(data).done(function () {
                _modalManager.close();
                abp.notify.info('Từ chối thành công!');
                abp.event.trigger('app.reloadDocTable');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);