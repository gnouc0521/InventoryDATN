(function () {
    $(function () {
        var _$docsTable = $('#docsTable');
        var _docService = abp.services.app.document;
        moment.locale(abp.localization.currentLanguage.name);
      
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'PersonalProfile/Documents/CreateWork',
            scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Works/_CreateModal.js',
            modalClass: 'CreateOrEditDocModal',
            modalType: 'modal-xl'
        });
        async function getDocumentCategoryEnum(docCategoryId) {
            var result = await _docService.getDocumentCategoryEnum(docCategoryId);
            return result;
        }
        /* DEFINE TABLE */
        var getFilter = async function () {
            let dataFilter = {
                targetLanguageName: $('#TextTargetLanguageSelectionCombobox').val(),
                documentCategoryTypeId: $('#DocumentCategoryTypeId').val(),
                documentTypeEnum: $('#DocumentTypeEnum').val(),
                documentCategoryEnum: null,
            };
            if ($('#SearchTerm').val()) {
                dataFilter.searchTerm = $('#SearchTerm').val();
            }
            console.log(dataFilter.documentCategoryTypeId);
            if (dataFilter.documentCategoryTypeId && dataFilter.documentCategoryTypeId == 0) {
                let id = $('#DocumentCategoryTypeId :selected').parent()[0].lastElementChild.getAttribute('value');
                dataFilter.documentCategoryEnum = await getDocumentCategoryEnum(id);
            }
            return dataFilter;
        }
        var dataTable = _$docsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            "searching": false,
            "language": {
                "info": "Hiển thị _START_ đến _END_ của _TOTAL_ kết quả tìm thấy",
                "infoEmpty": "",
                "emptyTable": "Không tìm thấy dữ liệu"
            },
            "bLengthChange": false,
            pageLength: 30,
            listAction: {
                ajaxFunction: _docService.getDocs,
                inputFilter: getFilter
            },

            ajax: async function (data, callback, settings) {
                var filter = await getFilter();
                filter.maxResultCount = data.length;
                filter.skipCount = data.start;

                if (data.order && data.order.length > 0) {
                    var orderingField = data.order[0];
                    if (data.columns[orderingField.column].data) {
                        filter.sorting = data.columns[orderingField.column].data + " " + orderingField.dir;
                    }
                }

                abp.ui.setBusy(_$docsTable);
                _docService.getDocs(filter)
                    .done(function (result) {
                        console.log(result);
                        callback({
                            data: result.items,
                            recordsTotal: result.totalCount,
                            recordsFiltered: result.totalCount,
                        });
                    }).always(function () {
                        abp.ui.clearBusy(_$docsTable);
                    });
            },
            
            "order": [[5, "desc"]],
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    render: function (data, type, row, meta) {
                        return `
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                            		<a class='dropdown-item doceditfunc' data-objid='` + row.id + `' href='javascript:void(0);'> Sửa </a>
                                    <a class='dropdown-item docdeletefunc' data-objid='` + row.id + `' href='javascript:void(0);'> Xóa </a>
                            	</div>
                            </div>`;
                    }
                },
                {
                    targets: 1,
                    orderable: false,
                    data: "decisionNumber",
                    render: function (data, type, row) {
                        //var $container = $("<span/>");
                        if (row.fileUrl) {
                            return `<a href="` + row.fileUrlInfo.fileUrl + `" target="_blank" >
                                <i class="fal fa-file mr-1 color-danger-700"></i>
                            </a>` + data;
                        } else {
                            return data;
                        }
                    }
                },
                {
                    orderable: false,
                    targets: 2,
                    data: "description"
                },
                {
                    orderable: false,
                    targets: 3,
                    data: "documentTypeName"
                },
                {
                    orderable: false,
                    targets: 4,
                    data: "issuedOrganization"
                },
                {
                    targets: 5,
                    data: "issuedDate",
                    render: function (creationTime) {
                        return moment(creationTime).format('L');
                    }
                },
                //, {
                //    targets: 6,
                //    data: "documentTypeEnum"
                //},
                //{
                //    orderable: false,
                //    targets: 6,
                //    data: "creationTime",
                //    render: function (creationTime) {
                //        return moment(creationTime).format('L') + moment(creationTime).format('LTS');
                //    }
                //}
            ]
        });
        function getDocs() {
            dataTable.ajax.reload();
        }

        function deleteDoc(user) {
            //if (user.userName === app.consts.userManagement.defaultAdminDocName) {
            //    abp.message.warn(app.localize("{0}DocCannotBeDeleted", app.consts.userManagement.defaultAdminDocName));
            //    return;
            //}

            abp.message.confirm(
                //app.localize('DeleteWarningMessage', user.userName),
                app.localize('Xóa tài liệu'),
                app.localize('Bạn có chắc không'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _docService.deleteDoc({
                            id: user.id
                        }).done(function () {
                            getDocs(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#CreateNewButton').click(function () {
            var dataFilter = { docCategory: $('#DocumentCategoryEnum').val() };
            _createOrEditModal.open(dataFilter).done(function (e, res) {
                console.log(e);
                console.log(res);
            });
        });
        $('#docsTable').on('click', '.doceditfunc', function (e) {
            var btnClick = $(this);
            var dataFilter = { id: btnClick[0].dataset.objid, docCategory: $('#DocumentCategoryEnum').val() };
            _createOrEditModal.open(dataFilter);
        });
        $('#docsTable').on('click', '.docdeletefunc', function (e) {
            var btnClick = $(this);
            deleteDoc({ id: btnClick[0].dataset.objid });
        });
        abp.event.on('app.updateDocModalSaved', function () {
            getDocs();
        });
        $('#Search').click(function (e) {
            e.preventDefault();
            getDocs();
        });

        jQuery(document).ready(function () {
            $("#SearchTerm").focus();
        });
    });
})();