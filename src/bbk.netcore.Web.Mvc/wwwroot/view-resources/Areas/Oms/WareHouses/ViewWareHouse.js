(function () {
    var _wareHouse = abp.services.app.wareHouse;
    var _warehouseTypesService = abp.services.app.warehouseTypesService;
    var _warehouseItemsService = abp.services.app.warehouseItem
    moment.locale(abp.localization.currentLanguage.name);


    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/WareHouse/CreateWarhouseItem',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/WareHouses/CreateItem.js',
        modalClass: 'CreatWarModal',
        modalType: 'modal-xl'

    });


    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/WareHouse/EditWarhouseItem',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/WareHouses/EditItem.js',
        modalClass: 'EditWarModal',
        modalType: 'modal-xl'
    });

    var _createModalItem = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/WareHouse/CreateWarhouseItemSub',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/WareHouses/CreateItemSub.js',
        modalClass: 'CreatItemsSub',
        modalType: 'modal-xl'

    });

    //------------------------- Load Name Address -----------------
    fullPathProvince = 'province.json';
    fullPathDistrict = 'district.json';
    fullPathVillage = 'village.json';

    function LoadAddress(filePath, idAddress, idSet, divview) {
        _wareHouse.getAddress(filePath, idAddress).done((result) => {
            for (let i = 0; i < result.addresses.length; i++) {
                if (result.addresses[i].id == idSet) {
                    $(divview).html(result.addresses[i].name);
                }
            }
        })
    }

    LoadAddress(fullPathProvince, "", $("#CityId").val(), "#CityName");
    LoadAddress(fullPathDistrict, $("#CityId").val(), $("#DistrictId").val(), "#DistrictName");
    LoadAddress(fullPathVillage, $("#DistrictId").val(), $("#WardsId").val(), "#VillageName");

    //----------------------------- Load Loại kho -----------------------------
    _warehouseTypesService.get({ id: $("#TypeCode").val() }).done((result1) => {

        $("#TypeWare").html(result1.name);
    })



      //------------------------------  Load item --------------------------------

    var _permissions = {

        edit: abp.auth.hasPermission('Inventorys.WareHouse.Layout'),
    };

    //Edit Root
    function editRootWhenClick() {
        $(".btn-edit-parent").click(function () {
            if (_permissions.edit) {
                var btnFilter = $(this).attr('data-edit');
                var dataFilter = { Id: btnFilter };
                _EditModal.open(dataFilter, function (callback) {
                    if (callback.ParrentId == 0) {
                        $(".item_name" + callback.Id).text(callback.Name);
                    }
                    else {
                        $(".item_names" + callback.Id).text(callback.Name);
                    }
                });
            }
            else {
                abp.notify.error('Bạn không có quyền vào mục này');
            }
         
        })
    }

    function editItemWhenClick() {
        $(".btn-edit").click(function () {
            if (_permissions.edit) {
                var btnFilter = $(this).attr('data-edit');
                var dataFilter = { Id: btnFilter };
                _EditModal.open(dataFilter, function (callback) {
                    $(".item_names" + callback.Id).text(callback.Name);
                });
            }
            else
            {
                abp.notify.error('Bạn không có quyền vào mục này');
            }
           
        })
    }

    function editItemSubWhenClick() {
        $(".btn-edit").click(function () {
            if (_permissions.edit) {
                var btnFilter = $(this).attr('data-edit');
                var dataFilter = { Id: btnFilter };
                _EditModal.open(dataFilter, function (callback) {
                    $(".item_name" + callback.Id).text(callback.Name);
                });
            }
            else
            {
                abp.notify.error('Bạn không có quyền vào mục này');
            }
        })
    }


    //Tạo item con
    function createItemWhenClick() {
        $(".btn-create-parent").click(function () {
            if (_permissions.edit) {
                var btnFilter = $(this).attr('data-create');
                var dataFilter = { Id: btnFilter };
                _createModalItem.open(dataFilter, function (callback) {
                    CreateItems(callback);
                });
            }
            else {
                abp.notify.error('Bạn không có quyền vào mục này');
            }
        })
    }

    //Tạo item con phan cap
    function createItemSubWhenClick() {
        $(".btn-create").click(function () {
            if (_permissions.edit) {
                var btnFilter = $(this).attr('data-create');
                var dataFilter = { Id: btnFilter };
                _createModalItem.open(dataFilter, function (callback) {
                    CreateItemSub(callback);
                });
            }
            else {
                abp.notify.error('Bạn không có quyền vào mục này');
            }
        })
    }

    // Xóa phần tử lớn nhất
    function deleteItem() {
        $(".btn-delete").click(function () {
            if (_permissions.edit) {
                var btnFilter = $(this).attr('data-delete');
                abp.message.confirm(
                    app.localize('Xóa'),
                    app.localize('Bạn có chắc không'),
                    (isConfirmed) => {
                        if (isConfirmed) {
                            _warehouseItemsService
                                .delete(btnFilter)
                                .done((result1) => {
                                    $("#wareItem" + result1).remove();
                                    $("#NameParentItem").html("");
                                    abp.notify.success(app.localize('Xóa thành công'));
                                });
                        }
                    }
                );
            }
            else {
                abp.notify.error('Bạn không có quyền vào mục này');
            }

        })
    } 

    // Xóa phần tử con
    function deleteItemSub() {
        $(".btn-delete").click(function () {
            if (_permissions.edit) {
                var btnFilter = $(this).attr('data-delete');
                var isParent = $(this).attr('data-parentId');
                var numlength = $("#js_accordion-" + isParent).children('.accordion').length;
                console.log("oapap", numlength);
                if (numlength == 0) {
                    abp.message.confirm(
                        app.localize('Xóa'),
                        app.localize('Bạn có chắc không'),
                        (isConfirmed) => {
                            if (isConfirmed) {
                                _warehouseItemsService
                                    .delete(btnFilter)
                                    .done((result1) => {
                                        $("#workGroup" + result1).remove();
                                        abp.notify.success(app.localize('Xóa thành công'));
                                    });
                            }
                        }
                    );
                }
                else {
                    abp.message.confirm(
                        app.localize('Xóa'),
                        app.localize('Bạn có chắc không'),
                        (isConfirmed) => {
                            if (isConfirmed) {
                                _warehouseItemsService
                                    .delete(btnFilter)
                                    .done((result1) => {
                                        $("#workGroup" + result1).remove();
                                        $("#workGroup" + isParent).children('.card').children('.card-header').children('a').children('.icon-actions').remove();
                                        abp.notify.success(app.localize('Xóa thành công'));
                                    });
                            }
                        }
                    );
                }
            }
            else
            {
                abp.notify.error('Bạn không có quyền vào mục này');
            }
        })
    } 

    function clickView() {
        $(".list-block_item-link").click(function () {
            $(".list-block_item-link.active").removeClass("active");
            $(this).addClass("active");
            var btnFilter = $(this).attr('data-view');
            var textheader = $(this).text();
            $("#NameParentItem").html(textheader);
            LoadItemContent(btnFilter);
        })
    }

    //Load ra Root
    function LoadParent() {
        var url = window.location.href;
        var id = url.substring(url.lastIndexOf('=') + 1);
        _warehouseItemsService.getAll(id).done(function (result1) {
            var html = "";
            $.each(result1.items, function (index, value) {
                html = `
                     <li class="list-block_item" id="wareItem`+ value.id + `">
                         <a class="list-block_item-link item_name`+ value.id + `" data-view='` + value.id + `' href="javascript:void(0);">`+ value.name + `</a>
                          <div class="list-block-item_action">
                            <a class="btn-create-parent" data-create='`+ value.id + `' href="javascript:void(0);">
                                   <i class="fal fa-plus-square"></i>
                             </a>
                             <a class="btn-edit-parent ml-2 mr-2" data-edit='`+ value.id + `' href="javascript:void(0);">
                                  <i class="fal fa-edit"></i>
                             </a>
                             <a class="btn-delete" data-delete='`+ value.id + `' href="javascript:void(0);">
                                   <i class="fal fa-trash-alt"></i>
                             </a>
                       </div>
                   </li>
                    `
                $("#tree_block").append(html);
            })


            editRootWhenClick();

            deleteItem();
            createItemWhenClick();

            clickView();

        })
    }

    //Load Item 
    function LoadItemContent(idItems) {
        var html = "";
        _warehouseItemsService.getAllItemRoot(idItems).done(function (result2) {
            $("#content-block").empty();
            if (result2.items.length == 0) {
                html = null;
            }
            else {
                $.each(result2.items, function (index, value) {

                    html = `
                                <div class="accordion mt-2" id="workGroup`+ value.id + `" class="mt-1">
                                    <div class="card">
                                        <div class="card-header d-flex align-items-center">
                                            <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ value.id + `" aria-expanded="false">
                            
                                                <span class="item_names`+ value.id + `">` + value.name + `</span>
                                            </a>
                                            <div class="w-25 text-right mr-2">
                                                <button class='btn p-2 btn-create`+ value.id + `' data-create='` + value.id + `' >
                                                    <i class="far fa-plus-square fz-11"></i>
                                                </button>
                                                <button class='btn p-2 btn-edit`+ value.id + `'  data-edit='`+ value.id + `' >
                                                    <i class="fal fa-edit fz-11"></i>
                                                </button>
                                                <button class='btn p-2 btn-delete'  data-delete='`+ value.id + `' data-parentId='` + idItems + `'>
                                                    <i class="fas fa-trash fz-11"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                        <div id="js_accordion-`+ value.id + `" class="collapse pl-4" data-parent="#workGroup` + value.id + ` ">
                                
                                        </div>
                                </div>
                            `;
                    
                    $("#content-block").append(html);

                    LoadItemSubContent(value.id);

                    $(".btn-create" + value.id).click(function () {
                        if (_permissions.edit) {
                            var btnFilter = $(this).attr('data-create');
                            var dataFilter = { Id: btnFilter };
                            _createModalItem.open(dataFilter, function (callback) {
                                CreateItemSub(callback);
                            });
                        }
                        else {
                            abp.notify.error('Bạn không có quyền vào mục này');
                        }
                    })

                    $(".btn-edit" + value.id).click(function () {
                        if (_permissions.edit) {
                            var btnFilter = $(this).attr('data-edit');
                            var dataFilter = { Id: btnFilter };
                            _EditModal.open(dataFilter, function (callback) {
                                $(".item_names" + callback.Id).text(callback.Name);
                            });
                        }
                        else {
                            abp.notify.error('Bạn không có quyền vào mục này');
                        }
                    })
                })
            }

            deleteItemSub();
            /*editItemWhenClick();*/
        })
    }

    //Load ItemSub
    function LoadItemSubContent(iditemsub) {
        var html1 = "";
        _warehouseItemsService.getAllItemSub(iditemsub).done(function (result3) {
            if (result3.items.length == 0) {
                html1 = null;
            }
            else {
                $.each(result3.items, function (index, value) {
                    
                        html1 = `
                                <div class="accordion mt-2" id="workGroup`+ value.id + `" class="mt-1">
                                    <div class="card">
                                        <div class="card-header d-flex align-items-center">
                                            <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ value.id + `" aria-expanded="false">
                                                <span class="item_name`+ value.id + `">` + value.name + `</span>
                                            </a>
                                            <div class="w-25 text-right mr-2">
                                                <button class='btn p-2 btn-create`+ value.id + `'  data-create='` + value.id + `' >
                                                    <i class="far fa-plus-square fz-11"></i>
                                                </button>
                                                <button class='btn p-2 btn-edit`+ value.id + `'  data-edit='`+ value.id + `' >
                                                    <i class="fal fa-edit fz-11"></i>
                                                </button>
                                                <button class='btn p-2 btn-delete'  data-delete='`+ value.id + `' data-parentId='` + iditemsub + `'>
                                                    <i class="fas fa-trash fz-11"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                        <div id="js_accordion-`+ value.id + `" class="collapse pl-4" data-parent="#workGroup` + value.id + ` ">
                                
                                        </div>
                                </div>
                            `

                    var htmlLimit = `
                                <div class="accordion mt-2" id="workGroup`+ value.id + `" class="mt-1">
                                    <div class="card">
                                        <div class="card-header d-flex align-items-center">
                                            <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ value.id + `" aria-expanded="false">
                                                <span class="item_name`+ value.id + `">` + value.name + `</span>
                                            </a>
                                            <div class="w-25 text-right mr-2">
                                                <button class='btn p-2 btn-edit`+ value.id + `'  data-edit='` + value.id + `' >
                                                    <i class="fal fa-edit fz-11"></i>
                                                </button>
                                                <button class='btn p-2 btn-delete'  data-delete='`+ value.id + `' data-parentId='` + iditemsub + `'>
                                                    <i class="fas fa-trash fz-11"></i>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                        <div id="js_accordion-`+ value.id + `" class="collapse pl-4" data-parent="#workGroup` + value.id + ` ">
                                
                                        </div>
                                </div>
                            `

                    if (value.warehouseLevel < 3) {
                        $("#js_accordion-" + iditemsub).append(html1);
                    }
                    else {
                        $("#js_accordion-" + iditemsub).append(htmlLimit);
                    }

                    LoadItemSubContent(value.id);

                    $(".btn-create" + value.id).click(function () {
                        if (_permissions.edit) {
                            var btnFilter = $(this).attr('data-create');
                            var dataFilter = { Id: btnFilter };
                            _createModalItem.open(dataFilter, function (callback) {
                                CreateItemSub(callback);
                            });
                        }
                        else {
                            abp.notify.error('Bạn không có quyền vào mục này');
                        }
                    })

                    $(".btn-edit" + value.id).click(function () {
                        if (_permissions.edit) {
                            var btnFilter = $(this).attr('data-edit');
                            var dataFilter = { Id: btnFilter };
                            _EditModal.open(dataFilter, function (callback) {
                                $(".item_name" + callback.Id).text(callback.Name);
                            });
                        }
                        else {
                            abp.notify.error('Bạn không có quyền vào mục này');
                        }
                    })

                })
                ////Create
                //createItemWhenClick();
                //Edit
                var numlength = $("#js_accordion-" + iditemsub).children('.accordion').length;
                var isIconAction = $("#workGroup" + iditemsub).children('.card').children('.card-header').children('a').children('span.icon-actions').length;
                console.log("sadada", isIconAction);
                if (numlength > 0 && isIconAction == 0) {
                        var htmlaction = `
                                <span class="ml-3 pt-1 icon-actions">
                                    <span class="collapsed-reveal">
                                        <i class="fal fa-minus-circle text-danger-ct fs-xl"></i>
                                    </span>
                                    <span class="collapsed-hidden">
                                        <i class="fal fa-plus-circle text-success-ct fs-xl"></i>
                                    </span>
                                </span>
                                `;

                        $("#workGroup" + iditemsub).children('.card').children('.card-header').children('a').append(htmlaction);
                    }
                deleteItemSub();
                /*editItemWhenClick();*/

            }
        })
    }

  
    $('#CreateItem').click(function () {
        _createModal.open(1, function (callback) {
            CreateRoot(callback);
        });
    });


    //Function Create Root
    function CreateRoot(callback) {
        html = `
              <li class="list-block_item" id="wareItem`+ callback.Id + `" >
                <a class="list-block_item-link item_name`+ callback.Id+`" href="javascript:void(0);">`+ callback.Name + `</a>
                <div class="list-block-item_action">
                    <a class="btn-create-parent`+ callback.Id + `" data-create='` + callback.Id + `' href="javascript:void(0);">
                        <i class="fal fa-plus-square"></i>
                    </a>
                    <a class="btn-edit-parent ml-2 mr-2" data-edit='`+ callback.Id + `' href="javascript:void(0);">
                        <i class="fal fa-edit"></i>
                    </a>
                    <a class="btn-delete" data-delete='`+ callback.Id + `' href="javascript:void(0);">
                        <i class="fal fa-trash-alt"></i>
                    </a>
                </div>
            </li>
        `;
        $("#tree_block").append(html);

        $(".btn-create-parent" + callback.Id).click(function () {
            var btnFilter = $(this).attr('data-create');
            var dataFilter = { Id: btnFilter };
            _createModalItem.open(dataFilter, function (callback) {
                CreateItems(callback);
            });
        })

        editRootWhenClick();

        deleteItem();

        clickView();

    }

    //Function Create Item
    function CreateItems(callback) {
        html = `
                <div class="accordion mt-2" id="workGroup`+ callback.Id + `" class="mt-1">
                    <div class="card">
                        <div class="card-header d-flex align-items-center">
                            <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ callback.Id + `" aria-expanded="false">
                            
                                <span class="item_names`+ callback.Id + `">` + callback.Name + `</span>
                            </a>
                            <div class="w-25 text-right mr-2">
                                <button class='btn p-2 btn-create'  data-create='` + callback.Id + `' >
                                    <i class="far fa-plus-square fz-11"></i>
                                </button>
                                <button class='btn p-2 btn-edit'  data-edit='`+ callback.Id + `' >
                                    <i class="fal fa-edit fz-11"></i>
                                </button>
                                <button class='btn p-2 btn-delete'  data-delete='`+ callback.Id + `' data-parentId='` + callback.ParrentId + `'>
                                    <i class="fas fa-trash fz-11"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                        <div id="js_accordion-`+ callback.Id + `" class="collapse pl-4" data-parent="#workGroup` + callback.Id + ` ">
                                
                        </div>
                </div>
            `;

        $("#content-block").append(html);

        createItemSubWhenClick();

        deleteItemSub();

        editItemWhenClick();

        console.log("chay vao day");
    }

    //Function Create ItemSub
    function CreateItemSub(callback) {
        console.log(callback);

        //Nếu cấp phần tử con < 4
        if (callback.WarehouseLevel < 3) {
            html = `
                    <div class="accordion mt-2" id="workGroup`+ callback.Id + `">
                        <div class="card">
                            <div class="card-header d-flex align-items-center">
                                <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ callback.Id + `" aria-expanded="false">
                                   <span class="item_name`+ callback.Id + `">` + callback.Name + `</span>
                                </a>
                                <div class="w-25 text-right mr-2">
                                    <button class='btn p-2 btn-create'  data-create='`+ callback.Id + `' >
                                        <i class="far fa-plus-square fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-edit'  data-edit='`+ callback.Id + `' >
                                        <i class="fal fa-edit fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-delete'  data-delete='`+ callback.Id + `' data-parentId='` + callback.ParrentId + `'>
                                        <i class="fas fa-trash fz-11"></i>
                                    </button>
                                </div>
                            </div>
                            <div id="js_accordion-`+ callback.Id + `" class="collapse pl-4" data-parent="#workGroup` + callback.Id + `">

                            </div>
                        </div>
                    </div>
                    `;

            var numlength = $("#js_accordion-" + callback.ParrentId).children('.accordion').length;
            if (numlength == 0) {
                var htmlaction = `
                                <span class="ml-3 pt-1 icon-actions">
                                    <span class="collapsed-reveal">
                                        <i class="fal fa-minus-circle text-danger-ct fs-xl"></i>
                                    </span>
                                    <span class="collapsed-hidden">
                                        <i class="fal fa-plus-circle text-success-ct fs-xl"></i>
                                    </span>
                                </span>
                                `;

                $("#workGroup" + callback.ParrentId).children('.card').children('.card-header').children('a').append(htmlaction);
                $("#js_accordion-" + callback.ParrentId).append(html);
            }
            else {
                $("#js_accordion-" + callback.ParrentId).append(html);
            }
        }

        // nếu lớn hơn 3
        else {
            var htmlLimit = `
                    <div class="accordion mt-2" id="workGroup`+ callback.Id + `">
                        <div class="card">
                            <div class="card-header d-flex align-items-center">
                                <a href="javascript:void(0);" class="card-title text-title d-flex w-75" data-toggle="collapse" data-target="#js_accordion-`+ callback.Id + `" aria-expanded="false">
                                   <span class="item_name`+ callback.Id + `">` + callback.Name + `</span>
                                </a>
                                <div class="w-25 text-right mr-2">
                                    <button class='btn p-2 btn-edit'  data-edit='`+ callback.Id + `' >
                                        <i class="fal fa-edit fz-11"></i>
                                    </button>
                                    <button class='btn p-2 btn-delete'  data-delete='`+ callback.Id + `' data-parentId='` + callback.ParrentId + `'>
                                        <i class="fas fa-trash fz-11"></i>
                                    </button>
                                </div>
                            </div>
                            <div id="js_accordion-`+ callback.Id + `" class="collapse pl-4" data-parent="#workGroup` + callback.Id + `">

                            </div>
                        </div>
                    </div>
                    `;

            var numlength = $("#js_accordion-" + callback.ParrentId).children('.accordion').length;
            if (numlength == 0) {
                var htmlaction = `
                                <span class="ml-3 pt-1 icon-actions">
                                    <span class="collapsed-reveal">
                                        <i class="fal fa-minus-circle text-danger-ct fs-xl"></i>
                                    </span>
                                    <span class="collapsed-hidden">
                                        <i class="fal fa-plus-circle text-success-ct fs-xl"></i>
                                    </span>
                                </span>
                                `;

                $("#workGroup" + callback.ParrentId).children('.card').children('.card-header').children('a').append(htmlaction);
                $("#js_accordion-" + callback.ParrentId).append(htmlLimit);
            }
            else {
                $("#js_accordion-" + callback.ParrentId).append(htmlLimit);
            }
        }

        createItemSubWhenClick();
        deleteItemSub();

        editItemSubWhenClick();
    }
   
    LoadParent();

    //----------------- View Warehouse ----------------
    $('#btn-viewlayout').click(function (e) {
        var btnClick = $(this);
        var dataFilter = { id: $(this).attr("data-viewlayout"), };

        abp.ajax({
            contentType: "application/x-www-form-urlencoded",
            url: abp.appPath + "Inventorys/WareHouse/OverView?Id=" + dataFilter.id,
            success: function (results) {
                console.log(results);
                window.location.href =
                    "/Inventorys/WareHouse/ViewWareHouseLayout?id=" + results.id;
            },
        });
    });

})(jQuery);