(function ($) {

    app.modals.WarehouseCardCreateModal = function () {

        var _$itemTable = $('#ItemTable');
        var _warehouseCardService = abp.services.app.warehouseCard;
        var _importRequestsdetail = abp.services.app.importRequestDetail;
        var _itemsServiceService = abp.services.app.itemsService;
        var _unitService = abp.services.app.unitService;
        var _transferService = abp.services.app.transfer;
        var _transferDetailService = abp.services.app.transferDetail;
        var _modalManager;
        var _frmIMP = null;



        this.init = function (modalManager) {
            _modalManager = modalManager;
            _frmIMP = _modalManager.getModal().find('form[name=frmCreate]');
           
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
            _frmIMP.addClass('was-validated');
            if (_frmIMP[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }

          var data = _frmIMP.serializeFormToObject();
          console.log("data", data);
          debugger;
            _modalManager.setBusy(true);
            _warehouseCardService.create(data)
                .done(function (result) {
                    _modalManager.close();
                    abp.notify.info('Thêm mới loại kho thành công!');
                    abp.event.trigger('app.reloadTranferTable');
                   
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);