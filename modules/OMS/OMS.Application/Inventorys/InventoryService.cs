using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Inventorys.Dto;
using bbk.netcore.mdl.OMS.Application.InventoryTicketDetails.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Castle.Windsor.Installer;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Inventorys
{
    [AbpAuthorize]
    public class InventoryService : ApplicationService, IInventoryService
    {
        private readonly IRepository<Inventory, long> _inventoryrepository;
        private readonly IRepository<Items, long> _itemsRepository;
        private readonly IRepository<ImportRequestDetail> _imprepository;
        private readonly IRepository<ExportRequestDetail,long> _exprepository;
        private readonly IRepository<Warehouse> _warerepository;
        private readonly IRepository<WarehouseLocationItems, long> _wareLocaItemrepository;
        public InventoryService(IRepository<Inventory, long> inventoryrepository ,
            IRepository<Items, long> itemsRepository,
            IRepository<ImportRequestDetail> imprepository,
            IRepository<Warehouse> warerepository, 
            IRepository<ExportRequestDetail, long> exprepository,
            IRepository<WarehouseLocationItems, long> wareLocaItemrepository)
        {
            _inventoryrepository = inventoryrepository;
            _itemsRepository = itemsRepository;
            _imprepository = imprepository;
             _warerepository = warerepository; 
            _exprepository = exprepository;
            _wareLocaItemrepository = wareLocaItemrepository;
        }

        public async Task<long> Create(InventoryCreate input)
        {
            try
            {
                long output = 0;
                var allItems = _inventoryrepository.GetAll().Select(x => new { x.ItemId, x.WarehouseId }).ToList();
                for (int i = 0; i < allItems.Count; i++)
                {
                    if (allItems[i].ItemId == input.ItemId && allItems[i].WarehouseId == input.WarehouseId)
                    {
                        Inventory invenvory = _inventoryrepository.GetAll().Where(x => x.ItemId == input.ItemId && x.WarehouseId == input.WarehouseId).FirstOrDefault();
                        input.Id = invenvory.Id;
                        input.Quantity = invenvory.Quantity + input.Quantity;
                        ObjectMapper.Map(input, invenvory);
                        await _inventoryrepository.UpdateAsync(invenvory);
                        output = invenvory.Id;
                    }
                }
                if (output == 0)
                {
                    Inventory newinvenvoryId = ObjectMapper.Map<Inventory>(input);
                    var newId = await _inventoryrepository.InsertAndGetIdAsync(newinvenvoryId);
                    output = newId;
                }
                return output;

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Delete(long id)
        {
            try
            {
                await _inventoryrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<InventoryListDto>> GetAll(InventorySearch input)
        {
            try
            {
                var query1 = _inventoryrepository.GetAll().ToList();

                var query = _inventoryrepository.GetAll()
                    .WhereIf(input.ItemId!=0, x => x.ItemId == input.ItemId)
                    .WhereIf(input.WareHouseId!=0, x => x.WarehouseId == input.WareHouseId).ToList();
                var queryItems = _itemsRepository.GetAll().ToList();

                var result = (from iv in query
                              join item in queryItems on iv.ItemId equals item.Id
                              select new InventoryListDto
                              {
                                  Id = iv.Id,
                                  Quantity = iv.Quantity,
                                  ItemId = iv.ItemId,
                                  NameCode = item.ItemCode + "-" + item.Name,
                                  WarehouseId = item.Id,
                                  WarehouseName = item.Name,
                              }).ToList();


                return new PagedResultDto<InventoryListDto>(
                     result.Count(),
                     result
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<InventoryListDto>> GetAllInStock(InventoryInputSearch input)
        {
            try
            {
                var itemsList =  _itemsRepository.GetAll()
                    .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.ItemCode.ToLower().Contains(input.SearchTerm.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(input.Kind), x => x.KindCode.ToLower().Contains(input.Kind.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(input.Category), x => x.CategoryCode.ToLower().Contains(input.Category.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(input.Group), x => x.GroupCode.ToLower().Contains(input.Group.ToLower()))
                    .ToList();
                var inven = _inventoryrepository.GetAll();
                var ware = _warerepository.GetAll();
                var result = (from items in itemsList
                              join invens in inven on items.Id equals invens.ItemId
                              join wr in ware on invens.WarehouseId equals wr.Id
                             
                              select new InventoryListDto
                              {
                                  Id = invens.Id,
                                  Quantity = invens.Quantity,
                                  ItemId = invens.ItemId,
                                  NameCode = items.ItemCode + "-" + items.Name,
                                  WarehouseId = wr.Id,
                                  WarehouseName = wr.Name,
                              }).ToList();


                return new PagedResultDto<InventoryListDto>(
                  result.Distinct().Count(),
                  result.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<InventoryListDto>> GetAllItemStock(InventorySearch input)
        {
            try
            {
                var itemsList = _itemsRepository.GetAll();
                var inven = _inventoryrepository.GetAll().OrderBy(x => x.Id);
                var result = (from iv in inven
                              join it in itemsList on iv.ItemId equals it.Id

                              select new InventoryListDto
                              {
                                  Id = iv.Id,
                                  Quantity = iv.Quantity,
                                  ItemId = iv.ItemId,
                                  NameCode = it.ItemCode + "-" + it.Name,

                              }).ToList();


                return new PagedResultDto<InventoryListDto>(
                  result.Distinct().Count(),
                  result.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<PagedResultDto<InventoryListDto>> GetAllItem(InventorySearch input)
        {
            try
            {
                var itemsList = _itemsRepository.GetAll();
                var inven = _inventoryrepository.GetAll().Where(x => x.WarehouseId == input.WareHouseId);
                var impd = _imprepository.GetAll();
                var exp = _exprepository.GetAll();
                var result = (from iv in inven
                              join it in itemsList on iv.ItemId equals it.Id
                             // join imp in impd on it.Id equals imp.ItemId
                              join ex in exp on it.Id equals ex.ItemId into t
                              from ex in t.DefaultIfEmpty()
                              select new InventoryListDto
                              {
                                  WarehouseId= iv.WarehouseId,
                                  ItemId = it.Id,
                                  NameCode = it.ItemCode + "-" + it.Name,
                                  QuantityIN = iv.Quantity,
                                  NameItem= it.Name,
                                  CodeItem = it.ItemCode,
                                  QuantityOUT= ex.Quantity,
                                  Quantity = iv.Quantity - ex.Quantity,
                              }).ToList();


                return new PagedResultDto<InventoryListDto>(
                  result.Distinct().Count(),
                  result.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<InventoryListDto> GetAsync(EntityDto<long> invenvoryId)
        {
            try
            {
                var invenvory = _inventoryrepository.GetAll().Where(x => x.Id == invenvoryId.Id).ToList();
                InventoryListDto newinvenvory = new InventoryListDto();
                newinvenvory.Quantity = invenvory[0].Quantity;
                newinvenvory.LastModificationTime = invenvory[0].LastModificationTime;
                newinvenvory.CreationTime = invenvory[0].CreationTime;
                newinvenvory.WarehouseId = invenvory[0].WarehouseId;
                newinvenvory.ItemCode = invenvory[0].ItemCode;
                newinvenvory.ItemId = invenvory[0].ItemId;
                newinvenvory.LastModifierUserId = invenvory[0].LastModifierUserId;
                newinvenvory.CreatorUserId = invenvory[0].CreatorUserId;
                newinvenvory.Id = invenvory[0].Id;
                return newinvenvory;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<InventoryListDto> GetInventoryByCode(string itemCode)
        {
            var item = _inventoryrepository.GetAll().Where(x => x.ItemCode.Contains(itemCode.Trim())).ToList();
            InventoryListDto inven = new InventoryListDto();
            for (int i = 0; i < item.Count; i++)
            {
                if (String.Compare(item[i].ItemCode, itemCode, true) == 0)
                {
                    inven.Id = item[i].Id;
                    inven.ItemCode = item[i].ItemCode;
                    inven.WarehouseId = item[i].WarehouseId;
                    inven.Quantity = item[i].Quantity;
                }
            }

            return inven;
        }

        public async Task<long> Update(InventoryListDto input)
        {
            try
            {
                Inventory invenvory = await _inventoryrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                ObjectMapper.Map(input, invenvory);
                await _inventoryrepository.UpdateAsync(invenvory);
                return input.Id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> DeleteByHa(InventorySearch input)
        {
            try
            {
                var idDelete = _inventoryrepository.GetAll().Where(x => x.ItemId == input.ItemId && x.WarehouseId == input.WareHouseId).FirstOrDefault().Id;
                await _inventoryrepository.DeleteAsync(idDelete);
                return idDelete;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateExport(InventoryListDto input)
        {
            try
            {
                Inventory invenvory = await _inventoryrepository.FirstOrDefaultAsync(x => x.WarehouseId == input.WarehouseId && x.ItemId == input.ItemId);
                invenvory.Quantity = invenvory.Quantity - input.Quantity;
                await _inventoryrepository.UpdateAsync(invenvory);
                return input.Id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);}
        }
        public async Task<InventoryListDto> GetViewItemInWare(InventorySearch input)
        {
            try
            {
                var query = _inventoryrepository.GetAll().Where(x => x.ItemCode.Contains(input.SearchTerm)).Select(x => x.WarehouseId).ToList();
                InventoryListDto newinvenvory = new InventoryListDto();

                if (query.IndexOf(input.WareHouseId) != -1)
                {
                    var result = _inventoryrepository.GetAll().Where(x => x.WarehouseId == input.WareHouseId && x.ItemCode.Contains(input.SearchTerm)).FirstOrDefault();

                    newinvenvory.WarehouseId = result.WarehouseId;
                    newinvenvory.CodeItem = result.ItemCode;
                    newinvenvory.Quantity = result.Quantity;
                    newinvenvory.ItemId = result.ItemId;
                }
                else
                {
                    newinvenvory.WarehouseId = input.WareHouseId;
                    newinvenvory.CodeItem = input.SearchTerm;
                    newinvenvory.Quantity = 0;
                }
                
                return newinvenvory;
                
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }

        public async Task<long> UpdateImport(InventoryCreate input)
        {
            try
            {
                var allItems = _inventoryrepository.GetAll().Select(x => new { x.ItemId, x.WarehouseId }).ToList();
                for (int i = 0; i < allItems.Count; i++) 
                {
                    if (allItems[i].ItemId == input.ItemId && allItems[i].WarehouseId == input.WarehouseId)
                    {
                        Inventory invenvory = _inventoryrepository.GetAll().Where(x => x.ItemId == input.ItemId && x.WarehouseId == input.WarehouseId).FirstOrDefault();
                        input.Id = invenvory.Id;
                        input.Quantity += input.Quantity;
                        ObjectMapper.Map(input, invenvory);
                        await _inventoryrepository.UpdateAsync(invenvory);
                    }
                }

                return allItems.Count();
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
