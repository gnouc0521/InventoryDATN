(function ($) {
    app.modals.DayOffEditModal = function () {

        var _dayOffService = abp.services.app.dayOff;
        var _modalManager;
        var _frmDelivery = null;
        var dataDaysOff = [];

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=FrmEdit]');

            var id = $("#Id").val();
            _dayOffService.get({ id: id }).done(function (result) {
                $('[name=selectColor]').val(result.typeDayOff).trigger('change');
            })
            //if (jQuery().datepicker) {
            //    $('.date-picker').datepicker({
            //        rtl: false,
            //        dateFormat: 'dd-mm--yy',
            //        orientation: "left",
            //        autoclose: true,
            //        language: abp.localization.currentLanguage.name
            //    });
            //}
            _dayOffService.getAllDayOffsIfById(id).done(function (result) {
                $.each(result, function (index, value) {
                    dataDaysOff.push(value);
                })

                $('.date-picker').datepicker({
                    rtl: false,
                    format: 'dd/mm/yyyy',
                    orientation: "left",
                    autoclose: true,
                    datesDisabled: dataDaysOff,
                /*    language: abp.localization.currentLanguage.name,*/

                });

                $("#StartDate").datepicker({
                    todayBtn: 1,
                    autoclose: true,
                }).on('changeDate', function (selected) {
                    var minDate = new Date(selected.date.valueOf());
                    $('#EndDate').datepicker('setStartDate', minDate);
                    /*       $('#EndDate').datepicker('setDate', minDate);*/
                });

                $("#EndDate").datepicker()
                    .on('changeDate', function (selected) {
                        var maxDate = new Date(selected.date.valueOf());
                        $('#StartDate').datepicker('setEndDate', maxDate);
                    });
            })
        }

            //hàm tính số ngày nghỉ 
            //function daysdifference(firstDate, secondDate) {
            //    var startDay = new Date(firstDate);
            //    var endDay = new Date(secondDate);

            //    var millisBetween = startDay.getTime() - endDay.getTime();
            //    var days = millisBetween / (1000 * 3600 * 24);

            //    return Math.round(Math.abs(days));
            //}

            

            

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
            _dayOffService.update(data)
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