(function ($) {

    app.modals.WareHouseEditModal = function () {


        var _wareHouse = abp.services.app.wareHouse;
        var _modalManager;
        var _frmDelivery = null;
        var dataDaysOff = [];



        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

            var id = $("#Id").val();
            _wareHouse.get({ id: id }).done(function (result) {
                $('[name=selectType]').val(result.typeWarehouse).trigger('change');
            })

            var selectProvinceOfBirth = "#selectProvinceOfBirth",
                selectDistrictOfBirth = "#selectDistrictOfBirth",
                selectVillageOfBirth = "#selectVillageOfBirth",


                fullPathDistrict = 'district.json',
                fullPathVillage = 'village.json';

            //Nhập giá trị cho Select 
            $(selectProvinceOfBirth).val($("#valueCityId").val());
            
            
            _wareHouse.getAddress(fullPathDistrict, $("#valueCityId").val()).done((result) => {
                for (let i = 0; i < result.addresses.length; i++) {
                    $(selectDistrictOfBirth).append(`<option value="${result.addresses[i].id}" data-id="${result.addresses[i].id}"
                                                    >
                                                    ${result.addresses[i].name}
                                    </option>`);

                }
                $(selectDistrictOfBirth).val($("#valueDistrictId").val());
            })
            
            _wareHouse.getAddress(fullPathVillage, $("#valueDistrictId").val()).done((result) => {
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
            
            //Loại kho
            $("#typeCode-select").val($("#WareHouseTypeId").val());


            //Xử lý chọn địa điểm
            function loadSelectAdress(filePath, superiorId, idSelect, txt) {
                console.log(superiorId);
                var selectName = $(idSelect).attr("data-select-id");
                changeStatusSelectVillage(idSelect, txt, false);
                _wareHouse.getAddress(filePath, superiorId).done((result) => {
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
            var typeWarehouse = document.getElementById("typeWarehouse");
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var data = _frmDelivery.serializeFormToObject();
            data.TypeWarehouse = typeWarehouse.options[typeWarehouse.selectedIndex].value;

            _modalManager.setBusy(true);
            _wareHouse.update(data)
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