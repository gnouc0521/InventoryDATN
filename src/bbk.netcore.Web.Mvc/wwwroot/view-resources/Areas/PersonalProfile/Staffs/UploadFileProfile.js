(function ($) {
    var _uploadfileService = abp.services.app.uploadFileProfile,
        _$table = $('#UploadFileTable');
    var oldlabletext = "Chọn tài liệu";
    var file;
    var _$modal = $('#Modal');
    var _$uploadFileTable = _$table.DataTable({
        dom: 't',
        serverSide: true,
        ajax: function (data, callback, settings) {
            var personId = $("#PersonId").val();
            abp.ui.setBusy(_$table);
            _uploadfileService.getFile(personId)
                .done(function (result) {
                    callback({
                        data: result
                    });
                }).always(function () {
                    abp.ui.clearBusy(_$table);
                });
        },
        order: [],
        rowId: "id",
        columnDefs: [
            {
                cellType: 'th',
                targets: 0,
                data: 'title',
                sortable: false

            },
            {
                cellType: 'th',
                targets: 1,
                data: 'fileUrl',
                sortable: false,
                render: function (data, type, row) {
                    if (row.fileUrl) {
                        return `<a href="` + row.fileUrl + `" target="_blank" >
                                <i class="fal fa-file mr-1 color-danger-700"></i>
                            </a>${row.fileName }`;
                    } 
                }
            }
           
        ]
    });
    $(document).on("change", "#PersonalProfile_File", function () {
        let fileData = $(this).prop("files")[0];
        if (fileData) {
            $(".custom-file-label").text(fileData.name);
            if (fileData.size > 10485760) //10 MB
            {
                abp.message.warn(app.localize('Dung lượng file không được quá 10Mb!'));
                return false;
            }
            file = fileData;
        } else {
            $(".custom-file-label").text(oldlabletext);
            file = null;
        }
    });
    $('#Modal').on('show.bs.modal', () => {
        file = null;
    });

    $('#Modal').on('hide.bs.modal', () => {
        file = null;
    });
    function create() {
        let _$form = _$modal.find('form');
        _$form.validateForm();
        if (_$form.valid()) {
            if (file) {
                let formData = new FormData();
                formData.set("File", file);
                formData.set("Id", parseInt($("#PersonId").val()));
                formData.set("Title", $('#Title_File').val());
                abp.ui.setBusy(_$form);
                abp.ajax({
                    url: abp.appPath + 'PersonalProfile/Staffs/AddFilePersonalProfile',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false
                }).done(function () {
                    abp.notify.info('Thêm mới thành công!');
                    _$modal.modal('hide');
                    abp.event.trigger("file.updated");
                });
                abp.ui.clearBusy(_$form);
            }  
         }
    }
    $(document).on('click','#button_add_file', function (e) {
        abp.ajax({
            url: abp.appPath + 'PersonalProfile/Staffs/CreateAddFilePersonalProfileModal',
            type: 'Get',
            dataType: 'html',
            success: function (content) {
                _$modal.modal('toggle');
                $('#Modal div.modal-dialog').html(content);
            },
            error: function (e) { }
        });
    });
    abp.event.on('file.updated', (data) => {
        _$uploadFileTable.ajax.reload();
    });
    $(document).on('click', '#Modal .create-file-personalprofile #SaveButton', function (e) {
        e.preventDefault();
        create();
    });
    $(document).on('click', '#button_delete_file', function (e) {
        var id = _$uploadFileTable.row('.selected').id();
        if (id) {
            var personId = $("#PersonId").val();
            abp.message.confirm(
                abp.utils.formatString("File tài liệu này!"),
                "Bạn có chắc chắn muốn xóa",
                (isConfirmed) => {
                    if (isConfirmed) {
                        abp.ui.setBusy(_$table);
                        _uploadfileService.deleteById({ id: id, personId })
                            .done(function () {
                                abp.notify.info('Xóa thành công!');
                                abp.event.trigger("file.updated");
                            }).always(function () {
                                abp.ui.clearBusy(_$table);
                            });
                    }
                }
            );
        } else {
            abp.notify.error("Chọn file cần xóa!");
        }
    });
    $('#UploadFileTable tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            _$uploadFileTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $(document).on('click', '#button_refresh_file', function (e) {
        _$uploadFileTable.ajax.reload();
    });
    $(document).on('click', '#button_edit_file', function (e) {
        var id = _$uploadFileTable.row('.selected').id();
        if (id) {
            abp.ajax({
                url: abp.appPath + `PersonalProfile/Staffs/EditFilePersonalProfileModal?id=${id}`,
                type: 'Get',
                dataType: 'html',
                success: function (content) {
                    _$modal.modal('toggle');
                    $('#Modal div.modal-dialog').html(content);
                },
                error: function (e) { }
            });

        } else {
            abp.notify.error("Chọn file cần sửa!");
        }
    });
})(jQuery);
