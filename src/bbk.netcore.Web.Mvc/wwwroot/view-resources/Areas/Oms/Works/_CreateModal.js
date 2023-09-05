(function ($) {
    app.modals.CreatDelModal = function () {

        var _$AttactFileTable = $('#attactfile-tbl');
        var _uploadfileService = abp.services.app.uploadFileCVService;
        var _workService = abp.services.app.work;
        var _dayOffService = abp.services.app.dayOff;
        var _modalManager;
        var _frmDelivery = null;
        var start;
        var end;
        this.init = function (modalManager) {

            $("#Description").summernote({ height: 270, forcus: true });
            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

            //$(_frmDelivery).on("change", ".file-upload-field", function () {
            //    $(this).parent().find(".custom-file-label").html($(this).val().replace(/.*(\/|\\)/, ''));
            //});

            $('.date-picker').datepicker({
                rtl: false,
                dateFormat: 'dd-mm--yy',
                startDate: '-0d',
                orientation: "left",
                autoclose: true,
                noWeekends: true,
                language: abp.localization.currentLanguage.name,
            });
            $("#StartDate").datepicker({
                todayBtn: 1,
                autoclose: true,

            }).on('changeDate', function (selected) {
                var minDate = new Date(selected.date.valueOf());
                $('#EndDate').datepicker('setStartDate', minDate);
                start = $("#StartDate").datepicker("getDate");
                _workService.checkDayOffStart(start).done(function (result) {
                    if (result == false) {
                        _dayOffService.getAllDate(start).done(function (result1) {
                            for (var i = 0; i <= result1.length; i++) {
                                alert('Lưu ý ngày nghỉ ' + result1[i].title + '! Mời chọn lại');
                                $("#StartDate").datepicker("show");
                                break;
                            }

                        })
                    }
                    else {
                        return true;
                    }
                });

            });

            $("#EndDate").datepicker()
                .on('changeDate', function (selected) {
                    var maxDate = new Date(selected.date.valueOf());
                    $('#StartDate').datepicker('setEndDate', maxDate);
                    end = $("#EndDate").datepicker("getDate");
                    _workService.checkDayOffStart(end).done(function (result) {
                        if (result == false) {
                            _dayOffService.getAllDate(end).done(function (result1) {
                                for (var i = 0; i <= result1.length; i++) {
                                    alert('Lưu ý ngày nghỉ ' + result1[i].title + '! Mời chọn lại');
                                    $("#EndDate").datepicker("show");
                                    break;
                                }

                            })

                        }
                        else {
                            return true;
                        }
                    });
                });

            $('.file-upload-field').bind('change', function () {
                infoAttactfile = $(this).prop("files");
                var filelable = $(this).next('.custom-file-label');
                if (infoAttactfile.length > 1) {
                    filelable.html(infoAttactfile.length + ' file được chọn')
                    /* LoadAttactFile(infoAttactfile);*/
                }
                else {
                    $(this).parent().find(".custom-file-label").html($(this).val().replace(/.*(\/|\\)/, ''));
                }

            });
            _frmDelivery.ajaxForm({
                beforeSubmit: function (formData, jqForm, options) {
                    var $fileInput = _frmDelivery.find('input[name=file]');
                    var files = $fileInput.get()[0].files;

                    if (!files.length) {
                        //return false;
                    }
                    else {
                        var file = files[0];
                        if (file.size > 10485760) //10 MB
                        {
                            abp.message.warn(app.localize('File_SizeLimit_Error'));
                            _modalManager.setBusy(false);
                            return false;
                        }
                    }
                    return true;
                },
                success: function (response) {
                    if (response.success) {
                        abp.notify.info('Thêm mới thành công!');
                        _modalManager.close();
                        abp.event.trigger('app.reloadDocTable');
                    } else {
                        abp.message.error(response.error.message);
                        _modalManager.setBusy(false);
                    }
                },
                error: function (xhr) {
                    abp.message.error(xhr.error.message);
                }
            });

        }
        $('.cancel-work-button').click(function () {

            abp.libs.sweetAlert.config = {
                confirm: {
                    icon: 'warning',
                    buttons: ['Không', 'Có']
                }
            };

            abp.message.confirm(
                app.localize('Đóng công việc'),
                app.localize('Bạn có chắc không'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _modalManager.close();
                        return true;

                    }
                }
            );

        });

        this.save = function () {
            var markupDescription = $("#Description").summernote("code");
            var data = _frmDelivery.serializeFormToObject();
            data.description = markupDescription;
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            _frmDelivery.submit();
        };
    };
})(jQuery);