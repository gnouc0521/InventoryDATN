(function ($) {
    app.modals.ViewModal = function () {
     
        var dataTableAttact = null;
        var _$AttactFileTable = $('#tableTL');
        var _attactfileService = abp.services.app.uploadFileCVService;
        var _userWorkService = abp.services.app.userWork;

        

        this.init = function (modalManager) {

            _modalManager = modalManager;
            _frmDelivery = _modalManager.getModal().find('form[name=FrmEdit]');

            var getFilter = function () {
                let data = {};
                data.workId = $("#Id").val();
                return data;
            }

            
            //code ha
            $(".select2-placeholder-multiple ").select2(
                {
                    placeholder: "All Items",
                    allowClear: true,
                });
            var IdWork = $("#Id").val();
            _userWorkService.getUserIdByWorkId(IdWork).done(function (results) {
                var userIdarr = [];
                $.each(results.items, function (index, value) {
                    userIdarr[index] = value.userId;
                })
                $('[name=multiple-placeholder]').val(userIdarr).trigger('change');
            })

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
                    }
                  

                ],
                "initComplete": function (row) {
                    $('#tableTL').css("width", "100%");
                }
            });


          
        }
    };
})(jQuery);