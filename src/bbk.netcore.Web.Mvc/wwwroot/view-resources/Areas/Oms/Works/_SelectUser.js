(function () {
    app.modals.UserModal = function () {
        var _$selectuserTable = $('#SelectUserTable');
        var _workService = abp.services.app.work;
        var _modalManager;
        moment.locale(abp.localization.currentLanguage.name);

        this.init = function (modalManager) {
            _modalManager = modalManager;

            //Load Table User
            var datauserTable = _$selectuserTable.DataTable({
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
                pageLength: 10,
                listAction: {
                    ajaxFunction: _workService.getAlUsers
                },
                "order": [[2, "asc"]],
                columnDefs: [
                    {
                        targets: 0,
                        data: null,
                        orderable: false,
                        autoWidth: false,
                        render: function (data, type, row, meta) {
                            return `<input name='record' type='checkbox' class='selectuserid' value='' data-objid='` + row.id + `' data-objname='` + row.userName + `' data-objfname='` + row.fullName + `'>`;
                        },
                        className: "text-center"
                    },
                    {
                        orderable: true,
                        targets: 1,
                        data: "fullName",
                        className: "text-center"
                    },
                    {
                        orderable: true,
                        targets: 2,
                        data: "email",
                        className: "text-center"
                    },
                ]
            });

            //Event tag input checked
            $('#SelectUserTable').on('click', '.selectuserid', function (e) {
                var btnClick = $(this);
                var eleselect = document.getElementsByClassName('selectuserid');
                for (var i = 0; i < eleselect.length; i++) {
                    if (eleselect[i].getAttribute('value') == 'checked') {
                        eleselect[i].checked = false;
                        eleselect[i].setAttribute('value', '');
                    }
                    if (eleselect[i] == btnClick) {
                        continue;
                    }
                }
                if (btnClick.attr('value') == 'checked') {
                    btnClick.attr('value', '');
                }
                else {
                    btnClick.attr('value', 'checked');
                }
            });
        }

        function SelectUserViewContracts() {
            var element = document.getElementsByClassName('selectuserid');
            var userid;
            var username;
            var userfullname;
            for (var i = 0; i < element.length; i++) {
                if (element[i].value == 'checked') {
                    userid = element[i].getAttribute('data-objid');
                    username = element[i].getAttribute('data-objname');
                    userfullname = element[i].getAttribute('data-objfname');
                    break;
                }
            }
            console.log('arridselect', userid, username);
            $('.close').click();
            $('#userselected').html(userfullname);
            $('#userselected').attr("data-objid", userid);
            $('#userselected').attr("data-objname", username);
        }


        this.save = function () {

            SelectUserViewContracts();



        }
    }

})(jQuery);