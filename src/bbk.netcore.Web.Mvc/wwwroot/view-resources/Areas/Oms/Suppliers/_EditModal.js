(function ($) {
    app.modals.SupplierEditModal = function () {

        var _SupplierService = abp.services.app.supplier;
        var _modalManager;
        var _frmsupplier = null;

        this.init = function (modalManager) {
            $("#Remark").summernote({ height: 270, forcus: true });
            _modalManager = modalManager;
            _frmsupplier = _modalManager.getModal().find('form[name=FrmEdit]');


            var selectProvinceOfBirth = "#selectProvinceOfBirth",
                selectDistrictOfBirth = "#selectDistrictOfBirth",
                selectVillageOfBirth = "#selectVillageOfBirth",


                fullPathDistrict = 'district.json',
                fullPathVillage = 'village.json';

            //Nhập giá trị cho Select 
            $(selectProvinceOfBirth).val($("#valueCityId").val());


            _SupplierService.getAddress(fullPathDistrict, $("#valueCityId").val()).done((result) => {
                for (let i = 0; i < result.addresses.length; i++) {
                    $(selectDistrictOfBirth).append(`<option value="${result.addresses[i].id}" data-id="${result.addresses[i].id}"
                                                    >
                                                    ${result.addresses[i].name}
                                    </option>`);

                }
                $(selectDistrictOfBirth).val($("#valueDistrictId").val());
            })

            _SupplierService.getAddress(fullPathVillage, $("#valueDistrictId").val()).done((result) => {
                console.log(result);
                for (let i = 0; i < result.addresses.length; i++) {
                    $(selectVillageOfBirth).append(`<option value="${result.addresses[i].id}" data-id="${result.addresses[i].id}"
                                                    >
                                                    ${result.addresses[i].name}
                                    </option>`)
                }
                console.log("sfasdf");
                $(selectVillageOfBirth).val($("#valueWardsId").val());
            })



            document.getElementById("PhoneNumber").addEventListener("input", function () {
                var valueChange = funcChanePhoneNumber();
                _frmsupplier.find('input[name=PhoneNumber]').val(valueChange);
            });
            function funcChanePhoneNumber() {
                var valueChange = null;
                var valueInputPhone = _frmsupplier.find('input[name=PhoneNumber]').val();
                if (valueInputPhone.substring(0, 1) == 0) {
                    valueChange = _frmsupplier.find('input[name=PhoneNumber]').val().replace('0', '');
                } else {
                    valueChange = _frmsupplier.find('input[name=PhoneNumber]').val().replace(/[^0-9]/g, '');
                }
                return valueChange;
            }
        }
        //sự kiện khi đóng modal
        $('.cancel-work-button').click(function () {
            abp.libs.sweetAlert.config = {
                confirm: {
                    icon: 'warning',
                    buttons: ['Không', 'Có']
                }
            };

            abp.message.confirm(
                app.localize('Đóng Nhà cung cấp'),
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
            _frmsupplier.addClass('was-validated');
            if (_frmsupplier[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            var markupDescription = $("#Remark").summernote("code");
            var data = _frmsupplier.serializeFormToObject();
            _modalManager.setBusy(true);
            _SupplierService.update(data)
                .done(function () {
                    _modalManager.close();
                    abp.notify.info('Sửa thành công!');
                    abp.event.trigger('app.reloadDocTable');

                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);