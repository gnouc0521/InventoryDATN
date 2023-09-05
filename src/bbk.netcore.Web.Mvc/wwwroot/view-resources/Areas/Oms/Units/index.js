(function () {
    var _$unitsTable = $('#unitsTable');
    var _unitsService = abp.services.app.unitService;
    moment.locale(abp.localization.currentLanguage.name);

    var _createModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Units/CreateUnits',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Units/_CreateModal.js',
        modalClass: 'UnitsCreateModal',
        modalType: 'modal-xl'

    });

    var _EditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Inventorys/Units/EditUnitsModal',
        scriptUrl: abp.appPath + 'view-resources/Areas/Oms/Units/_EditModal.js',
        modalClass: 'UnitsEditModal',
        modalType: 'modal-xl'
    });

    $('#CreateNewButtonxx').click(function () {
        var btnClick = $(this);
        var dataFilter = { id: null };
        _createModal.open(dataFilter, function (callback) {
            $('.tbody_hidden').hide();
            addhtmlunits(callback);
            CreateModal();
        });


    });

    /* DEFINE TABLE */
   
    function deleteUnits() {
        $('.btnDelete').click(function () {
            var btnClick = $(this);
            var point = btnClick.closest('.card-point');
            console.log(point)
            btnClick
            abp.message.confirm(
                app.localize('Xóa đơn vị tính', "Đơn vị tính"),
                app.localize('Bạn có chắc không'),
                (isConfirmed) => {
                    if (isConfirmed) {
                        _unitsService
                            .delete(btnClick[0].dataset.units)
                            .done(() => {
                                point.empty();
                                abp.notify.success(app.localize('Xóa đơn vị tính thành công'));

                            });
                    }
                }
            );
        })
    }

    $('#Search').click(function (e) {
        loadUnitParrent();
    });

    function getDocs() {

        dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
        $("#SearchTerm").focus();
    });
    function CreateModal() {
        $('.btnCreate').click(function () {
            var btnClick = $(this);
            var dataFilter = { id: btnClick[0].dataset.units };
            _createModal.open(dataFilter, function (callback) {
                loadUnitsChil(btnClick[0].dataset.units)
            });
        });

    }
    function EditModal() {
        $('.btnEdit').click(function () {
            var btnClick = $(this);
            var dataFilter = { id: btnClick[0].dataset.units };
            _EditModal.open(dataFilter, function (callback) {
                var abc = btnClick.closest('.card-header').children('a').children('p').text(callback.Name);
            });
        });
    }
    function addhtmlunits(value) {
        html = `<div class="card card-point pl-2 pr-2 pt-1">
                            <div class="card-header  d-flex" >
                                <a href="javascript:void(0);" class="p-0 card-title collapsed text-left" data-toggle="collapse" data-target="#collapse`+ value.id + `" aria-expanded="false" >
                                    <span class="ml-3 mr-3">
                                        <span class="collapsed-reveal">
                                            <i class="fal fa-minus-circle text-danger"></i>
                                        </span>
                                        <span class="collapsed-hidden">
                                            <i class="fal fa-plus-circle text-success"></i>
                                        </span>
                                    </span>
                                    <p class='mt-0 mb-0'> `+ value.name + ` </p>
                                   
                                </a>
                                <div class="ml-auto">
                                    <button class="btn btnCreate pr-1" style="font-size: 1rem; " data-units=`+ value.id + `>
                                        <i class="far fa-plus-square"></i>
                                    </button>
                                    <button class="btn btnEdit p-2" style="font-size: 1rem; " data-units=`+ value.id + `>
                                        <i class="fas fa-pencil"></i>
                                    </button>
                                    <button class="btn btnDelete pl-2" style="font-size: 1rem;" data-units=`+ value.id + `>
                                        <i class="fal fa-trash-alt"></i>
                                    </button>
                                </div>
                            </div>
                            <div id="collapse`+ value.id + `" class="collapse " >
                               
                            </div>
                        </div>`
        $('#accordionExample').append(html);

    }

    function loadUnitParrent() {
        $('#accordionExample').empty()
        _unitsService.getAll({ searchTerm: $('#SearchTerm').val().trim() }).done(function (result) {
            if (result.totalCount == 0) {
                $('.tbody_hidden').show();
            } else {
                $('.tbody_hidden').hide();
            }
            $.each(result.items, function (index, value) {
                if (value.parrentId == null) {
                    addhtmlunits(value)
                    loadUnitsChil(value.id);
                }
            });
            CreateModal();
        });
    }
    function loadUnitsChil(ParrentId) {
        var collapse = `#collapse` + ParrentId
        $(collapse).empty();
        _unitsService.getAll({ parrentId: ParrentId }).done(function (result) {
            $.each(result.items, function (index, value) {
                html = ` <div class="card-body p-0 pl-4 pr-4 pt-1 pb-1 card-point">
                                        <div class="card-header d-flex" >
                                            <a href="javascript:void(0);" class="p-0 pl-5 card-title collapsed text-left" data-toggle="collapse"  aria-expanded="false" >
                                               <p class='mt-0 mb-0'> `+ value.name + ` </p>
                                            </a>
                                               <div class="ml-auto">
                                                <button class="btn p-2 btnEdit" style="font-size: 1rem;" data-units=`+ value.id + `>
                                                    <i class="fas fa-pencil"></i>
                                                </button>
                                                <button class="btn pl-2 btnDelete" style="font-size: 1rem;" data-units=`+ value.id + `>
                                                    <i class="fal fa-trash-alt"></i>
                                                </button>
                                            </div>
                                    </div>
                                </div>`
                $(collapse).append(html)
            });
            deleteUnits();
            EditModal();
        });
       
      
    }
    loadUnitParrent();
    
    

})(jQuery);