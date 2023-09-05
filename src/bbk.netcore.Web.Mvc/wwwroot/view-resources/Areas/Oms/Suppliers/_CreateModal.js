(function ($) {

    app.modals.SupplierCreateModal = function () {


        var _SupplierService = abp.services.app.supplier;
        var _modalManager;
        var _frmsupplier = null;

        this.init = function (modalManager) {
            $("#Remark").summernote({ height: 270, forcus: true });
            _modalManager = modalManager;
            _frmsupplier = _modalManager.getModal().find('form[name=frmCreate]');


            var selectProvinceOfBirth = "#selectProvinceOfBirth",
                selectDistrictOfBirth = "#selectDistrictOfBirth",
                selectVillageOfBirth = "#selectVillageOfBirth",


                fullPathDistrict = 'district.json',
                fullPathVillage = 'village.json';

            function loadSelectAdress(filePath, superiorId, idSelect, txt) {
                var selectName = $(idSelect).attr("data-select-id");
                changeStatusSelectVillage(idSelect, txt, false);
                _SupplierService.getAddress(filePath, superiorId).done((result) => {
                    for (let i = 0; i < result.addresses.length; i++) {
                        $(idSelect).append(`<option value="${result.addresses[i].id}" data-id="${result.addresses[i].id}"
                                                   ${selectName == result.addresses[i].name ? 'selected' : ''} >
                                                    ${result.addresses[i].name}
                                    </option>`)
                    }
                })
            }
            $(document).on('change', selectProvinceOfBirth, function () {
                changeStatusSelectVillage(selectVillageOfBirth, "Chọn Xã/Phường", true);
                loadSelectAdress(fullPathDistrict, $(selectProvinceOfBirth).find(':selected').attr('data-id'), selectDistrictOfBirth, 'Chọn Huyện/Quận');
            });
            $(document).on('change', selectDistrictOfBirth, function () {
                changeStatusSelectVillage(selectVillageOfBirth, "Chọn Xã/Phường", false);
                loadSelectAdress(fullPathVillage, $(selectDistrictOfBirth).find(':selected').attr('data-id'), selectVillageOfBirth, 'Chọn Xã/Phường');
            });
            /* $(document).on('change', selectProvinceOfNativePlace, () => {
                 changeStatusSelectVillage(selectVillageOfNativePlace, "Chọn Xã/Phường", true);
                 loadSelectAdress(fullPathDistrict, selectProvinceOfNativePlace.find(':selected').attr('data-id'), selectDistrictOfNativePlace, "Chọn Huyện/Quận")
             })*/
            function changeStatusSelectVillage(idChangeSelect, txt, status) {
                //Delete element isChange
                $(idChangeSelect).empty();
                //Add properties disable = true if want idChangeSelect enabled
                $(idChangeSelect).prop('disabled', status);
                $(idChangeSelect).append(`<option value="" selected disabled>` + txt + `</option>`);
            };


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
            _SupplierService.create(data)
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