(function ($) {
    app.modals.SendMailModal = function () {

        var _purchaseAssignment = abp.services.app.purchaseAssignmentService;
        var _supplierService = abp.services.app.supplier;
        moment.locale(abp.localization.currentLanguage.name);
        var _modalManager;
        var _frmDelivery = null;
        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');
            $("#Comment").summernote({ height: 270});

            $('#SupplierId').on('change', function () {
                var getFilter = function () {
                    let dataFilter = {};
                    dataFilter.supplierId = $('#SupplierId').find(':selected').val();
                    //dataFilter.supplierId = $('#SupplierId').find(':selected').val();
                    return dataFilter;
                }
                _supplierService.getSupplier(getFilter()).done(function (result) {
                    $.each(result, function (index, value) {
                        $("#Address").val(value.adress);
                        $("#PhoneNumber").val(value.phoneNumber);
                        $("#TaxCode").val(value.taxCode);
                        $("#EmailAddress").val(value.email);

                    })
                })
            });

            $('.file-upload-field').bind('change', function () {
                $(this).parent().find(".custom-file-label").html($(this).val().replace(/.*(\/|\\)/, ''));
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
                        abp.notify.info('Gửi email thành công!');
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
            _frmDelivery.submit();
        };
    };
})(jQuery);