(function ($) {
  app.modals.RulesEditModal = function () {

    var _rulerService = abp.services.app.rulesService;
    var _modalManager;
    var _frmDelivery = null;
    var dataDaysOff = [];

    this.init = function (modalManager) {

      _modalManager = modalManager;
      _frmDelivery = _modalManager.getModal().find('form[name=FrmEdit]');
      document.querySelectorAll('input[type="number"]').forEach(input => {
        input.oninput = () => {
          if (input.value.length > input.maxLength) input.value = input.value.slice(0, input.maxLength);
        }
      })
      if ($('#ValueItemKey').val()) {
        ($('select[name="ItemKey"]').val($('#ValueItemKey').val()))
      }
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
      _modalManager.setBusy(true);
      _rulerService.update(data)
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