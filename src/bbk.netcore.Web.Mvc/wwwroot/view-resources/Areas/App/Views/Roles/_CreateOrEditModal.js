(function ($) {
    app.modals.CreateOrEditRoleModal = function () {

        var _modalManager;
        var _roleService = abp.services.app.role;
        var _$roleInformationForm = null;
        var _permissionsTree;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _permissionsTree = new PermissionsTree();
            _permissionsTree.init(_modalManager.getModal().find('.permission-tree'));

            _$roleInformationForm = _modalManager.getModal().find('form[name=RoleInformationsForm]');
            _$roleInformationForm.validate({ ignore: "" });

            $("#RoleName").on("focusout", function () {
                var dest = $(this);
                dest.val(jQuery.trim(dest.val()));
                dest.val(dest.val().replace(/[ ]{2,}/, ' ')); 
                dest.split(" ").join("");
            });
        };

        this.save = function() {
            if (!_$roleInformationForm.valid()) {
                return;
            }
         


            var role = _$roleInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _roleService.createOrUpdateRole({
                role: role,
                grantedPermissionNames: _permissionsTree.getSelectedPermissionNames()
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditRoleModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);