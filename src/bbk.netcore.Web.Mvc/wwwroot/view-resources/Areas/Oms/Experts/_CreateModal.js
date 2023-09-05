(function ($) {

    app.modals.ExpertCreateModal = function () {

        var _ItemsService = abp.services.app.itemsService;
        var _userService = abp.services.app.user;
        var _expertService = abp.services.app.expert;
        var _assignService = abp.services.app.assignments;


        var _modalManager;
        var _frmDelivery = null;
        var dataDaysOff = [];



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

            var getFilterUser = function () {
                let dataFilter = {};
                dataFilter.filter = "";
                dataFilter.permission = "";
                dataFilter.role = 3;

                return dataFilter;
            }

            //Load san pham
            _expertService.getAllItem().done(function (result) {
                var dataselectItems = $.map(result.items, function (obj) {
                    obj.id = obj.id;
                    obj.text = obj.itemCode + " - " + obj.name; // replace name with the property used for the text
                    return obj;

                });

                // select Items
                $('.selectExport').select2({
                    width: "100%",
                    dropdownParent: $('#ExpertCreateModal'),
                    placeholder: 'Chọn hàng hóa',
                    data: dataselectItems,
                    search: true
                });
            });

            //Load user
            _expertService.getAllUser().done(function (result1) {
                var dataselectUsers = $.map(result1.items, function (obj) {
                    obj.id = obj.id;
                    obj.text = obj.surname +" "+ obj.name; // replace name with the property used for the text
                    return obj;

                });

                // select Users
                $('.selectUsers').select2({
                    width: "100%",
                    dropdownParent: $('#ExpertCreateModal'),
                    placeholder: 'Chọn chuyên viên',
                    data: dataselectUsers,
                }).on('select2:select', function (e) {
                    var data = e.params.data;
                    var nameEx = data.surname +" "+ data.name;
                    $("#EmailUser").val(data.emailAddress);
                    $("#NameUser").val(nameEx);
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

            var typeWarehouse = document.getElementById("typeWarehouse");
            // validation: jquery validate
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

            var dataItem = $("#ItemsList").find(":selected");
            for (var i = 0; i < dataItem.length; i++) {
            }

            var data = _frmDelivery.serializeFormToObject();

            
            _modalManager.setBusy(true);
            _expertService.create(data).done(function (result3) {
                _modalManager.close();
                abp.notify.info('Cập nhật Chuyên viên thành công!');
                abp.event.trigger('app.reloadDocTable');

                //them moi bang phan cong
                var dataItem = $("#ItemsList").find(":selected");
                var dataAssign = {};
                for (var i = 0; i < dataItem.length; i++) {
                    dataAssign.UserId = data.UserId;
                    dataAssign.ItemId = dataItem[i].getAttribute("value");

                    _assignService.create(dataAssign).done(function () {

                    });
                }

            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);