(function () {
    $(function () {

        var _$rolesTable = $('#RolesTable');
        var _roleService = abp.services.app.role;
        var _entityTypeFullName = 'bbk.netcore.Authorization.Roles.Role';

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Roles.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Roles.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Roles.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Roles/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Roles/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditRoleModal'
        });

        //var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, function (entityType) {
                    return entityType === _entityTypeFullName;
                }).length === 1; R45
        }

        var dataTable = _$rolesTable.DataTable({
            search: false,
            paging: false,
            serverSide: false,
            processing: false,
            drawCallback: function (settings) {
                $('[data-toggle=m-tooltip]').tooltip();
            },
            listAction: {
                ajaxFunction: _roleService.getRoles,
                inputFilter: function () {
                    return {
                        permission: $('#PermissionSelectionCombo').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    render: function (data, type, row, meta) {
                        return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item roleeditfunc' data-roleid='` + row.id + `' href='javascript:void(0);'>` + app.localize('Cập nhật') + `</a>
                                    <a class='dropdown-item roledeletefunc' data-roleid='` + row.id + `' href='javascript:void(0);'>` + app.localize('Xoá') + `</a>
                            	</div>
                            </div>`;
                    },
                    rowAction: {
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.id });
                            }
                        }, {
                            text: app.localize('Delete'),
                            visible: function (data) {
                                return !data.record.isStatic && _permissions.delete;
                            },
                            action: function (data) {
                                deleteRole(data.record);
                            }
                        },
                        {
                            text: app.localize('History'),
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.id,
                                    entityTypeDescription: data.record.displayName
                                });
                            }
                        }]
                    }
                },
                {
                    targets: 2,
                    data: "displayName",
                    render: function (displayName, type, row, meta) {
                        var $span = $('<span/>');
                        $span.append(displayName + " &nbsp;");

                        if (row.isStatic) {
                            $span.append(
                                $("<span/>")
                                    .addClass("m-badge m-badge--brand m-badge--wide")
                                    .attr("data-toggle", "m-tooltip")
                                    .attr("title", app.localize('StaticRole_Tooltip'))
                                    .attr("data-placement", "top")
                                    .text(app.localize('Static'))
                                    .css("margin-right", "5px")
                            );
                        }

                        if (row.isDefault) {
                            $span.append(
                                $("<span/>")
                                    .addClass("m-badge m-badge--metal m-badge--wide")
                                    .attr("data-toggle", "m-tooltip")
                                    .attr("title", app.localize('DefaultRole_Description'))
                                    .attr("data-placement", "top")
                                    .text(app.localize('Default'))
                                    .css("margin-right", "5px")
                            );
                        }

                        return $span[0].outerHTML;
                    }
                },
                {
                    targets: 3,
                    data: "creationTime",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                }
            ]
        });

        function deleteRole(Id) {
            abp.message.confirm(
                app.localize('Xoa Role'),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _roleService.deleteRole({
                            id: Id
                        }).done(function () {
                            getRoles();
                            abp.notify.success(app.localize('Xoá thành công'));
                        });
                    }
                }
            );
        };

        $("#CreateNewUserButton").click(function () {
            _createOrEditModal.open();
        });

        $('#RefreshRolesButton').click(function (e) {
            e.preventDefault();
            getRoles();
        });

        function getRoles() {
            dataTable.ajax.reload();
        }
        $('#RolesTable').on('click', '.roleeditfunc', function (e) {
            var btnClick = $(this);
            _createOrEditModal.open({ id: btnClick[0].dataset.roleid });
        });
        $('#RolesTable').on('click', '.roledeletefunc', function (e) {
            var Id = $(this).attr('data-roleid');
            deleteRole(Id);
        });
        abp.event.on('app.createOrEditRoleModalSaved', function () {
            getRoles();
        });

    });
})();