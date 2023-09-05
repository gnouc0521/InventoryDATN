using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.ExportRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseItems;
using bbk.netcore.mdl.OMS.Application.WarehouseLocationItem;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ExportRequestDetails
{
    public class ExportRequestDetails : ApplicationService, IExportRequestDetails
    {
        private readonly IRepository<ExportRequestDetail, long> _exportRequestDetailRepository;
        private readonly IRepository<WarehouseLocationItems, long> _warehouseLocationItems;
        private readonly IRepository<Items, long> _itemsRepository;
        private readonly IRepository<Inventory, long> _inventoryRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IWarehouseItemAppService _warehouseItemAppService;
        private readonly IWarehouseLocationItemService _IwarehouseLocationItemService;
        public ExportRequestDetails(
            IRepository<ExportRequestDetail, long> exportRequestDetailRepository,
            IRepository<WarehouseLocationItems, long> warehouseLocationItems,
            IRepository<Items, long> itemsRepository,
            IRepository<Inventory, long> inventoryRepository,
            IRepository<Unit> unitRepository,
            IWarehouseItemAppService warehouseItemAppService,
            IWarehouseLocationItemService IwarehouseLocationItemService
           )
        {
            _exportRequestDetailRepository = exportRequestDetailRepository;
            _itemsRepository = itemsRepository;
            _inventoryRepository = inventoryRepository;
            _unitRepository = unitRepository;
            _warehouseItemAppService = warehouseItemAppService; 
            _warehouseLocationItems = warehouseLocationItems;
            _IwarehouseLocationItemService = IwarehouseLocationItemService;
        }
        
        public async Task<long> Create(ExportRequestDetailCreate input)
        {
            try
            {
                ExportRequestDetail newItemId = ObjectMapper.Map<ExportRequestDetail>(input);
                var newId = await _exportRequestDetailRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch ( Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
           
        }
        
        public async Task<long> Delete(long id)
        {
            try
            {
                await _exportRequestDetailRepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<PagedResultDto<ExportRequestDetailListDto>> GetAll(ExportRequestDetailsSearch input)
        {
            try
            {
                var query = _exportRequestDetailRepository.GetAll().Where(x=>x.ExportRequestId == input.ExportRequestId).ToList();
                var query2 = _itemsRepository.GetAll().ToList();
                var queryinventory = _inventoryRepository.GetAll().Where(x=>x.WarehouseId == input.WarehouseId).ToList();
                var queryunit = _unitRepository.GetAll().ToList();
                var output = (from e in query
                              join items in query2 on e.ItemId equals items.Id
                              join inventory in queryinventory on items.Id equals inventory.ItemId
                              join unit in queryunit on e.UnitId equals unit.Id into gj
                              from subpet in gj.DefaultIfEmpty()
                              select new ExportRequestDetailListDto
                              {
                                  ExportRequestId = e.ExportRequestId,
                                  QuantityExport = e.Quantity,
                                  ExportPrice = e.ExportPrice,
                                  ExpireDate = e.ExpireDate,
                                  Remark = e.Remark,
                                  Id = e.Id,
                                  ItemId = e.ItemId,
                                  BlockId = e.BlockId,
                                  ShelfId = e.ShelfId,
                                  FloorId = e.FloorId,
                                  UnitId = e.UnitId,
                                  ItemsCode = items.ItemCode,
                                  ItemsName = items.Name,
                                  UnitName = subpet.Name ?? string.Empty,
                                  QuantityTotal = inventory.Quantity,
                                  Thanhtien = (decimal)(e.ExportPrice * e.Quantity)


                              }).ToList();
                foreach (var item in output)
                {
                    if (item.BlockId != 0 && item.BlockId!=null)
                    {
                        var queryWarehouseLocation = _warehouseItemAppService.GetById(new EntityDto((int)(item.BlockId)));
                        item.BlockName = queryWarehouseLocation.Result.Name;
                    }
                    if (item.ShelfId != null && item.ShelfId != 0)
                    {
                        var queryWarehouseLocation1 = _warehouseItemAppService.GetById(new EntityDto((int)(item.ShelfId)));
                        item.ShelfName = queryWarehouseLocation1.Result.Name;

                    }
                    if (item.FloorId != null && item.FloorId != 0 )
                    {
                        var queryWarehouseLocation2 = _warehouseItemAppService.GetById(new EntityDto((int)(item.FloorId)));
                        item.FloorName = queryWarehouseLocation2.Result.Name;

                    }

                }
                var itemscount = output.Count();
                return new PagedResultDto<ExportRequestDetailListDto>(
                     itemscount,
                     output
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            };
        }

        public  async Task<PagedResultDto<ExportRequestDetailListDto>> GetAllExport(ExportRequestDetailsSearch input)
        {
            try
            {
                var query = _exportRequestDetailRepository.GetAll().Where(x => x.ExportRequestId == input.ExportRequestId).ToList();
                var query2 = _itemsRepository.GetAll().ToList();
                var queryinventory = _inventoryRepository.GetAll().Where(x => x.WarehouseId == input.WarehouseId).ToList();
                var queryunit = _unitRepository.GetAll().ToList();
                var location = _warehouseLocationItems.GetAll().Where(x=>x.WarehouseId==input.WarehouseId).ToList();
                

                var output = (from e in query
                              join items in query2 on e.ItemId equals items.Id
                              join locations in location on e.ItemId equals locations.ItemId 
                              join inventory in queryinventory on items.Id equals inventory.ItemId
                              join unit in queryunit on e.UnitId equals unit.Id into gj
                              from subpet in gj.DefaultIfEmpty()
                              //from LocationNull in g.DefaultIfEmpty()
                              select new ExportRequestDetailListDto
                              {
                                  ExportRequestId = e.ExportRequestId,
                                  QuantityExport = e.Quantity,
                                  ExportPrice = e.ExportPrice,
                                  ExpireDate = e.ExpireDate,
                                  Remark = e.Remark,
                                  Id = e.Id,
                                  LocationId = locations.Id,
                                  ItemId = e.ItemId,
                                  BlockId = (locations.Block != null) ?  Int64.Parse(locations.Block) : 0 ,
                                  ShelfId = (locations.Shelf != null) ? Int64.Parse(locations.Shelf) : 0,
                                  FloorId = (locations.Floor != null) ? Int64.Parse(locations.Floor) : 0,
                                  UnitId = e.UnitId,
                                  ItemsCode = items.ItemCode,
                                  ItemsName = items.Name,
                                  UnitName = subpet.Name ?? string.Empty,
                                  QuantityTotal = inventory.Quantity,
                                  QuantityLocation = locations.Quantity,
                                  Thanhtien = (decimal)(e.ExportPrice * e.Quantity)
                              }).ToList();

                foreach (var item in output)
                {
                    if (item.BlockId != 0 && item.BlockId != null)
                    {
                        var queryWarehouseLocation = _warehouseItemAppService.GetById(new EntityDto((int)(item.BlockId)));
                        item.BlockName = queryWarehouseLocation.Result.Name;
                    }
                    if (item.ShelfId != null && item.ShelfId != 0)
                    {
                        var queryWarehouseLocation1 = _warehouseItemAppService.GetById(new EntityDto((int)(item.ShelfId)));
                        item.ShelfName = queryWarehouseLocation1.Result.Name;
                    }
                    if (item.FloorId != null && item.FloorId != 0)
                    {
                        var queryWarehouseLocation2 = _warehouseItemAppService.GetById(new EntityDto((int)(item.FloorId)));
                        item.FloorName = queryWarehouseLocation2.Result.Name;
                    }


                }
                var itemscount = output.Count();
                return new PagedResultDto<ExportRequestDetailListDto>(
                     itemscount,
                     output
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            };
        }

        public async Task<ExportRequestDetailListDto> GetAsync(EntityDto<long> itemId)
        {
            try
            {
                var item = _exportRequestDetailRepository.Get(itemId.Id);
                ExportRequestDetailListDto newItem = ObjectMapper.Map<ExportRequestDetailListDto>(item);
                return newItem;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ExportRequestDetailListDto> GetDetail(EntityDto<long> itemId)
        {
            try
            {
                var item = _exportRequestDetailRepository.FirstOrDefault(x=>x.ExportRequestId == itemId.Id);
                ExportRequestDetailListDto newItem = ObjectMapper.Map<ExportRequestDetailListDto>(item);
                return newItem;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Update(ExportRequestDetailListDto input)
        {
            ExportRequestDetail items = await _exportRequestDetailRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, items);
            input.CreationTime = items.CreationTime;
            input.CreatorUserId = items.CreatorUserId;
            input.UnitName = items.UnitName;
            await _exportRequestDetailRepository.UpdateAsync(items);
            return input.Id;
        }
    }
}
