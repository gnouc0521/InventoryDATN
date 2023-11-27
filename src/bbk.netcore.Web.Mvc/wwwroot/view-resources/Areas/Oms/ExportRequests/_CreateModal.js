(function ($) {

  app.modals.ExportRequestsCreateModal = function () {


    var _itemsServiceService = abp.services.app.itemsService;
    var _exportRequests = abp.services.app.exportRequests;
    var _exportRequestsdetail = abp.services.app.exportRequestDetails;
    var _inventory = abp.services.app.inventoryService;
    var _warehouseLocationItemService = abp.services.app.warehouseLocationItemService;
    var _unitService = abp.services.app.unitService;
    var _modalManager;
    var _frmDelivery = null;
    html = `<tr>
                                <th></th>
                                <th><select  class="form-control selectExport ItemId" style="width:100%"   required>
                                  <option value="" disabled selected=""> Chọn đơn vị </option>
                                </select></th>
                                <th><input type="number" id="row-1-age"  class="form-control ExportPrice"  required></th>
                                <th><input type="number" id="row-1-age"  class="form-control QuantityTotal"  disabled></th>
                                <th><input type="number" id="row-1-age" class="form-control Quantity"  required></th>
                                <th><select  class="form-control UnitId" style="width:100%"   required >
                                <option value="" disabled selected=""> Chọn đơn vị </option>
                                </select></th>
                                <th><select size="1" id="row-1-office" class="form-control ShelfId" name="row-1-office" fdprocessedid="ee8fyd" >
                                <option value="" disabled selected=""> Chọn Khối </option>
                                </select></th>
                                <th><select size="1" id="row-1-office" class="form-control FloorId" name="row-1-office" fdprocessedid="ee8fyd" >
                                <option value="" disabled selected=""> Chọn Tầng </option>
                                </select></th>
                                <th><select size="1" id="row-1-office" class="form-control BlockId" name="row-1-office" fdprocessedid="ee8fyd" >
                                 <option value="" disabled selected=""> Chọn Khay </option>
                                </select></th>
                                <th><p class=" LocationId mb-0 text-wrap" name="row-1-office" ></p></th>
                                <th class="text-center"><a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
                            </tr>`;

    function unit() {
      _unitService.getAll({}).done(function (results) {
        $.each(results.items, function (index, value) {
          if (value.parrentId == null) {
            optgroup = `<optgroup data-parrent=` + value.id + ` label="` + value.name + `">` + `</optgroup>`
            $('.UnitId option:last').after(optgroup)
          }
        })
        var listopgroup = $('.UnitId optgroup')
        $.each(listopgroup, function (index, value) {
          let dataparrent = value.dataset.parrent;
          $.each(results.items, function (index, valueoption) {
            if (valueoption.parrentId == dataparrent) {
              option = `<option value="` + valueoption.id + `">` + valueoption.name + `</option>`
              $(value).append(option)
            }
          })
        })
      })
    }

    this.init = function (modalManager) {
      function delete_row() {
        $('.delete_row').click(function () {
          $(this).parents('tr').remove();
        })
      }
      var getFilter = function () {
        let dataFilter = {};
        dataFilter.wareHouseId = $('#WarehouseDestinationId').find(':selected').val();
        dataFilter.itemId = $(this).find(':selected').val();
        return dataFilter;
      }
      $('#WarehouseDestinationId').on('change', function () {
        $('#ExportRequestModal tbody tr').remove();
        $('#addRow').attr('disabled', false)
      })
      function tbodytr(length) {
        var stt = length + 1
        return html = `<tr>
                                <th>`+ stt + `</th>
                                <th><select  class="form-control selectExport ItemId" style="width:100%"   required>
                                  <option value="" disabled selected=""> Chọn đơn vị </option>
                                </select></th>
                                <th><input type="number" id="row-1-age"  class="form-control ExportPrice"  required></th>
                                <th><input type="number" id="row-1-age"  class="form-control QuantityTotal"  disabled></th>
                                <th><input type="number" id="row-1-age" class="form-control Quantity"  required></th>
                                <th><select  class="form-control UnitId" style="width:100%"   required >
                                <option value="" disabled selected=""> Chọn đơn vị </option>
                                </select></th>
                                <th class="text-center"><a class="delete_row" href='javascript:void(0);'><i class="fal fa-trash-alt  align-bottom "></i></a> </th>
                            </tr>`;
      }

      if ($('#WarehouseDestinationId :selected').val() != undefined) {
        $('#addRow').click(function () {
          _inventory.getAll(getFilter()).done(function (result) {
            var dataselectItems = $.map(result.items, function (obj) {
              obj.id = obj.itemId;
              obj.text = obj.nameCode; // replace name with the property used for the text
              return obj;

            });
            var length = $('#ExportRequestModal tbody tr').length
            $('#ExportRequestModal tbody ').append(tbodytr(length));
            unit();
            $("#ExportRequestModal tbody .selectExport").change(function () {
              var selVal = [];
              $("#ExportRequestModal tbody .selectExport").each(function () {
                selVal.push(this.value);
              });

              var abc = $(this).parents('th').find('select')
              $(abc).find("option").removeAttr("disabled").filter(function () {

                var a = $(this).parent("select").val();
                return (($.inArray(this.value, selVal) > -1) && (this.value != a))
              }).attr("disabled", "disabled");

            });


            $(".selectExport").eq(0).trigger('change');


            // select Items
            $('.selectExport').select2({
              width: "100%",
              dropdownParent: $('#ExportRequestsCreateModal'),
              placeholder: 'Chọn hàng hóa',
              data: dataselectItems,
            }).on('select2:select', function (e) {

              var data = e.params.data
              var InQuaTotal = $(this).parents('tr').find('.QuantityTotal').val(data.quantity)
              $(this).parents('tr').find('.Quantity').attr('max', data.quantity);
              var selectShelfId = $(this).parents('tr').find('.ShelfId');

              var selectFloorId = $(this).parents('tr').find('.FloorId')
              var selectBlockId = $(this).parents('tr').find('.BlockId')

              var getFilter = function () {
                let dataFilter = {};
                dataFilter.wareHouseId = $('#WarehouseDestinationId').find(':selected').val();
                dataFilter.warehouseId = $('#WarehouseDestinationId').find(':selected').val();
                dataFilter.itemId = data.itemId;
                dataFilter.block;
                return dataFilter;
              }
              _warehouseLocationItemService.getLocationItems(getFilter()).done(function (result) {
                $.each(result.items, function (index, value) {
                  $(selectShelfId).append("<option value=" + value.shelf + ">" + value.shelfName + "</option>");
                  $(selectFloorId).append("<option value=" + value.floor + ">" + value.floorName + "</option>");
                  if (value.block != null) {
                    $(selectBlockId).append("<option value=" + value.block + ">" + value.blockName + "</option>");
                  }
                })
              })


            }).trigger('change');

            $('input[type="number"]').on('keyup', function () { if ($(this).val() > $(this).attr('max') * 1) { $(this).val($(this).attr('max')); } });
            delete_row();
          })
        })
        delete_row();
      }
      document.getElementById("phone").addEventListener("input", function () {
        var valueChange = funcChanePhoneNumber();
        _frmDelivery.find('input[name=Phone]').val(valueChange);
      });
      function funcChanePhoneNumber() {
        var valueChange = null;
        var valueInputPhone = _frmDelivery.find('input[name=Phone]').val();
        if (valueInputPhone.substring(0, 1) == 0) {
          valueChange = _frmDelivery.find('input[name=Phone]').val().replace('0', '');
        } else {
          valueChange = _frmDelivery.find('input[name=Phone]').val().replace(/[^0-9]/g, '');
        }
        return valueChange;
      }

      $('.date-picker').datepicker({
        rtl: false,
        format: 'dd/mm/yyyy',
        orientation: "left",
        autoclose: true,
        language: abp.localization.currentLanguage.name,

      });

      _modalManager = modalManager;
      _frmDelivery = _modalManager.getModal().find('form[name=frmCreate]');

    }

    //sự kiện khi đóng modal
    $(".close-modal").on("click", function () {
      abp.libs.sweetAlert.config = {
        confirm: {
          icon: 'warning',
          buttons: ['Không', 'Có']
        }
      };
      abp.message.confirm(
        app.localize('Đóng'),
        app.localize('Bạn có chắc không'),
        function (isConfirmed) {
          if (isConfirmed) {
            _modalManager.close();
            return true;

          }
        }
      );
    })
    this.save = function () {
      // validation: jquery validate
      _frmDelivery.addClass('was-validated');
      if (_frmDelivery[0].checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        return;
      }
      var data = _frmDelivery.serializeFormToObject();
      _modalManager.setBusy(true);
      data.WarehouseDestinationId = $('#WarehouseDestinationId').find(':selected').val()
      _exportRequests.create(data)
        .done(function (result) {
          _modalManager.close();
          abp.notify.info('Thêm mới phiếu xuất thành công!');
          abp.event.trigger('app.reloadDocTable');
          $('#ExportRequestModal tbody tr').each(function (index, value) {
            // data.WarehouseSourceId = $('#WarehouseDestinationId').find(':selected').val()
            data.ExportRequestId = result;
            data.ItemId = $(value).children('th').find('.ItemId option:selected').val();
            data.Quantity = $(value).children('th').find('.Quantity ').val();
            data.ExportPrice = $(value).children('th').find('.ExportPrice ').val();
            if ($(value).children('th').find('.BlockId option:selected').val() != "") {
              data.BlockId = $(value).children('th').find('.BlockId option:selected').val();

            }
            data.FloorId = $(value).children('th').find('.FloorId option:selected').val();
            data.ShelfId = $(value).children('th').find('.ShelfId option:selected').val();
            data.UnitId = $(value).children('th').find('.UnitId option:selected').val()
            _exportRequestsdetail.create(data)

          })
        }).always(function () {
          _modalManager.setBusy(false);
        });

    }
  };
})(jQuery);


