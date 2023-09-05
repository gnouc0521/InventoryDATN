(function ($) {
    app.modals.SubsidiaryEditModal = function () {

        var _subsidiaryService = abp.services.app.subsidiaryService;
        var _modalManager;
        var _frmsubsidiary = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmsubsidiary = _modalManager.getModal().find('form[name=FrmEdit]');


           /* var id = $("#Id").val();*/
            //_subsidiaryService.get({ id: id }).done(function (result) {
            //    $('[name=selectType]').val(result.typeWarehouse).trigger('change');
            //})

            var selectProvinceOfBirth = "#selectProvinceOfBirth",
                selectDistrictOfBirth = "#selectDistrictOfBirth",
                selectVillageOfBirth = "#selectVillageOfBirth",


                fullPathDistrict = 'district.json',
                fullPathVillage = 'village.json';

            //Nhập giá trị cho Select 
            $(selectProvinceOfBirth).val($("#valueCityId").val());


            _subsidiaryService.getAddress(fullPathDistrict, $("#valueCityId").val()).done((result) => {
                for (let i = 0; i < result.addresses.length; i++) {
                    $(selectDistrictOfBirth).append(`<option value="${result.addresses[i].id}" data-id="${result.addresses[i].id}"
                                                    >
                                                    ${result.addresses[i].name}
                                    </option>`);

                }
                $(selectDistrictOfBirth).val($("#valueDistrictId").val());
            })

            _subsidiaryService.getAddress(fullPathVillage, $("#valueDistrictId").val()).done((result) => {
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

            function loadSelectAdress(filePath, superiorId, idSelect, txt) {
                var selectName = $(idSelect).attr("data-select-id");
                changeStatusSelectVillage(idSelect, txt, false);
                _subsidiaryService.getAddress(filePath, superiorId).done((result) => {
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
                _frmsubsidiary.find('input[name=PhoneNumber]').val(valueChange);
            });
            function funcChanePhoneNumber() {
                var valueChange = null;
                var valueInputPhone = _frmsubsidiary.find('input[name=PhoneNumber]').val();
                if (valueInputPhone.substring(0, 1) == 0) {
                    valueChange = _frmsubsidiary.find('input[name=PhoneNumber]').val().replace('0', '');
                } else {
                    valueChange = _frmsubsidiary.find('input[name=PhoneNumber]').val().replace(/[^0-9]/g, '');
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
            _frmsubsidiary.addClass('was-validated');
            if (_frmsubsidiary[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            var data = _frmsubsidiary.serializeFormToObject();
            _modalManager.setBusy(true);
            _subsidiaryService.update(data)
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