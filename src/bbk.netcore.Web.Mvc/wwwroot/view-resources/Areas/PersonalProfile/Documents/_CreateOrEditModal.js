(function ($) {
    app.modals.CreateOrEditDocModal = function () {

        //var _userService = abp.services.app.user;

        var _modalManager;
        var _$docInforForm = null;

        this.init = function (modalManager) {
            
            _modalManager = modalManager;
            _$docInforForm = _modalManager.getModal().find('form[name=DocInforForm]');

            $(_$docInforForm).on("change", ".file-upload-field", function () {
                $(this).parent().find(".custom-file-label").html($(this).val().replace(/.*(\/|\\)/, ''));
            });

            if (jQuery().datepicker) {
                $('.date-picker').datepicker({
                    rtl: false,
                    orientation: "left",
                    autoclose: true,
                    language: abp.localization.currentLanguage.name
                });
            }

            _$docInforForm.ajaxForm({
                beforeSubmit: function (formData, jqForm, options) {
                    var $fileInput = _$docInforForm.find('input[name=docFile]');
                    var files = $fileInput.get()[0].files;

                    if (!files.length) {
                        //return false;
                    }
                    else {
                        var file = files[0];

                        //File type check
                        //var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
                        //if ('|css|'.indexOf(type) === -1) {
                        //    abp.message.warn(app.localize('File_Invalid_Type_Error'));
                        //    return false;
                        //}

                        //File size check
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
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.updateDocModalSaved');
                    } else {
                        abp.message.error(response.error.message);
                        _modalManager.setBusy(false);
                    }
                },
                error: function (xhr) {
                    //abp.message.error(xhr);
                    abp.message.error(xhr.error.message);
                }
            });
        };

        this.save = function () {

            _$docInforForm.addClass('was-validated');
            if (_$docInforForm[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            _modalManager.setBusy(true);
            _$docInforForm.submit();
        };
    };
})(jQuery);