
(function ($) {
       moment.locale(abp.localization.currentLanguage.name);
       var _recruitmentInfomationService = abp.services.app.recruitmentInfomation,
         _$docTable = $('#DocumentTable'),
         _$standTable = $('#StanderdTable'),
         _$recruimentTable = $('#AfterRecruimentTable');
     
       var _$DocumentTable = _$docTable.DataTable({
           dom: 't',
           order: [],
            serverSide: true,
            ajax: function (data, callback, settings) {
                var staffId = $("#PersonId").val();
                abp.ui.setBusy(_$docTable);
                _recruitmentInfomationService.getAll(staffId)
                    .done(function (result) {
                        callback({
                            data: result.items
                        });
                    }).always(function () {
                        abp.ui.clearBusy(_$docTable);
                    });
            },
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    data: 'registrationForm',
                    sortable: false,
                    cellType: 'th',                
                    render: function (data) {
                        return Format(data);
                    }
                },
                {
                    targets: 1,
                    data: 'certifiedBackground',
                    sortable: false,
                    render: function (data) {
                        return Format(data);
                    }
                },
                {
                    targets: 2,
                    data: 'copyOfBirthCertificate',
                    sortable: false,
                    render: function (data) {
                        return Format(data);
                    }
                },
                {
                    targets: 3,
                    data: 'healthCertificate',
                    sortable: false,
                    render: function (data) {
                        return Format(data);
                    }
                },
                {
                    targets: 4,
                    data: 'preferredCertificate',
                    sortable: false
                },
                {
                    targets: 5,
                    data: 'notDisciplined',
                    sortable: false,
                    render: function (data) {
                        return Format(data);
                    }
                }
               
            ]
        });
       var _$StandardTable = _$standTable.DataTable({
           dom: 't',
           order: [],
            serverSide: true,
            ajax: function (data, callback, settings) {
                var staffId = $("#PersonId").val();
                abp.ui.setBusy(_$standTable);
                _recruitmentInfomationService.getAll(staffId)
                    .done(function (result) {
                        callback({
                            data: result.items
                        });
                    }).always(function () {
                        abp.ui.clearBusy(_$standTable);
                    });
            },
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    data: 'expertise',
                    sortable: false,
                    defaultContent: '',
                    cellType: 'th'
                },
                {
                    targets: 1,
                    data: 'otherLanguage',
                    defaultContent: '',
                    sortable: false
                },
                {
                    targets: 2,
                    data: 'infomationTechnology',
                    defaultContent: '',
                    sortable: false
                },
                {
                    targets: 3,
                    data: "",
                    sortable: false,
                    defaultContent:''  
                }             
            ]
        });
       var _$AfterRecruitmentInfomation = _$recruimentTable.DataTable({
           dom: 't',
           order: [], 
            serverSide: true,
            ajax: function (data, callback, settings) {
                var staffId = $("#PersonId").val();
                abp.ui.setBusy(_$standTable);
                _recruitmentInfomationService.getAll(staffId)
                    .done(function (result) {
                        callback({
                            data: result.items
                        });
                    }).always(function () {
                        abp.ui.clearBusy(_$standTable);
                    });
            },
            rowId: "id",
            columnDefs: [
                {
                    targets: 0,
                    sortable: false,
                    defaultContent: '',
                    render: (data, type, row) => {
                        return `${moment(row.timeElectNotice).format('L')}`
                    },
                    cellType: 'th'
                },
                {
                    targets: 1,
                    data: 'timeDecision',
                    sortable: false,
                    render: (data, type, row) => {
                        return `${moment(row.timeDecision).format('L')}`
                    }
                },
                {
                    targets: 2,
                    data: 'workUnit',
                    sortable: false
                },
                {
                    targets: 3,                 
                    sortable: false,
                    render: (data, type, row) => {
                        return `${moment(row.timeGetJob).format('L')}`
                    }
                },
                {
                    targets: 4,
                    data: "regulationsGuideProbation",
                    sortable: false
                },
                {
                    targets: 5,
                    data: "regimeOfApprentice",
                    sortable: false,
                    render: function (data) {
                        return Format(data);
                    }
                },
                {
                    targets: 6,
                    data: "regimeOfApprenticeInstructor",
                    sortable: false
                },
                {
                    targets: 7,
                    data: "exemptProbationary",
                    sortable: false,
                    render: function (data) {
                        return Format(data);
                    }
                }

            ]
        });
     
       abp.event.on('recruitmentInfomation.updated', (data) => {
           _$DocumentTable.ajax.reload();
           _$StandardTable.ajax.reload();
           _$AfterRecruitmentInfomation.ajax.reload();


        });

       $('#DocumentTable tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                _$DocumentTable.$('tr.selected').removeClass('selected');
                _$StandardTable.$('tr.selected').removeClass('selected');
                _$AfterRecruitmentInfomation.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
       });
       $('#StanderdTable tbody').on('click', 'tr', function () {
           if ($(this).hasClass('selected')) {
               $(this).removeClass('selected');
           }
           else {
               _$DocumentTable.$('tr.selected').removeClass('selected');
               _$StandardTable.$('tr.selected').removeClass('selected');
               _$AfterRecruitmentInfomation.$('tr.selected').removeClass('selected');
               $(this).addClass('selected');
           }
       });
       $('#AfterRecruimentTable tbody').on('click', 'tr', function () {
           if ($(this).hasClass('selected')) {
               $(this).removeClass('selected');
           }
           else {
               _$DocumentTable.$('tr.selected').removeClass('selected');
               _$StandardTable.$('tr.selected').removeClass('selected');
               _$AfterRecruitmentInfomation.$('tr.selected').removeClass('selected');
               $(this).addClass('selected');
           }
       });

       $(document).on('click', '.refresh-recruitmentInfomation', function (e) {
            _$DocumentTable.ajax.reload();
            _$StandardTable.ajax.reload();
            _$AfterRecruitmentInfomation.ajax.reload();
        });

      

       $(document).on('click', '.create-recruitmentInfomation', function (e) {
            abp.ajax({
                url: abp.appPath + 'PersonalProfile/Staffs/CreateRecruitmentInfomationModal',
                type: 'Get',
                dataType: 'html',
                success: function (content) {
                    $('#Modal div.modal-dialog').html(content);
                    $('#Modal').modal('toggle');
                    if (jQuery().datepicker) {
                        $('#Modal .date-picker').datepicker({
                            rtl: false,
                            orientation: "left",
                            autoclose: true,
                            language: abp.localization.currentLanguage.name
                        });
                    }
                },
                error: function (e) { }
            });
       });
       $('.delete-recruitmentInfomation').click(function () {
           var Id;
           if(_$DocumentTable.row('.selected').id()) {
                Id = _$DocumentTable.row('.selected').id();
           } else if(_$StandardTable.row('.selected').id()) {
                Id = _$StandardTable.row('.selected').id();
           } else{
                Id = _$AfterRecruitmentInfomation.row('.selected').id();
           }
           if (!Id) {
               abp.notify.error("Chọn trước khi muốn xóa!");
               return;
           }
           abp.message.confirm(
               abp.utils.formatString("Bạn có chắc chắn muốn xóa!"),
               null,
               (isConfirmed) => {
                   if (isConfirmed) {
                       abp.ui.setBusy(_$DocumentTable, _$StandardTable, _$AfterRecruitmentInfomation);
                       _recruitmentInfomationService.deleteById(Id)
                           .done(function () {
                               abp.notify.info('Xóa thành công!');
                               _$DocumentTable.ajax.reload();
                               _$StandardTable.ajax.reload();
                               _$AfterRecruitmentInfomation.ajax.reload();
                           }).always(function () {
                               abp.ui.clearBusy(_$DocumentTable, _$StandardTable, _$AfterRecruitmentInfomation);
                           });
                   }
               }
           );
       });

       $(document).on('click', '.edit-recruitmentInfomation', function (e) {
           var id;
           if (_$DocumentTable.row('.selected').id()) {
               id = _$DocumentTable.row('.selected').id();
           } else if (_$StandardTable.row('.selected').id()) {
               id = _$StandardTable.row('.selected').id();
           } else {
               id = _$AfterRecruitmentInfomation.row('.selected').id();
           }
           if (!id) {
               abp.notify.error("Chọn trước khi muốn sửa!");
               return;
           }
           abp.ajax({
               url: abp.appPath + `PersonalProfile/Staffs/EditRecruitmentInfomationModal?id=${id}`,
               type: 'Get',
               dataType: 'html',
               success: function (content) {
                   $('#Modal div.modal-dialog').html(content);
                   $('#Modal').modal('toggle');
                   if (jQuery().datepicker) {
                       $('#Modal .date-picker').datepicker({
                           rtl: false,
                           orientation: "left",
                           autoclose: true,
                           language: abp.localization.currentLanguage.name
                       });
                   }
               },
               error: function (e) {
                   $('#Modal div.modal-dialog').html(null);
                   abp.message.error('Vui lòng thử lại!', 'Có lỗi');
               }
           });
       });

       function Format(type) {
           switch (type) {
               case 0: {
                   return 'Không'
                   break;
               }               
               case 1: {
                   return 'Có'
                   break;
               }
               case 2: {
                   return 'Chưa xác định'
                   break;
               }
               default:
                   break;
           }
       }
    })(jQuery);

