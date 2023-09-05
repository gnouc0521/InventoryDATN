using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestDetails
{
    [AbpAuthorize]
    public class ImportRequestDetailAppService : ApplicationService, IImportRequestDetailAppService
    {
        private readonly IRepository<ImportRequestDetail> _importRequestDetail;
        private readonly IRepository<Items, long> _items;
        private readonly IRepository<ImportRequest> _importRequest;
        private readonly IRepository<Inventory, long> _inventory;

        public ImportRequestDetailAppService(IRepository<ImportRequestDetail> importRequestDetail, IRepository<Items, long> items, IRepository<ImportRequest> importRequest, IRepository<Inventory, long> inventory)
        {
            _importRequestDetail = importRequestDetail;
            _items = items;
            _importRequest = importRequest;
            _inventory = inventory;
        }


        public async Task<PagedResultDto<ImportRequestDetailListDto>> GetAll(GetImportRequestDetailInput input)
        {
            try
            {
                var itemsList = _items.GetAll();
                var query = _importRequestDetail
                      .GetAll()
                      .Where(x => x.ImportRequestId == input.importRequestId);
                     
                      
                var impquery = _importRequest.GetAll();
                var result = (from i in query
                              join im in impquery on i.ImportRequestId equals im.Id
                              join it in itemsList on i.ItemId equals it.Id
                              select new ImportRequestDetailListDto
                              {
                                  Id = i.Id,
                                  ImportRequestId = i.ImportRequestId,
                                  CodeItem = it.ItemCode,
                                  NameItem = it.ItemCode+"-"+it.Name,
                                  UnitId = i.UnitId,
                                  UnitName = i.UnitName,
                                  ImportPrice = i.ImportPrice,
                                  Quantity = i.Quantity,
                                  ItemId = i.ItemId,
                                  ExpireDate = i.ExpireDate,
                                  ShipperName= im.ShipperName,
                                  ShipperPhone= im.ShipperPhone,
                                  QuantityHT= i.QuantityHT,
                                 // MFG = i.MFG,
                                  Thanhtien = i.ImportPrice * i.Quantity
                              }).ToList();


                return new PagedResultDto<ImportRequestDetailListDto>(
                  result.Distinct().Count(),
                  result.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }



        //public async Task<PagedResultDto<ImportRequestDetailListDto>> GetAllItem(GetImportRequestDetailInput input)
        //{
        //    try
        //    {
        //        var item = _items.GetAll();
        //        var iven = _inventory.GetAll();
        //        var impquery = _importRequest.GetAll().Where(x => x.WarehouseDestinationId == input.WarehouseId);
        //        var query = _importRequestDetail
        //            .GetAll()
        //            .ToList()
        //            .OrderBy(x => x.Id);

        //        var result = (from wh in iven
        //                      join it in item on wh.ItemId equals it.Id
        //                      where wh.WarehouseId == input.WarehouseId
        //                      select new ImportRequestDetailListDto
        //                      {
        //                          ItemId = wh.ItemId,
        //                          Quantity = wh.Quantity,
        //                          CodeItem = it.ItemCode,
        //                          NameItem = it.Name,
        //                          //ImportPrice = it.e,


        //                      }).ToList();


        //        return new PagedResultDto<ImportRequestDetailListDto>(
        //          result.Distinct().Count(),
        //          result.Distinct().ToList()
        //          );
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}


        public async Task<long> Create(ImportRequestDetailCreateDto input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input.ItemCode) && (input.ItemId == null|| input.ItemId==0))
                {
                   var items =  _items.GetAll().Where(x => x.ItemCode.Equals(input.ItemCode)).FirstOrDefault();
                    if (items.Id != null)
                    {
                        input.ItemId = items.Id;
                    }

                }

                ImportRequestDetail newItemId = ObjectMapper.Map<ImportRequestDetail>(input);
                var newId = await _importRequestDetail.InsertAndGetIdAsync(newItemId);
                //var imp = _importRequest.GetAll().Where(x=>x.Id == newItemId.ImportRequestId).FirstOrDefault();
                //var items = _items.GetAll().Where(x => x.WarehouseId == imp.WarehouseDestinationId && x.Id == newItemId.ItemId);
                //foreach (var itemq in items)
                //{
                //    itemq.EntryPrice = newItemId.ImportPrice;
                //    await _items.UpdateAsync(itemq);
                //}

                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }



        public async Task<long> Update(ImportRequestDetailListDto input)
        {
            ImportRequestDetail IMP = await _importRequestDetail.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.ExpireDate= IMP.ExpireDate ;
            input.ImportPrice = IMP.ImportPrice;
            ObjectMapper.Map(input, IMP);

            //var imp = _importRequest.GetAll().Where(x => x.Id == IMP.ImportRequestId).FirstOrDefault();
            //var inventory = _inventory.GetAll().Where(x => x.WarehouseId == imp.WarehouseDestinationId && x.ItemId == IMP.ItemId);
            //var items = _items.GetAll().Where(x => x.WarehouseId == imp.WarehouseDestinationId && x.Id == IMP.ItemId);
            //foreach (var item in inventory)
            //{
            //    item.Quantity = IMP.Quantity + item.Quantity;
            //    await _inventory.UpdateAsync(item);
            //}
            //foreach (var itemq in items)
            //{
            //    itemq.EntryPrice = IMP.ImportPrice;
            //    await _items.UpdateAsync(itemq);
            //}

            await _importRequestDetail.UpdateAsync(IMP);
            return input.Id;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _importRequestDetail.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ImportRequestDetailListDto> GetAsync(EntityDto itemId)
        {
            var item = _importRequestDetail.Get(itemId.Id);
            ImportRequestDetailListDto newItem = ObjectMapper.Map<ImportRequestDetailListDto>(item);
            return newItem;
        }

    }
}
