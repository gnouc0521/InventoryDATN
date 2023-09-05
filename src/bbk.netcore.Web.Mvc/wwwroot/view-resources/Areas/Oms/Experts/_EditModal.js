(function ($) {

    app.modals.ExpertEditModal = function () {

        var _ItemsService = abp.services.app.itemsService;
        var _userService = abp.services.app.user;
        var _expertService = abp.services.app.expert;
        var _assignService = abp.services.app.assignments;


        var _modalManager;
        var _frmDelivery = null;



        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

            document.getElementById("phone").addEventListener("input", function () {
                var valueChange = funcChanePhoneNumber();
                _frmDelivery.find('input[name=PhoneNumber]').val(valueChange);
            });
            function funcChanePhoneNumber() {
                var valueChange = null;
                var valueInputPhone = _frmDelivery.find('input[name=PhoneNumber]').val();
                if (valueInputPhone.substring(0, 1) == 0) {
                    valueChange = _frmDelivery.find('input[name=PhoneNumber]').val().replace('0', '');
                } else {
                    valueChange = _frmDelivery.find('input[name=PhoneNumber]').val().replace(/[^0-9]/g, '');
                }
                return valueChange;
            }

            var getFilter = function () {
                let dataFilter = {};
                dataFilter.searchTerm = "";

                return dataFilter;
            }

            //Load san pham
            _expertService.getAllItemExpert($("#UserId").val()).done(function (result) {
                var dataselectItems = $.map(result.items, function (obj) {
                    obj.id = obj.id;
                    obj.text = obj.itemCode + " - " + obj.name; // replace name with the property used for the text
                    return obj;

                });

                // select Items
                $('.selectExport').select2({
                    width: "100%",
                    dropdownParent: $('#ExpertEditModal'),
                    placeholder: 'Chọn hàng hóa',
                    data: dataselectItems,
                });

                _assignService.getAllByUser($("#UserId").val()).done(function (result2) {
                    var valueSet = [];

                    $.each(result2.items, function (index, value) {
                        valueSet.push(value.itemId);
                    });

                    $('.selectExport').val(valueSet);
                    $('.selectExport').trigger('change');
                });
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

            var typeWarehouse = document.getElementById("typeWarehouse");
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var dataItem = $("#ItemsList").find(":selected");
            var dataselct = [];
            for (var i = 0; i < dataItem.length; i++) {
                dataselct.push(parseInt(dataItem[i].getAttribute("value")));
            }

            var data = _frmDelivery.serializeFormToObject();

            _modalManager.setBusy(true);
           
            
            
            _expertService.update(data).done(function (result3) {
                _modalManager.close();
                abp.notify.info('Cập nhật Chuyên viên thành công!');
                abp.event.trigger('app.reloadDocTable');

                _assignService.getAllByUser($("#UserId").val()).done(function (result2) {
                    var valueSample = [];

                    $.each(result2.items, function (index, value) {
                        valueSample.push(parseInt(value.itemId));

                    });

                    var dataXoa = dataselct;

                    // Cac san pham them moi
                    var dataCreate = {};
                    dataselct = dataselct.filter(function (val) {
                        return valueSample.indexOf(val) == -1;
                    });
                    for (var i = 0; i < dataselct.length; i++) {
                        dataCreate.UserId = data.UserId;
                        dataCreate.ItemId = dataselct[i];

                        _assignService.create(dataCreate).done(function () {

                        });
                    }


                    //Cac san pham can xoa
                    valueSample = valueSample.filter(function (val) {
                        return dataXoa.indexOf(val) == -1;
                    });
                    for (var j = 0; j < valueSample.length; j++) {
                        _assignService.deletebyItem(valueSample[j]).done(function () {

                        });
                        }
                    });
            

                ////them moi bang phan cong
                //var dataItem = $("#ItemsList").find(":selected");
                //var dataAssign = {};
                //for (var i = 0; i < dataItem.length; i++) {
                //    dataAssign.UserId = data.UserId;
                //    dataAssign.ItemId = dataItem[i].getAttribute("value");

                //    _assignService.create(dataAssign).done(function () {

                //    });
                //}

            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);