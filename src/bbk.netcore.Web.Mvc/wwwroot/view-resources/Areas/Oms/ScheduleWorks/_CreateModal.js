(function ($) {
    app.modals.ScheduleWorkCreateModal = function () {


        var _schedulework = abp.services.app.scheduleWork;
        var _modalManager;
        var _frmDelivery = null;



        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

            $("#Description").summernote({ height: 270, forcus: true });

            let dateInput = document.getElementById("StartDay");
            let dateInputEnd = document.getElementById("EndDay");

            dateInput.min = new Date().toISOString().slice(0, new Date().toISOString().lastIndexOf(":"));

            $("#EndDay").on('click', function () {
                if ($("#StartDay").val() == "") {
                    dateInput.min = new Date().toISOString().slice(0, new Date().toISOString().lastIndexOf(":"));
                }
                else {
                    dateInputEnd.min = new Date($("#StartDay").val()).toISOString().slice(0, new Date().toISOString().lastIndexOf(":"));
                    
                }
                
            })

            $("#StartDay").on('click', function () {
                if ($("#EndDay").val() == "") {
                    dateInput.min = new Date().toISOString().slice(0, new Date().toISOString().lastIndexOf(":"));
                
                }
                else {
                    dateInput.max = new Date($("#EndDay").val()).toISOString().slice(0, new Date().toISOString().lastIndexOf(":"));
                    
                }
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

            console.log("aaaaaaaaaaaaaaaa", idColor.options[idColor.selectedIndex].value);
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
            data.Color = idColor.options[idColor.selectedIndex].value;

            console.log("aaaaaaaaaaaaa", data);
            _modalManager.setBusy(true);
            _schedulework.create(data)
                .done(function () {
                    _modalManager.close();
                    abp.notify.info('Thêm mới thành công!');
                    $("#calendar").empty();
                    abp.event.trigger('app.reloadCalendar');

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);