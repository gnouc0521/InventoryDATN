(function () {

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Quote/CreateItems',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Quote/_CreateModal.js',
        modalClass: 'CreatDelModal',
        modalType: 'modal-xl'

    });
    $('#Add').on('click', function () {
        _createModal.open();
    })
  
    
    
})(jQuery);