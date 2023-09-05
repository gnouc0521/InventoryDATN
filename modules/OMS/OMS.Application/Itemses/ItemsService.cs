using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace bbk.netcore.mdl.OMS.Application.Itemses
{
    public class ItemsService : ApplicationService, IItemsService
    {
        private readonly IRepository<Items, long> _Itemsrepository;
        private readonly IRepository<Inventory, long> _inventoryrepository;
        private readonly IRepository<Producer> _producerrepository;
        public ItemsService(IRepository<Items, long> itemsrepository,
           IRepository<Inventory, long> inventoryrepository,
           IRepository<Producer> producerrepository
           )
        {
            _Itemsrepository = itemsrepository;
            _inventoryrepository = inventoryrepository;
            _producerrepository = producerrepository;
        }
        public async Task<long> Create(ItemsListDto input)
        {
            try
            {
                string code()
                {
                    string s = input.CategoryCode.ToString() + input.GroupCode.ToString() + input.KindCode.ToString();
                    return s;
                }
                string sinhma(string ma)
                {
                    string s = ma.Substring(6, ma.Length - 6);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "00000" + Convert.ToString(i);
                    else if (i >= 10 && i < 100) return "0000" + Convert.ToString(i);
                    else if (i >= 100 && i < 1000) return "000" + Convert.ToString(i);
                    else if (i >= 1000 && i < 10000) return "00" + Convert.ToString(i);
                    else if (i >= 10000 && i < 100000) return "0" + Convert.ToString(i);
                    else return Convert.ToString(i);

                }
                string ma;
                var query = await _Itemsrepository.GetAll().ToListAsync();
                var count = query.Count;
                if (count == 0)

                {
                    ma = "000000000000";
                }
                else
                {
                    ma = _Itemsrepository.GetAll().OrderByDescending(x => x.Id).Select(x => x.ItemCode).ToList().First();
                }

                input.ItemCode = code() + sinhma(ma.ToString());
                Items newItemId = ObjectMapper.Map<Items>(input);
                var newId = await _Itemsrepository.InsertAndGetIdAsync(newItemId);
                return newId;
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
                await _Itemsrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        // get all items in inventory
        public async Task<PagedResultDto<ItemsListDto>> GetAll(ItemsSearch input)
        {
            try
            {
                var queryout = _Itemsrepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm),
                                                                x => x.Name.ToLower().Contains(input.SearchTerm.ToLower().Trim())
                                                                || input.SearchTerm.ToLower().Trim().Contains(x.Name.ToLower())
                                                                || x.ItemCode.ToLower().Contains(input.SearchTerm.ToLower()))
                                                .ToList();
                var inventory = _inventoryrepository.GetAll().Where(x => x.WarehouseId == input.WareHouseId);
                var itemslist = ObjectMapper.Map<List<ItemsListDto>>(queryout);
                var itemscount = inventory.Count();
                var output = (from q in itemslist
                              join i in inventory on q.Id equals i.ItemId
                              select new ItemsListDto
                              {

                                  Id = q.Id,
                                  WarehouseId = i.WarehouseId,
                                  ItemCode = q.ItemCode,
                                  Name = q.Name,
                                  LastModificationTime = q.LastModificationTime,
                                  CreationTime = q.CreationTime,
                                  DeleterUserId = q.DeleterUserId,
                                  SupplierCode = q.SupplierCode,
                                  SymbolCode = q.SymbolCode,
                                  KindCode = q.KindCode,
                                  GroupCode = q.GroupCode,
                                  CategoryCode = q.CategoryCode,
                                  EntryPrice = q.EntryPrice,
                                  Stauts = q.Stauts,
                                  Quantity = i.Quantity,
                                  Description = q.Description,
                                  InventoryId = i.Id,
                              }).ToList();


                return new PagedResultDto<ItemsListDto>(
                     itemscount,
                     output
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        // get all items
        public async Task<PagedResultDto<ItemsListDto>> GetAllItems(ItemsSearch input)
        {
            try
            {
                var queryout = _Itemsrepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm),
                                                                x => x.Name.ToLower().Contains(input.SearchTerm.ToLower().Trim())
                                                                || input.SearchTerm.ToLower().Trim().Contains(x.Name.ToLower())
                                                                || x.ItemCode.ToLower().Contains(input.SearchTerm.ToLower())).ToList();
                var querysupplier = _producerrepository.GetAll();
                var itemslist = ObjectMapper.Map<List<ItemsListDto>>(queryout);
                var output = (from q in itemslist
                              join supp in querysupplier on q.ProducerCode equals supp.Code into gj
                              from subpet in gj.DefaultIfEmpty()
                              select new ItemsListDto
                              {

                                  Id = q.Id,
                                  ItemCode = q.ItemCode,
                                  Name = q.Name,
                                  LastModificationTime = q.LastModificationTime,
                                  CreationTime = q.CreationTime,
                                  DeleterUserId = q.DeleterUserId,
                                  SupplierCode = q.SupplierCode,
                                  SymbolCode = q.SymbolCode,
                                  KindCode = q.KindCode,
                                  GroupCode = q.GroupCode,
                                  CategoryCode = q.CategoryCode,
                                  EntryPrice = q.EntryPrice,
                                  Stauts = q.Stauts,
                                  Description = q.Description,
                                  SupplierName = subpet?.Name ?? string.Empty

                              }).ToList();


                return new PagedResultDto<ItemsListDto>(
                     output.Count(),
                     output
                     );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ItemsListDto> GetAsync(EntityDto<long> itemId)
        {
            var item = _Itemsrepository.Get(itemId.Id);
            ItemsListDto newItem = ObjectMapper.Map<ItemsListDto>(item);
            return newItem;
        }

        public async Task<long> Update(ItemsListDto input)
        {
            string code()
            {
                string s = input.CategoryCode.ToString() + input.GroupCode.ToString() + input.KindCode.ToString();
                return s;
            }

            Items items = await _Itemsrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.ItemCode = code() + items.ItemCode.Substring(6, items.ItemCode.Length - 6);
            input.CreatorUserId = items.CreatorUserId;
            input.CreationTime = items.CreationTime;
            ObjectMapper.Map(input, items);
            await _Itemsrepository.UpdateAsync(items);
            return input.Id;
        }

        public async Task<List<ItemsListDto>> GetItemList()
        {
            try
            {
                var query = _Itemsrepository.GetAll().OrderBy(x => x.Id).ToList();
                var inventory = _inventoryrepository.GetAll();
                var output = (from q in query
                              join i in inventory on q.Id equals i.ItemId
                              select new ItemsListDto
                              {
                                  Id = q.Id,
                                  WarehouseId = i.WarehouseId,
                                  ItemCode = q.ItemCode,
                                  Name = q.Name,
                                  LastModificationTime = q.LastModificationTime,
                                  CreationTime = q.CreationTime,
                                  DeleterUserId = q.DeleterUserId,
                                  SymbolCode = q.SymbolCode,
                                  KindCode = q.KindCode,
                                  GroupCode = q.GroupCode,
                                  CategoryCode = q.CategoryCode,
                                  //EntryPrice = q.EntryPrice,
                                  Quantity = i.Quantity,
                                  Description = q.Description,
                              }).ToList();

                return output;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<ItemsListDto>> GetItemImportList()
        {
            try
            {
                var query = _Itemsrepository.GetAll().OrderBy(x => x.Id).ToList();
                var output = (from q in query
                              select new ItemsListDto
                              {
                                  Id = q.Id,
                                  ItemCode = q.ItemCode,
                                  Name = q.Name,
                                  LastModificationTime = q.LastModificationTime,
                                  CreationTime = q.CreationTime,
                                  DeleterUserId = q.DeleterUserId,
                                  SymbolCode = q.SymbolCode,
                                  KindCode = q.KindCode,
                                  GroupCode = q.GroupCode,
                                  CategoryCode = q.CategoryCode,
                                  //EntryPrice = q.EntryPrice,
                                  Description = q.Description,
                              }).ToList();

                return output;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ItemsListDto> GetItemByCode(string itemCode)
        {
            var item = _Itemsrepository.GetAll().Where(x => x.ItemCode.Contains(itemCode.Trim())).ToList();
            var newItem = ObjectMapper.Map<ItemsListDto>(item[0]);
            return newItem;
        }
    }
}
