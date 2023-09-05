(function ($) {
    app.modals.ScheduleWorkEditModal = function () {


        var _schedulework = abp.services.app.scheduleWork;
        var _modalManager;
        var _frmDelivery = null;



        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

            $("#Description").summernote({ height: 270, forcus: true });
            var colorId = $("#ColorId").val();
            _schedulework.get({ id: colorId }).done(function (result) {
                console.log(result);
                $('[name=Color]').val(result.color).trigger('change');
                $('[name=StartDay]').val(result.startDay).trigger('change');
                $('[name=EndDay]').val(result.endDay).trigger('change');
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
            let idColor = document.getElementById("selectColor");

            var markupDescription = $("#Description").summernote("code");
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmDelivery.serializeFormToObject();
            data.Description = markupDescription;
            data.Repeat = null;
            /*data.Color = idColor.options[idColor.selectedIndex].value;*/

            
            _modalManager.setBusy(true);
            _schedulework.update(data)
                .done(function () {
                    _modalManager.close();
                    abp.notify.info('Cập nhật thành công!');
                    $("#calendar").empty();
                    abp.event.trigger('app.reloadCalendar');

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);