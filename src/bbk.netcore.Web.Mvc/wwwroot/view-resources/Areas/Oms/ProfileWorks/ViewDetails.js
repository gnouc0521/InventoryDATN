(function () {
    var _$tableviewwork = $('#TableViewWork');
    var _$tableviewfile = $('#TableViewFile');
    var _profileWorkService = abp.services.app.profileWork;
    moment.locale(abp.localization.currentLanguage.name);


    //Hiển thị view Công việc
    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Work/ViewDetails',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Works/_uploadFile.js',
        modalClass: 'ViewModal',
        modalType: 'modal-xl'
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        let idProfileWork = $("#ProfileWorkId").val();

        dataFilter.idProfileWork = idProfileWork;
        dataFilter.searchTerm = $("#SearchTerm").val().toLocaleLowerCase();

        return dataFilter;
    }


    // Bảng hiển thị công việc
    var dataTableViewfile = _$tableviewwork.DataTable({
        paging: true,
        serverSide: false,
        processing: true,
        "searching": false,
        "language": {
            "emptyTable": "Không tìm thấy dữ liệu",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
        },
        "bInfo": false,
        "bLengthChange": true,
        //"dom": 'Rltip',
        lengthMenu: [
            [5, 10, 25, 50, -1],
            [5, 10, 25, 50, 'Tất cả'],
        ],
        pageLength: 10,
        listAction: {
            ajaxFunction: _profileWorkService.getAllWorkByProfileWorkId,
            inputFilter: getFilter
        },

        columnDefs: [
            {
                orderable: false,
                targets: 0,
                className: "text-center",
                render: function (data, type, row, meta) {
                    var stt = meta.row + 1;
                    
                    return `<span>` + stt +`</span>`
                }
            },
            {
                orderable: false,
                targets: 1,
                render: function (data, type, row, meta) {
                    return `
                        <a class="viewwork" data-objid='` + row.id + `' href='javascript:void(0); '>`+row.title+`</a>
                    `
                }
            },
            {
                orderable: false,
                targets: 2,
                data: "hostName",
            },
            {
                orderable: false,
                targets: 3,
                data: "startDate",
                render: function (createTime) {
                    return moment(createTime).format("L");
                }
            },
            {
                orderable: false,
                targets: 4,
                data: "endDate",
                render: function (createTime) {
                    return moment(createTime).format("L");
                }
            },
            {
                orderable: false,
                targets: 5,
                data: "fileNum",
            },
        ]
    });

    $('#TableViewWork').on('click', '.viewwork', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });

    // Bảng hiển thị file
    //var dataTableViewwork = _$tableviewfile.DataTable({
    //    paging: true,
    //    serverSide: true,
    //    processing: true,
    //    "searching": false,
    //    "language": {
    //        "info": "Hiển thị _START_ đến _END_ của _TOTAL_ kết quả tìm thấy",
    //        "infoEmpty": "",
    //        "emptyTable": "Không tìm thấy dữ liệu"
    //    },
    //    "bLengthChange": false,
    //    pageLength: 10,
    //    listAction: {
    //        ajaxFunction: _profileWorkService.getAllFileByProfileWorkId,
    //        inputFilter: getFilter
    //    },

    //    columnDefs: [
    //        {
    //            orderable: false,
    //            targets: 0,
    //            render: function (data, type, row, meta) {
    //                var nameFile = row.fileUrl.split("\\\\");
    //                var name = nameFile[1];
    //                return `
    //                           <a href="`+ row.fileUrl + `">` + name + `</a>
    //                        `

    //            }
    //        },
    //    ],
    //});

    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {

        dataTableViewfile.ajax.reload();
    }



    //-------------------------------------Thống kê---------------------------------------------

    _profileWorkService.getAllWorkByProfileWorkIdInChart(getFilter()).done(function (result) {
        var tongdg = 0;
        var tonghoanthanhdg = 0;
        var tongdxldg = 0;
        var tongchoxldg = 0;
        var tongquahandg = 0;

        var tongdag = 0;
        var tonghoanthanhdag = 0;
        var tongdxldag = 0;
        var tongchoxldag = 0;
        var tongquahandag = 0;

        var tongph = 0;
        var tonghoanthanhph = 0;
        var tongdxlph = 0;
        var tongchoxlph = 0;
        var tongquahanph = 0;

        for (var i = 0; i < result.items.length; i++) {
            if (result.items[i].ownerStatusEnum == 1 ) {
                if (result.items[i].status == 2) {
                    tonghoanthanhdg += 1;
                    tongdg += 1;
                }
                if (result.items[i].status == 1) {
                    tongdxldg += 1;
                    tongdg += 1;
                }
                if (result.items[i].status == 3) {
                    tongquahandg += 1;
                    tongdg += 1;
                }
                if (result.items[i].status == 0) {
                    tongchoxldg += 1;
                    tongdg += 1;
                }

            }
            else if (result.items[i].ownerStatusEnum == 4) {
                
                if (result.items[i].status == 2) {
                    tonghoanthanhdag += 1;
                    tongdag += 1;
                }
                if (result.items[i].status == 1) {
                    tongdxldag += 1;
                    tongdag += 1;
                }
                if (result.items[i].status == 3) {
                    tongquahandag += 1;
                    tongdag += 1;
                }
                if (result.items[i].status == 0) {
                    tongchoxldag += 1;
                    tongdag += 1;
                }

            }
            else if (result.items[i].ownerStatusEnum == 2) {
               
                if (result.items[i].status == 2) {
                    tonghoanthanhph += 1;
                    tongph += 1;
                }
                if (result.items[i].status == 1) {
                    tongdxlph += 1;
                    tongph += 1;
                }
                if (result.items[i].status == 3) {
                    tongquahanph += 1;
                    tongph += 1;
                }
                if (result.items[i].status == 0) {
                    tongchoxlph += 1;
                    tongph += 1;
                }

                
            }
        }
        //View tổng công việc được giao
        $("#sum-dg_ht").html(tonghoanthanhdg);
        $("#sum-dg_dag").html(tongdxldg);
        $("#sum-dg_ch").html(tongchoxldg);
        $("#sum-dg_qh").html(tongquahandg);
        $(".sum-dg_all").html(tongdg);
        //View tổng công việc đã giao
        $("#sum-dag_ht").html(tonghoanthanhdag);
        $("#sum-dag_dag").html(tongdxldag);
        $("#sum-dag_ch").html(tongchoxldag);
        $("#sum-dag_qh").html(tongquahandag);
        $(".sum-dag_all").html(tongdag);
        //View tổng công việc phối hợp
        $("#sum-ph_ht").html(tonghoanthanhph);
        $("#sum-ph_dag").html(tongdxlph);
        $("#sum-ph_ch").html(tongchoxlph);
        $("#sum-ph_qh").html(tongquahanph);
        $(".sum-ph_all").html(tongph);

        

    
    })
    //-------------------------------------/ Thống kê---------------------------------------------
    

    

})(jQuery);