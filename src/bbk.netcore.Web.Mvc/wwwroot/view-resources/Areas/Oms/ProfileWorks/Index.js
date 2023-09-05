(function () {
    var _$profileWorkTable = $('#profileWorkTable');
    var _profileWorkService = abp.services.app.profileWork;
    moment.locale(abp.localization.currentLanguage.name);


    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ProfileWork/CreateProfileWork',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ProfileWorks/_CreateModal.js',
        modalClass: 'CreatDelModal',
        modalType: 'modal-xl'

    });

    var _createWorkitemModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ProfileWork/CreateProfileWorkItems',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ProfileWorks/_CreateWorkItemModal.js',
        modalClass: 'WorkitemCreateModal',
        modalType: 'modal-xl'

    });


    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/ProfileWork/EditProfileWork',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/ProfileWorks/_EditModal.js',
        modalClass: 'DeliveryEditModal',
        modalType: 'modal-xl'
    });

    /* DEFINE TABLE */
    var getFilter = function () {
        let dataFilter = {};
        let idProfileWork = document.getElementById("list-workgroup");

        dataFilter.idProfileWork = idProfileWork.options[idProfileWork.selectedIndex].value;
        dataFilter.searchTerm = $('#SearchTerm').val();

        return dataFilter;
    }


    $("#list-workgroup").change(function (e) {

        getDocs();
    });

    var dataTable = _$profileWorkTable.DataTable({
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
            ajaxFunction: _profileWorkService.getAll,
            inputFilter: getFilter
        },

        columnDefs: [
            {

                targets: 0,
                data: null,
                orderable: false,
                autoWidth: false,
                render: function (data, type, row, meta) {
                    if (row.parentId == null) {
                        return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                                    <a class='dropdown-item docaddwork'  data-objid='` + row.id + `' href='javascript:void(0);'> Thêm mới nhóm công việc</a>
                                    <a class='dropdown-item docviewdt' data-objid='` + row.id + `'href='javascript:void(0); ' > Xem chi tiết</a>
                            		<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                                    <a class='dropdown-item docdeletefunc'  data-objid='` + row.id + `' href='javascript:void(0);'> Xóa </a>
                            	</div>
                            </div>`;
                    }
                    else {
                        return `                        
                            <div class='dropdown d-inline-block'>
                            	<a href='#'' class='btn btn-sm btn-icon btn-outline-primary rounded-circle shadow-0' data-toggle='dropdown' aria-expanded='true' title='More options'>
                            		<i class="fal fa-ellipsis-v"></i>
                            	</a>
                            	<div class='dropdown-menu'>
                                    <a class='dropdown-item docviewdt' data-objid='` + row.id + `'href='javascript:void(0); ' > Xem chi tiết</a>
                            		<a class='dropdown-item doceditfunc'data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>
                                    <a class='dropdown-item docdeletefunc'  data-objid='` + row.id + `' href='javascript:void(0);'> Xóa </a>
                            	</div>
                            </div>`;
                    }
                }

            },
            {
                orderable: false,
                targets: 1,
                data: "title"
            },
        ]
    });

    abp.event.on('app.reloadDocTable', function () {
        getDocs();
    });

    $('#profileWorkTable').on('click', '.doceditfunc', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        _EditModal.open(dataFilter);
    });

    $('#profileWorkTable').on('click', '.docaddwork', function (e) {
        var btnClick = $(this);
        var dataFilter = { Id: btnClick[0].dataset.objid };
        /*_EditModal.open(dataFilter);*/
        console.log("aaa", dataFilter);
        _createWorkitemModal.open(dataFilter);
    });

    //Code Hà
    //Click xem chi tiết
    $('#profileWorkTable').on('click', '.docviewdt', function (e) {
        var btnClick = $(this);
        var dataFilter = { id: btnClick[0].dataset.objid };
        abp.ajax({
            contentType: 'application/x-www-form-urlencoded',
            url: "ProfileWork/View/?Id=" + dataFilter.id,
            success: (function (results) {
                window.location.href = "/Inventorys/ProfileWork/ViewDetails?id=" + results.id;
            })
        })
    });


    $('#Search').click(function (e) {
        e.preventDefault();
        getDocs();
    });

    function getDocs() {

        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });



    /* --------------------------------------------- / DEFINE TABLE ----------------------------------------------------*/

    //---------------------------------------------- LOAD LIST WORKPROFILE -------------------------------------------------

    //function create Items
    function createItemWhenClick() {
        $(".btn-create").click(function () {
            var btnFilter = $(this).attr('data-create');
            var dataFilter = { Id: btnFilter };
            _createWorkitemModal.open(dataFilter, function (callback) {
                console.log(callback);
                CreateChilren(callback);
            });
        })
    }

    //function delete Items
    function deleteItem() {
        $(".btn-delete").click(function () {
            console.log("lllllll")
            var btnFilter = $(this).attr('data-delete');
            var isParent = $(this).attr('data-parentId');
            //Nếu là Root
            if (isParent == 0) {
                abp.message.confirm(
                    app.localize('Xóa Hồ sơ công việc'),
                    app.localize('Bạn có chắc không'),
                    (isConfirmed) => {
                        if (isConfirmed) {
                            _profileWorkService
                                .delete(btnFilter)
                                .done((result1) => {
                                    $("#workGroup" + result1).remove();
                                    abp.notify.success(app.localize('Xóa Hồ sơ Công việc thành công'));
                                });
                        }
                    }
                );
            }
            else {
                var numlength = $("#js_accordion-" + isParent).children('.accordion').length;
                if (numlength == 1) {
                    abp.message.confirm(
                        app.localize('Xóa Hồ sơ công việc'),
                        app.localize('Bạn có chắc không'),
                        (isConfirmed) => {
                            if (isConfirmed) {
                                _profileWorkService
                                    .delete(btnFilter)
                                    .done((result2) => {
                                        $("#workGroup" + result2).remove();
                                        $("#workGroup" + isParent).children('.card').children('.card-header').children('a').children('.icon-actions').remove();
                                        abp.notify.success(app.localize('Xóa Hồ sơ Công việc thành công'));
                                    });
                            }
                        }
                    );
                }
                else {
                    abp.message.confirm(
                        app.localize('Xóa Hồ sơ công việc'),
                        app.localize('Bạn có chắc không'),
                        (isConfirmed) => {
                            if (isConfirmed) {
                                _profileWorkService
                                    .delete(btnFilter)
                                    .done((result2) => {
                                        $("#workGroup" + result2).remove();
                                        abp.notify.success(app.localize('Xóa Hồ sơ Công việc thành công'));
                                    });
                            }
                        }
                    );
                }
            }
        })
    }

    //function update Item
    function editItemWhenClick() {
        $(".btn-edit").click(function () {
            var btnFilter = $(this).attr('data-edit');
            var dataFilter = { Id: btnFilter };
            console.log($(this).parent('div').parent('div').children('a').children('.item_name').text("aaaaa"));

            console.log(btnFilter);
            _EditModal.open(dataFilter, function (callback) {
                console.log(callback);
                /*  EditList(callback);*/
                /*$(this).parent('div').parent('div').children('a').children('.item_name').text("dddd");*/
                $(".item_name" + callback.Id).text(callback.Title);
            });
        })
    }


    //Load All Item Root
    function LoadParent() {
        _profileWorkService.getAllListParent().done(function (result1) {
            var html = "";
            $.each(result1.items, function (index, value) {
                var numindex = index + 1;
                if (value.numItemsChild == 0) {
                    html = `
                    <div class="accordion mt-2" id="workGroup`+ value.id + `">
                        <div class="card">
                            <div class="card-header d-flex align-items-center">
                                <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ value.id + `" aria-expanded="false">
                                   <span class="font-weight-bold text-index mr-1 indexing`+ value.id + `">` + numindex + `.</span> 
                                    <span class="item_name`+ value.id + `">` + value.title + `</span>
                                </a>
                                <div class="w-25 text-right mr-2">
                                    <button class='btn p-2 btn-create'  data-create='`+ value.id + `' >
                                        <i class="far fa-plus-square fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-edit'  data-edit='`+ value.id + `' >
                                        <i class="fal fa-edit fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-view'  data-view='`+ value.id + `' >
                                        <i class="fal fa-eye fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-delete'  data-delete='`+ value.id + `' data-parentId='0'>
                                        <i class="fas fa-trash fz-11"></i>
                                    </button>
                                </div>
                            </div>
                            <div id="js_accordion-`+ value.id + `" class="collapse pl-4" data-parent="#workGroup` + value.id + `">

                            </div>
                        </div>
                    </div>
                    `
                }
                else {
                    html = `
                    <div class="accordion mt-2" id="workGroup`+ value.id + `">
                        <div class="card">
                            <div class="card-header d-flex align-items-center">
                                <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ value.id + `" aria-expanded="false">
                                    <span class="font-weight-bold text-index mr-1 indexing`+ value.id + `">` + numindex + `.</span>
                                    <span class="item_name`+ value.id + `">` + value.title + `</span>
                                        <span class="ml-3 pt-1 icon-actions">
                                            <span class="collapsed-reveal">
                                                <i class="fal fa-minus-circle text-danger fs-xl"></i>
                                            </span>
                                            <span class="collapsed-hidden">
                                                <i class="fal fa-plus-circle text-success fs-xl"></i>
                                            </span>
                                        </span>
                                </a>
                                <div class="w-25 text-right mr-2">
                                    <button class='btn p-2 btn-create'  data-create='`+ value.id + `' >
                                        <i class="far fa-plus-square fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-edit'  data-edit='`+ value.id + `' >
                                        <i class="fal fa-edit fz-11"></i>
                                    </button>
                                      <button class='btn p-2 btn-view'  data-view='`+ value.id + `' >
                                        <i class="fal fa-eye fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-delete'  data-delete='`+ value.id + `' data-parentId='0'>
                                        <i class="fas fa-trash fz-11"></i>
                                    </button>
                                </div>
                            </div>
                            <div id="js_accordion-`+ value.id + `" class="collapse pl-4" data-parent="#workGroup` + value.id + `">

                            </div>
                        </div>
                    </div>
                    `
                }
                $("#tree_workProfile").append(html);

                LoadItem(value.id);

            })

            //Edit
            editItemWhenClick();
            //Create
            createItemWhenClick();

            //Delete
            deleteItem();

            ViewDetailWhenClick();

        })
    }

    //Load All Item Chilren
    function LoadItem(idchar) {
        var html1 = "";
        var textindexing = "";
        textindexing = $(".indexing" + idchar).text();
        _profileWorkService.getAllListItem(idchar).done(function (result2) {
            if (result2.items.length == 0) {
                html1 = null;
            }
            else {
                $.each(result2.items, function (index, value) {
                    var tangindex = index + 1;
                    if (value.numItemsChild == 0) {
                        html1 = `
                                <div class="accordion mt-2" id="workGroup`+ value.id + `" class="mt-1">
                                    <div class="card">
                                        <div class="card-header d-flex align-items-center">
                                            <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ value.id + `" aria-expanded="false">
                                                <span class="font-weight-bold text-index mr-1 indexing`+ value.id + `">` + textindexing + `` + tangindex + `.</span>
                                                <span class="item_name`+ value.id + `">` + value.title + `</span>
                                            </a>
                                            <div class="w-25 text-right mr-2">
                                                <button class='btn p-2 btn-create`+ value.id + `'  data-create='`+ value.id + `' >
                                                    <i class="far fa-plus-square fz-11"></i>
                                                </button>
                                                <button class='btn p-2 btn-edit'  data-edit='`+ value.id + `' >
                                                    <i class="fal fa-edit fz-11"></i>
                                                </button>
                                                 <button class='btn p-2 btn-view'  data-view='`+ value.id + `' >
                                                      <i class="fal fa-eye fz-11"></i>
                                                 </button>
                                                <button class='btn p-2 btn-delete'  data-delete='`+ value.id + `' data-parentId='` + idchar + `'>
                                                    <i class="fas fa-trash fz-11"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                        <div id="js_accordion-`+ value.id + `" class="collapse pl-4" data-parent="#workGroup` + value.id + ` ">
                                
                                        </div>
                                </div>
                            `
                    }
                    else {
                        html1 = `
                                <div class="accordion mt-2" id="workGroup`+ value.id + `" class="mt-1">
                                    <div class="card">
                                        <div class="card-header d-flex align-items-center">
                                            <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ value.id + `" aria-expanded="false">
                                                <span class="font-weight-bold text-index mr-1 indexing`+ value.id + `">` + textindexing + `` + tangindex + `.</span>
                                                <span class="item_name`+ value.id + `">` + value.title + `</span>
                                                <span class="ml-3 pt-1 icon-actions">
                                                    <span class="collapsed-reveal">
                                                        <i class="fal fa-minus-circle text-danger fs-xl"></i>
                                                    </span>
                                                    <span class="collapsed-hidden">
                                                        <i class="fal fa-plus-circle text-success fs-xl"></i>
                                                    </span>
                                                </span>
                                            </a>
                                            <div class="w-25 text-right mr-2">
                                                <button class='btn p-2 btn-create`+ value.id + `'  data-create='`+ value.id + `' >
                                                    <i class="far fa-plus-square fz-11"></i>
                                                </button>
                                                <button class='btn p-2 btn-edit'  data-edit='`+ value.id + `' >
                                                    <i class="fal fa-edit fz-11"></i>
                                                </button>
                                                <button class='btn p-2 btn-view'  data-view='`+ value.id + `' >
                                                    <i class="fal fa-eye fz-11"></i>
                                                 </button>
                                                <button class='btn p-2 btn-delete'  data-delete='`+ value.id + `' >
                                                    <i class="fas fa-trash fz-11"></i>
                                                </button>
                                            </div>
                                            </div>
                                        </div>
                                        <div id="js_accordion-`+ value.id + `" class="collapse pl-4" data-parent="#workGroup` + value.id + ` ">
                                
                                        </div>
                                </div>
                            `
                    }
                    $("#js_accordion-" + idchar).append(html1);

                    LoadItem(value.id);

                    $(".btn-create" + value.id).click(function () {
                        console.log("âppsospdpdpd");
                        var btnFilter = $(this).attr('data-create');
                        var dataFilter = { Id: btnFilter };
                        _createWorkitemModal.open(dataFilter, function (callback) {
                            console.log(callback);
                            CreateChilren(callback);
                        });
                    })

                })
                ////Create
                //createItemWhenClick();
                //Edit
                editItemWhenClick();

                //Delete
                deleteItem();

                ViewDetailWhenClick();

            }
        })
    }

    LoadParent();

    //---------------------------------------------- / LOAD LIST WORKPROFILE -------------------------------------------------



    $('#CreateNewButtonxx').click(function () {
        console.log("ádsa");
        _createModal.open(1, function (callback) {
            CreateRoot(callback);
        });
    });


    //Function Create Root
    function CreateRoot(callback) {
        var numlength = $("#tree_workProfile").children('.accordion').length + 1;

        html = `
                    <div class="accordion mt-2" id="workGroup`+ callback.Id + `">
                        <div class="card">
                            <div class="card-header d-flex align-items-center">
                                <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ callback.Id + `" aria-expanded="false">
                                   <span class="font-weight-bold text-index mr-1 indexing`+ callback.Id + `">` + numlength + `.</span>
                                   <span class="item_name`+ callback.Id + `">` + callback.Title + `</span>
                                </a>
                                <div class="w-25 text-right mr-2">
                                    <button class='btn p-2 btn-create'  data-create='`+ callback.Id + `' >
                                        <i class="far fa-plus-square fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-edit'  data-edit='`+ callback.Id + `' >
                                        <i class="fal fa-edit fz-11"></i>
                                    </button>
                                      <button class='btn p-2 btn-view'  data-view='`+ callback.Id + `' >
                                        <i class="fal fa-eye fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-delete'  data-delete='`+ callback.Id + `' data-parentId='0'>
                                        <i class="fas fa-trash fz-11"></i>
                                    </button>
                                </div>
                            </div>
                            <div id="js_accordion-`+ callback.Id + `" class="collapse pl-4" data-parent="#workGroup` + callback.Id + `">

                            </div>
                        </div>
                    </div>
                    `
        $("#tree_workProfile").append(html);

        //Create
        createItemWhenClick();

        //Edit
        editItemWhenClick();

        //Delete
        deleteItem();

        ViewDetailWhenClick();
    }

    //Function Create Chilren Items
    function CreateChilren(callback) {

        var textindexing = $(".indexing" + callback.ParentId).text();
        var textindexitem = $("#js_accordion-" + callback.ParentId).children('.accordion').length + 1;
        html = `
                    <div class="accordion mt-2" id="workGroup`+ callback.Id + `">
                        <div class="card">
                            <div class="card-header d-flex align-items-center">
                                <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ callback.Id + `" aria-expanded="false">
                                   <span class="font-weight-bold text-index mr-1 indexing`+ callback.Id + `">` + textindexing + `` + textindexitem + `.</span>
                                   <span class="item_name`+ callback.Id + `">` + callback.Title + `</span>
                                </a>
                                <div class="w-25 text-right mr-2">
                                    <button class='btn p-2 btn-create'  data-create='`+ callback.Id + `' >
                                        <i class="far fa-plus-square fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-edit'  data-edit='`+ callback.Id + `' >
                                        <i class="fal fa-edit fz-11"></i>
                                    </button>
                                     <button class='btn p-2 btn-view'  data-view='`+ callback.Id + `' >
                                        <i class="fal fa-eye fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-delete'  data-delete='`+ callback.Id + `' data-parentId='` + callback.ParentId + `'>
                                        <i class="fas fa-trash fz-11"></i>
                                    </button>
                                </div>
                            </div>
                            <div id="js_accordion-`+ callback.Id + `" class="collapse pl-4" data-parent="#workGroup` + callback.Id + `">

                            </div>
                        </div>
                    </div>
                    `
        var numlength = $("#js_accordion-" + callback.ParentId).children('.accordion').length;
        if (numlength == 0) {
            var htmlaction = `
                                <span class="ml-3 pt-1 icon-actions">
                                    <span class="collapsed-reveal">
                                        <i class="fal fa-minus-circle text-danger fs-xl"></i>
                                    </span>
                                    <span class="collapsed-hidden">
                                        <i class="fal fa-plus-circle text-success fs-xl"></i>
                                    </span>
                                </span>
                                `;

            $("#workGroup" + callback.ParentId).children('.card').children('.card-header').children('a').append(htmlaction);
            $("#js_accordion-" + callback.ParentId).append(html);
        }
        else {
            $("#js_accordion-" + callback.ParentId).append(html);
        }

        //Create
        createItemWhenClick();

        //Edit
        editItemWhenClick();

        //Delete
        deleteItem();

        ViewDetailWhenClick();

    }



    //Click xem chi tiết
    function ViewDetailWhenClick() {
        $(".btn-view").click(function () {
            var btnFilter = $(this).attr('data-view');
            var dataFilter = { Id: btnFilter };
            console.log("11", dataFilter)
            abp.ajax({
                contentType: 'application/x-www-form-urlencoded',
                url: "ProfileWork/View/?Id=" + dataFilter.Id,
                success: (function (results) {
                    window.location.href = "/Inventorys/ProfileWork/ViewDetails?id=" + results.id;
                })
            })
        })
    }

})(jQuery);