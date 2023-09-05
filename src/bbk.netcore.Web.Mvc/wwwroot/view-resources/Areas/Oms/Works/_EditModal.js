(function ($) {
    app.modals.DeliveryEditModal = function () {
        var _workService = abp.services.app.work;
        var _userWorkService = abp.services.app.userWork;
        var _dayOffService = abp.services.app.dayOff;
        var _modalManager;
        var _frmDelivery = null;
        var dataaa = [];
        var unselect = [];
        var dataa = {};
        var dataTableAttact = null;
        var _$AttactFileTable = $('#attactfile-tbl');
        var _attactfileService = abp.services.app.uploadFileCVService;

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=FrmEdit]');
            $("#Description").summernote({ height: 270, forcus: true });

            $(".select2-placeholder-multiple ").select2(
                {
                    placeholder: "All Items",
                    allowClear: true,
                });
            $("ul.select2-selection__rendered").sortable({
                containment: 'parent'
            });

            //code ha
            var IdWork = $("#Id").val();
            _userWorkService.getUserIdByWorkId(IdWork).done(function (results) {
                var userIdarr = [];
                $.each(results.items, function (index, value) {
                    userIdarr[index] = value.userId;


                })
                $('[name=multiple-placeholder]').val(userIdarr).trigger('change');
            })

            $('.file-upload-field').bind('change', function () {
                infoAttactfile = $(this).prop("files");
                var filelable = $(this).next('.custom-file-label');
                if (infoAttactfile.length > 1) {
                    filelable.html(infoAttactfile.length + ' file được chọn')
                    LoadTblAttact();
                }
                else {
                    $(this).parent().find(".custom-file-label").html($(this).val().replace(/.*(\/|\\)/, ''));
                }

            });

            var getFilter = function () {
                let data = {};
                data.workId = $("#Id").val();
                return data;
            }



            dataTableAttact = _$AttactFileTable.DataTable({
                paging: false,
                serverSide: true,
                processing: false,
                ordering: false,
                info: false,
                "searching": false,
                "language": {
                    "info": "Hiển thị _START_ đến _END_ của _TOTAL_ kết quả tìm thấy",
                    "infoEmpty": "",
                    "emptyTable": "",
                    "zeroRecords": " "
                },
                "bLengthChange": false,
                pageLength: 10,
                listAction: {
                    ajaxFunction: _attactfileService.getAllById,
                    inputFilter: getFilter
                },

                columnDefs: [
                    {
                        orderable: false,
                        targets: 0,
                        data: "fileName",
                        render: function (data, type, row, meta) {
                            return `<a href="` + row.fileUrl + `" style='color: blue' data-filepath='` + row.fileUrl + `' data-filename='` + row.fileName + `'>` + data + `</a>`;
                        }
                    },
                    {
                        orderable: false,
                        targets: 1,
                        data: "creationTime",
                        render: function (creationTime) {
                            return moment(creationTime).format('DD/MM/YYYY HH:mm');
                        }
                    },
                    {
                        orderable: false,
                        targets: 2,
                        data: "createBy"
                    },
                    {
                        orderable: false,
                        targets: 3,
                        className: "text-center",
                        data: "id",
                        render: function (data, type, row, meta) {
                            return `<a style='color: blue; cursor: pointer' class="text-decoration-none deletefunc" data-objid='` + data + `' data-objfname='` + row.fileName + `' data-objcontractid='` + row.contractId + `'>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                    <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"></path>
                                    <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"></path>
                                    </svg>
                                    </a>`;
                        }
                    }

                ],
                "initComplete": function (row) {
                    $('#attactfile-tbl').css("width", "100%");
                }
            });


            function LoadTblAttact() {
                dataTableAttact.ajax.reload();
            }

            /*DELETE*/
            function deleteAttact(objinput) {
                abp.message.confirm(
                    app.localize('Xóa tài liệu'),
                    app.localize('Bạn có chắc không'),
                    (isConfirmed) => {
                        if (isConfirmed) {
                            _attactfileService.deleteUploadFile(objinput)
                                .done(() => {
                                    abp.notify.success(app.localize('Xoá tài liệu thành công!'));
                                    LoadTblAttact();
                                });
                        }
                    }
                );
            }

            $('#attactfile-tbl').on('click', '.deletefunc', function () {
                var btnClick = $(this);
                var datainput = { contractId: btnClick[0].dataset.objcontractid, fileName: btnClick[0].dataset.objfname };
                deleteAttact(datainput);
            });



            $("#multiple-placeholder").select2({}).on("select2:select", function (e) {
                dataaa.push(parseInt(e.params.data.id));
            })


            $("#multiple-placeholder").select2({}).on("select2:unselect", function (e) {
                unselect.push(parseInt(e.params.data.id));
            })
            $('#btnHoanThanh').on('click', function () {
                var data = _frmDelivery.serializeFormToObject();
                _workService.changstatus(data)
                    .done(function () {
                        _modalManager.close();
                        abp.notify.info('Cập nhật thành công!');
                        abp.event.trigger('app.reloadDocTable');
                        /* window.location.reload();*/
                    })
            });

            $('.date-picker').datepicker({
                rtl: false,
                dateFormat: 'dd-mm--yy',
                /* endDate: '0d',*/
                orientation: "left",
                autoclose: true,
                language: abp.localization.currentLanguage.name
            });
            $("#StartDate").datepicker({
                todayBtn: 1,
                autoclose: true,
            }).on('changeDate', function (selected) {
                var minDate = new Date(selected.date.valueOf());
                $('#EndDate').datepicker('setStartDate', minDate);
                $('#EndDate').datepicker('setDate', minDate);
                start = $("#StartDate").datepicker("getDate");
                _workService.checkDayOffStart(start).done(function (result) {
                    if (result == false) {
                        _dayOffService.getAllDate(start).done(function (result1) {
                            for (var i = 0; i <= result1.length; i++) {
                                alert('Lưu ý ngày nghỉ ' + result1[i].title + '! Mời chọn lại');
                                $("#StartDate").datepicker("show");
                                break;
                            }

                        })
                    }
                    else {
                        return true;
                    }
                });
            });

            $("#EndDate").datepicker()
                .on('changeDate', function (selected) {
                    var maxDate = new Date(selected.date.valueOf());
                    $('#StartDate').datepicker('setEndDate', maxDate);
                    end = $("#EndDate").datepicker("getDate");
                    _workService.checkDayOffStart(end).done(function (result) {
                        if (result == false) {
                            _dayOffService.getAllDate(end).done(function (result1) {
                                for (var i = 0; i <= result1.length; i++) {
                                    alert('Lưu ý ngày nghỉ ' + result1[i].title + '! Mời chọn lại');
                                    $("#EndDate").datepicker("show");
                                    break;
                                }

                            })
                        }
                        else {
                            return true;
                        }
                    });
                });


            _frmDelivery.ajaxForm({
                beforeSubmit: function (formData, jqForm, options) {
                    var $fileInput = _frmDelivery.find('input[name=file]');
                    var files = $fileInput.get()[0].files;

                    if (!files.length) {
                        //return false;
                    }
                    else {
                        var file = files[0];
                        if (file.size > 10485760) //10 MB
                        {
                            abp.message.warn(app.localize('File_SizeLimit_Error'));
                            _modalManager.setBusy(false);
                            return false;
                        }
                    }

                    return true;
                },
                success: function (response) {
                    var data = _frmDelivery.serializeFormToObject();
                    if (response.success) {
                        _workService.getById(data.Id).done(function (result) {
                            if (unselect.length == 0) {
                                dataa.workId = result.id;
                                dataa.status = result.status;
                                dataa.ownerStatus = 2;
                                /* result.ownerStatus = 2;*/
                                for (i = 0; i < dataaa.length; i++) {
                                    dataa.userid = dataaa[i];
                                    _userWorkService.create(dataa).done(function () { });

                                }
                                abp.notify.info('Cập nhật thành công!');
                                _modalManager.close();
                                abp.event.trigger('app.reloadDocTable');
                            }
                            else {
                                for (i = 0; i < unselect.length; i++) {
                                    _userWorkService.delete(unselect[i]).done(function () { });

                                }
                                dataa.workId = result.id;
                                dataa.status = result.status;
                                dataa.ownerStatus = 2;
                                /* result.ownerStatus = 2;*/
                                for (i = 0; i < dataaa.length; i++) {
                                    dataa.userid = dataaa[i];
                                    _userWorkService.create(dataa).done(function () { });

                                }
                                abp.notify.info('Cập nhật thành công!');
                                _modalManager.close();
                                abp.event.trigger('app.reloadDocTable');

                            }

                        })


                    } else {
                        abp.message.error(response.error.message);
                        _modalManager.setBusy(false);
                    }
                },
                error: function (xhr) {
                    /* abp.message.error(xhr);*/
                    abp.message.error(xhr.error.message);
                }
            });

            $('.cancel-work-button').click(function () {
                abp.libs.sweetAlert.config = {
                    confirm: {
                        icon: 'warning',
                        buttons: ['Không', 'Có']
                    }
                };

                abp.message.confirm(
                    app.localize('Đóng công việc'),
                    app.localize('Bạn có chắc không'),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            _modalManager.close();
                            return true;

                        }
                    }
                );

            });
        }
        this.save = function () {
            _frmDelivery.addClass('was-validated');
            if (_frmDelivery[0].checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
                return;
            }
            _frmDelivery.submit();

        };
    };
})(jQuery);