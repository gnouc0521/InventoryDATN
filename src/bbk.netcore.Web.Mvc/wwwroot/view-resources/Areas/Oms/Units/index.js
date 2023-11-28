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
      _createModal.open();
    });

    /* DEFINE TABLE */
    var getFilter = function () {
      let dataFilter = {};
      dataFilter.searchTerm = $('#SearchTerm').val();
      return dataFilter;
    }

    var dataTable = _$unitsTable.DataTable({
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
        ajaxFunction: _unitsService.getAll,
        inputFilter: getFilter
      },
      order: [[1, 'asc']],
      columnDefs: [

        {
          targets: 0,
          orderable: false,
          className: 'dt-body-center text-center',
          render: function (data, type, row, meta) {
            return '<input type="checkbox" name="" value="' + row.id + '">';
          }
        },
        {
          orderable: true,
          targets: 1,
          data: 'name'
        },
        {
          orderable: true,
          targets: 2,
          data: 'description'
        },
        {
          targets: 3,
          data: 'id',
          orderable: false,
          autoWidth: false,
          className: "text-center",
          render: function (data, type, row, meta) {
            console.log(row)
            return `<a class='btn btn-warning doceditfunc text-right text-white' data-objid='` + row.id + `'href='javascript:void(0); ' > Sửa </a>`;
          }
        },

      ]
    });

    $('#Search').click(function (e) {
      getDocs();
    });

    abp.event.on('app.reloadDocTable', function () {
      getDocs();
    });

    function getDocs() {

      dataTable.ajax.reload();
    }

    jQuery(document).ready(function () {
      $("#SearchTerm").focus();
    });

    $('#example-select-all').on('click', function () {
      // Get all rows with search applied
      var rows = dataTable.rows({ 'search': 'applied' }).nodes();
      // Check/uncheck checkboxes for all rows in the table
      $('input[type="checkbox"]', rows).prop('checked', this.checked);
      $('#DeleteAll').removeAttr('disabled');
      var selected = new Array();
      $('#unitsTable tbody input[type="checkbox"]:checked').each(function () {
        selected.push($(this));
      });
      if (selected.length > 0) {
        $('#DeleteAll').removeAttr('disabled');
      } else {
        $('#DeleteAll').prop('disabled', true);
      }
    });

  // Handle click on checkbox to set state of "Select all" control
  $('#unitsTable tbody').on('change', 'input[type="checkbox"]', function () {
    // If checkbox is not checked
    var selected = new Array();
    $('#unitsTable tbody input[type="checkbox"]:checked').each(function () {
      selected.push($(this));
    });
    if (selected.length > 0) {
      $('#DeleteAll').removeAttr('disabled');
    } else {
      $('#DeleteAll').prop('disabled', true);
    }

    if (!this.checked) {
      var el = $('#example-select-all').get(0);
      console.log(el)
      // If "Select all" control is checked and has 'indeterminate' property
      if (el && el.checked && ('indeterminate' in el)) {
        // Set visual state of "Select all" control
        // as 'indeterminate'
        el.indeterminate = true;
      }
    }
  });

  dataTable.$('input[type="checkbox"]').each(function (index, value) {
    if (value > 0) {
      $('#DeleteAll').removeAttr('disabled');
    }
  })

  $('#DeleteAll').on('click', function (e) {
    abp.message.confirm(
      app.localize('Xóa quy cách', "Quy cách"),
      app.localize('Bạn có chắc không'),
      function (isConfirmed) {
        if (isConfirmed) {
          // Iterate over all checkboxes in the table
          dataTable.$('input[type="checkbox"]').each(function (index, value) {
            if ($(value).is(":checked")) {
              _unitsService.delete(
                $(value).val()
              ).done(function () {
                // getUsers(true);
                $('#example-select-all').prop('checked', false);
                abp.notify.success(app.localize('Xóa quy cách thành công'));
                getDocs();
              });
            }
            // If checkbox doesn't exist in DOM
            // If checkbox is checked
            console.log($(value).val())

          }
          );

        }
      });
  })


    $('#unitsTable').on('click', '.doceditfunc', function (e) {
      var btnClick = $(this);
      var dataFilter = { id: btnClick[0].dataset.objid };
      _EditModal.open(dataFilter);
    });

    //function CreateModal() {
    //    $('.btnCreate').click(function () {
    //        var btnClick = $(this);
    //        var dataFilter = { id: btnClick[0].dataset.units };
    //        _createModal.open(dataFilter, function (callback) {
    //            loadUnitsChil(btnClick[0].dataset.units)
    //        });
    //    });

    //}
    //function EditModal() {
    //    $('.btnEdit').click(function () {
    //        var btnClick = $(this);
    //        var dataFilter = { id: btnClick[0].dataset.units };
    //        _EditModal.open(dataFilter, function (callback) {
    //            var abc = btnClick.closest('.card-header').children('a').children('p').text(callback.Name);
    //        });
    //    });
    //}
    //function addhtmlunits(value) {
    //    html = `<div class="card card-point pl-2 pr-2 pt-1">
    //                        <div class="card-header  d-flex" >
    //                            <a href="javascript:void(0);" class="p-0 card-title collapsed text-left" data-toggle="collapse" data-target="#collapse`+ value.id + `" aria-expanded="false" >
    //                                <span class="ml-3 mr-3">
    //                                    <span class="collapsed-reveal">
    //                                        <i class="fal fa-minus-circle text-danger"></i>
    //                                    </span>
    //                                    <span class="collapsed-hidden">
    //                                        <i class="fal fa-plus-circle text-success"></i>
    //                                    </span>
    //                                </span>
    //                                <p class='mt-0 mb-0'> `+ value.name + ` </p>
                                   
    //                            </a>
    //                            <div class="ml-auto">
    //                                <button class="btn btnCreate pr-1" style="font-size: 1rem; " data-units=`+ value.id + `>
    //                                    <i class="far fa-plus-square"></i>
    //                                </button>
    //                                <button class="btn btnEdit p-2" style="font-size: 1rem; " data-units=`+ value.id + `>
    //                                    <i class="fas fa-pencil"></i>
    //                                </button>
    //                                <button class="btn btnDelete pl-2" style="font-size: 1rem;" data-units=`+ value.id + `>
    //                                    <i class="fal fa-trash-alt"></i>
    //                                </button>
    //                            </div>
    //                        </div>
    //                        <div id="collapse`+ value.id + `" class="collapse " >
                               
    //                        </div>
    //                    </div>`
    //    $('#accordionExample').append(html);

    //}

    //function loadUnitParrent() {
    //    $('#accordionExample').empty()
    //    _unitsService.getAll({ searchTerm: $('#SearchTerm').val().trim() }).done(function (result) {
    //        if (result.totalCount == 0) {
    //            $('.tbody_hidden').show();
    //        } else {
    //            $('.tbody_hidden').hide();
    //        }
    //        $.each(result.items, function (index, value) {
    //            if (value.parrentId == null) {
    //                addhtmlunits(value)
    //                loadUnitsChil(value.id);
    //            }
    //        });
    //        CreateModal();
    //    });
    //}
    //function loadUnitsChil(ParrentId) {
    //    var collapse = `#collapse` + ParrentId
    //    $(collapse).empty();
    //    _unitsService.getAll({ parrentId: ParrentId }).done(function (result) {
    //        $.each(result.items, function (index, value) {
    //            html = ` <div class="card-body p-0 pl-4 pr-4 pt-1 pb-1 card-point">
    //                                    <div class="card-header d-flex" >
    //                                        <a href="javascript:void(0);" class="p-0 pl-5 card-title collapsed text-left" data-toggle="collapse"  aria-expanded="false" >
    //                                           <p class='mt-0 mb-0'> `+ value.name + ` </p>
    //                                        </a>
    //                                           <div class="ml-auto">
    //                                            <button class="btn p-2 btnEdit" style="font-size: 1rem;" data-units=`+ value.id + `>
    //                                                <i class="fas fa-pencil"></i>
    //                                            </button>
    //                                            <button class="btn pl-2 btnDelete" style="font-size: 1rem;" data-units=`+ value.id + `>
    //                                                <i class="fal fa-trash-alt"></i>
    //                                            </button>
    //                                        </div>
    //                                </div>
    //                            </div>`
    //            $(collapse).append(html)
    //        });
    //        deleteUnits();
    //        EditModal();
    //    });
       
      
    //}
    //loadUnitParrent();
    
    

})(jQuery);