(function ($) {

    app.modals.DayOffCreateModal = function () {


        var _dayOffService = abp.services.app.dayOff;
        var _modalManager;
        var _frmDelivery = null;
        var dataDaysOff = [];
        
        

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');
            //if (jQuery().datepicker) {
            //    $('.date-picker').datepicker({
            //        rtl: false,
            //        dateFormat: 'dd-mm--yy',
            //        orientation: "left",
            //        autoclose: true,
            //        datesDisabled: datesForDisable,
            //        language: abp.localization.currentLanguage.name,

            //    });
            //}

            _dayOffService.getAllDayOffsIf().done(function (result) {
                $.each(result, function (index, value) {
                    dataDaysOff.push(value);
                })

                $('.date-picker').datepicker({
                    rtl: false,
                    format: 'dd/mm/yyyy',
                    orientation: "left",
                    autoclose: true,
                    datesDisabled: dataDaysOff,
                    language: abp.localization.currentLanguage.name,

                });

                $("#StartDate").datepicker({
                    todayBtn: 1,
                    autoclose: true,
                }).on('changeDate', function (selected) {
                    var minDate = new Date(selected.date.valueOf());
                    $('#EndDate').datepicker('setStartDate', minDate);

                });

                $("#EndDate").datepicker()
                    .on('changeDate', function (selected) {
                        var maxDate = new Date(selected.date.valueOf());
                        $('#StartDate').datepicker('setEndDate', maxDate);
                    });
                
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
            let idType = document.getElementById("selectColor");
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmDelivery.serializeFormToObject();
            data.TypeDayOff = idType.options[idType.selectedIndex].value;
            _modalManager.setBusy(true);
            _dayOffService.create(data)
                .done(function () {
                    _modalManager.close();
                    abp.notify.info('Thêm mới thành công!');
                    abp.event.trigger('app.reloadDocTable');

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);