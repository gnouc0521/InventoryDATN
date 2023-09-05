using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseItems;
using bbk.netcore.mdl.OMS.Application.WarehouseLocationItem.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Castle.Windsor.Installer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseLocationItem
{
    [AbpAuthorize]
    public class WarehouseLocationItemService : netcoreAppServiceBase, IWarehouseLocationItemService
    {
        private readonly IRepository<Items, long> _itemrepository;
        private readonly IRepository<Warehouse> _warehouserepository;
        private readonly IRepository<WarehouseItem> _wareItemhouserepository;
        private readonly IRepository<Inventory, long> _Inventoryrepository;
        private readonly IRepository<WarehouseLocationItems, long> _warehouseLocationItemrepository;
        private readonly IRepository<ImportRequest> _importRequestrepository;
        private readonly IRepository<ImportRequestDetail> _importRequestDetailrepository;
        private readonly IWarehouseItemAppService _warehouseItemAppService;

        public WarehouseLocationItemService(IRepository<Items, long> itemrepository,
                                            IRepository<Warehouse> warehouserepository,
                                            IRepository<WarehouseLocationItems, long> warehouseLocationItemrepository,
                                            IRepository<Inventory, long> inventoryrepository,
                                            IRepository<ImportRequest> importRequestrepository,
                                            IRepository<ImportRequestDetail> importRequestDetailrepository,
                                            IWarehouseItemAppService warehouseItemAppService,
                                            IRepository<WarehouseItem> wareItemhouserepository)
        {
            _itemrepository = itemrepository;
            _warehouserepository = warehouserepository;
            _warehouseLocationItemrepository = warehouseLocationItemrepository;
            _Inventoryrepository = inventoryrepository;
            _importRequestrepository = importRequestrepository;
            _importRequestDetailrepository = importRequestDetailrepository;
            _warehouseItemAppService = warehouseItemAppService;
            _wareItemhouserepository = wareItemhouserepository;
        }

        public async Task<long> Create(WarehouseLocationItemsCreateDto input)
        {
            try
            {
                WarehouseLocationItems newItemId = ObjectMapper.Map<WarehouseLocationItems>(input);
                var newId = await _warehouseLocationItemrepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ItemsListDto>> GetAll(WarehouseLocationItemsInput input)
        {
            try
            {
                var query = _itemrepository.GetAll()
                            .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Name.ToLower().Contains(input.SearchTerm.ToLower().Trim()) || u.ItemCode.Contains(input.SearchTerm))

                            //.WhereIf(!string.IsNullOrEmpty(input.SupplierId), u => u.SupplierCode.Contains(input.SupplierId))
                            .WhereIf(!string.IsNullOrEmpty(input.ProducerId), u => u.ProducerCode.Contains(input.ProducerId)).ToList();
                //.WhereIf(input.Year != 0, u => u.MFG == input.Year)
                //.WhereIf(!string.IsNullOrEmpty(input.fromDate), u => u.EXP >= DateTime.Parse(input.fromDate))
                //.WhereIf(!string.IsNullOrEmpty(input.toDate), u => u.EXP <= DateTime.Parse(input.toDate)).ToList();

                var query2 = _Inventoryrepository.GetAll().WhereIf(input.WarehouseId != 0, u => u.WarehouseId == input.WarehouseId).ToList();


                var result = (from item in query
                              join inven in query2 on item.ItemCode equals inven.ItemCode
                              select new ItemsListDto
                              {
                                  Id = item.Id,
                                  ItemCode = item.ItemCode,
                                  WarehouseId = inven.WarehouseId,
                                  Quantity = inven.Quantity,
                                  InventoryId = inven.Id,
                                  Name = item.Name,
                                  IsDeleted = inven.IsDeleted,
                              }).ToList();


                return new PagedResultDto<ItemsListDto>(
                        result.Count(),
                        result
                    );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }

        }
        // items được sắp xếp
        public async Task<PagedResultDto<WarehouseLocationItemsDto>> GetAllItem(WarehouseLocationItemsInput input)
        {
            try
            {
                var query = _importRequestDetailrepository.GetAll().ToList();
                var queryimport = _importRequestrepository.GetAll().ToList();
                var querywarehouse = _warehouserepository.GetAll().WhereIf(input.WarehouseId != 0, x => x.Id == input.WarehouseId).ToList();
                var queryitems = _itemrepository.GetAll().ToList();
                var querywarehouselocation = _warehouseLocationItemrepository.GetAll().ToList();
                var result = (from importdetail in query
                              join item in queryitems on importdetail.ItemId equals item.Id
                              join import in queryimport on importdetail.ImportRequestId equals import.Id
                              join warehouse in querywarehouse on import.WarehouseDestinationId equals warehouse.Id
                              join warehouselocation in querywarehouselocation on importdetail.Id equals warehouselocation.ImportRequestDetailId
                              select new WarehouseLocationItemsDto
                              {
                                  Id = item.Id,
                                  ItemCode = item.ItemCode,
                                  ImportRequestCode = import.Code,
                                  WarehouseName = warehouse.Name,
                                  ExpireDate = import.RequestDate,
                                  ImportDate = import.CreationTime,
                                  WarehouseId = warehouse.Id,
                                  QuantityImport = importdetail.Quantity,
                                  ImportRequestDetailId = importdetail.Id,
                                  ImportRequestId = importdetail.ImportRequestId,
                                  Quantity = warehouselocation.Quantity,
                                  Block = warehouselocation.Block,
                                  ItemsName = item.Name,
                                  ItemId = item.Id,
                                  WarehouseLocationItemsId = warehouselocation.Id

                              }).OrderByDescending(x => x.ImportRequestCode).ToList();
                var WarehouseId = result.Select(x => x.WarehouseId).Distinct().ToList();
                var ImportRequestDetailId = result.Select(x => x.ImportRequestDetailId).Distinct().ToList();

                //for (int i = 0; i < result.Count(); i++)
                //{
                //    string locationName = await _warehouseItemAppService.GetinfoBin(Int32.Parse(result[i].Block));
                //    result[i].BlockName = locationName;
                //}
                //var abc = from line in querywarehouselocation
                //          group line by new {
                //              line.Block,
                //              line.WarehouseId,
                //              line.ItemId,
                //          } into g
                //          select new WarehouseLocationItemsDto
                //          {
                //              ItemId = g.First().ItemId,
                //              Quantity = g.Sum(pc => pc.Quantity),
                //              WarehouseId = g.First().WarehouseId,
                //              ImportRequestDetailId = g.First().ImportRequestDetailId,
                //              Block = g.First().Block,
                //          };

                //result = (from results in result
                //         group results by new { results.Block, results.ItemId }  into g
                //         select new WarehouseLocationItemsDto
                //         {
                //             Id = g.First().Id,
                //             ItemCode = g.First().ItemCode,
                //             ImportRequestCode = g.First().ImportRequestCode,
                //             WarehouseName = g.First().WarehouseName,
                //             ExpireDate = g.First().ExpireDate,
                //             ImportDate = g.First().ImportDate,
                //             WarehouseId = g.First().WarehouseId,
                //             QuantityImport = g.First().QuantityImport,
                //             ImportRequestDetailId = g.First().ImportRequestDetailId,
                //             ImportRequestId = g.First().ImportRequestId,
                //             BlockName = g.First().BlockName,
                //             Block = g.First().Block,
                //             Quantity = g.Sum(pc=>pc.Quantity),
                //             ItemsName = g.First().ItemsName,
                //             ItemId = g.First().ItemId,
                //             WarehouseLocationItemsId = g.First().WarehouseLocationItemsId,
                //         }).ToList();

                //if (abc.Count() > 0)
                //{
                //    result = (from results in result
                //              join abcd in abc on results.Block equals abcd.Block into g
                //              from subpet in g.DefaultIfEmpty()
                //              select new WarehouseLocationItemsDto
                //              {
                //                  Id = results.Id,
                //                  ItemCode = results.ItemCode,
                //                  ImportRequestCode = results.ImportRequestCode,
                //                  WarehouseName = results.WarehouseName,
                //                  ExpireDate = results.ExpireDate,
                //                  ImportDate = results.ImportDate,
                //                  WarehouseId = results.WarehouseId,
                //                  QuantityImport = results.QuantityImport,
                //                  ImportRequestDetailId = results.ImportRequestDetailId,
                //                  ImportRequestId = results.ImportRequestId,
                //                  QuantityLocaiton = subpet?.Quantity ?? 0,
                //                  BlockName = results.BlockName,
                //                  Block = results.Block,
                //                  Quantity = results.Quantity,
                //                  ItemsName = results.ItemsName,
                //                  ItemId= results.ItemId,
                //                  WarehouseLocationItemsId = results.WarehouseLocationItemsId,

                //              }).Where(x => x.QuantityImport > 0).OrderByDescending(x => x.ImportRequestCode).ToList();
                //}

                return new PagedResultDto<WarehouseLocationItemsDto>(result.Count(), result);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="input"></param>
        /// <returns>All items chưa sắp xếp </returns>
        /// <exception cref="UserFriendlyException"></exception>
        // get all items chưa được sắp sếp 
        public async Task<PagedResultDto<WarehouseLocationItemsDto>> GetAllItems(WarehouseLocationItemsInput input)
        {
            try
            {
                // all các phiếu nhập
                var query = _importRequestDetailrepository.GetAll().ToList();
                var queryimport = _importRequestrepository.GetAll().ToList();
                // thông tin trong kho
                var querywarehouse = _warehouserepository.GetAll().ToList();
                // thông tin items 
                var queryitems = _itemrepository.GetAll().ToList();

                // thông tin chi tiết các phiếu nhập trong kho  
                var result = (from importdetail in query
                              join item in queryitems on importdetail.ItemId equals item.Id
                              join import in queryimport on importdetail.ImportRequestId equals import.Id
                              join warehouse in querywarehouse on import.WarehouseDestinationId equals warehouse.Id
                              select new WarehouseLocationItemsDto
                              {
                                  ItemCode = item.ItemCode,
                                  ImportRequestCode = import.Code,
                                  WarehouseName = warehouse.Name,
                                  ExpireDate = import.RequestDate,
                                  ImportDate = import.CreationTime,
                                  WarehouseId = warehouse.Id,
                                  QuantityImport = importdetail.Quantity,
                                  ImportRequestDetailId = importdetail.Id,
                                  ImportRequestId = importdetail.ImportRequestId,
                                  ItemId = item.Id,

                              }).OrderByDescending(x => x.ImportRequestCode).ToList();
                // List danh sách kho 
                var WarehouseId = result.Select(x => x.WarehouseId).Distinct().ToList();
                // Id chi tiết phiếu nhập 
                var ImportRequestDetailId = result.Select(x => x.ImportRequestDetailId)
                                                .Distinct()
                                                .ToList();
                // vị trí Item nhằm ở vị trí nào với idwarehouseId , Id chi tiết phiếu nhập
                var querywarehouselocation = _warehouseLocationItemrepository.GetAll()
                                                                            .Where(x => WarehouseId.Contains(x.WarehouseId) && ImportRequestDetailId.Contains(x.ImportRequestDetailId))
                                                                            .Select(x => new { x.ItemId, x.Quantity, x.WarehouseId, x.ImportRequestDetailId, x.Block })
                                                                          .ToList();

                // tổng Items nằm trong vị trí với Id chi tiết phiếu nhập
                var abc = from line in querywarehouselocation
                          group line by line.ImportRequestDetailId into g
                          select new WarehouseLocationItemsDto
                          {
                              ItemId = g.First().ItemId,
                              Quantity = g.Sum(pc => pc.Quantity),
                              WarehouseId = g.First().WarehouseId,
                              ImportRequestDetailId = g.First().ImportRequestDetailId,
                          };

                if (abc.Count() > 0)
                {
                    result = (from results in result
                              join abcd in abc on results.ImportRequestDetailId equals abcd.ImportRequestDetailId into g
                              from subpet in g.DefaultIfEmpty()
                              select new WarehouseLocationItemsDto
                              {
                                  ItemCode = results.ItemCode,
                                  ImportRequestCode = results.ImportRequestCode,
                                  WarehouseName = results.WarehouseName,
                                  ExpireDate = results.ExpireDate,
                                  ImportDate = results.ImportDate,
                                  WarehouseId = results.WarehouseId,
                                  QuantityImport = results.QuantityImport - subpet?.Quantity ?? results.QuantityImport,
                                  ImportRequestDetailId = results.ImportRequestDetailId,
                                  ImportRequestId = results.ImportRequestId,
                                  ItemId = results.ItemId,
                              }).Where(x => x.QuantityImport > 0).OrderBy(x => x.WarehouseId).ToList();
                }
                return new PagedResultDto<WarehouseLocationItemsDto>(result.Count(), result);
            }

            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WarehouseLocationItemsDto>> GetAllListItem(WarehouseLocationItemsInput input)
        {
            var query = _warehouseLocationItemrepository.GetAll().Where(x => x.ItemCode.Contains(input.SearchTerm.Trim()));
            var query1 = _warehouserepository.GetAll();
            var query2 = _Inventoryrepository.GetAll();
            var query3 = _wareItemhouserepository.GetAll();

            List<WarehouseLocationItemsDto> listItem = new List<WarehouseLocationItemsDto>();

            var result = (from item in query
                          join ware in query1 on item.WarehouseId equals ware.Id
                          join inven in query2 on item.ItemCode equals inven.ItemCode
                          select new WarehouseLocationItemsDto
                          {
                              Id = item.Id,
                              ItemCode = item.ItemCode,
                              WarehouseName = ware.Name,
                              WarehouseId = ware.Id,
                              ItemId = item.ItemId,
                              ParcelId = item.ParcelId,
                              Block = item.Block,
                              Shelf = item.Shelf,
                              Floor = item.Floor,
                              Quantity = item.Quantity,
                              ExpireDate = item.ExpireDate,
                              ImportDate = inven.CreationTime,
                              ImportRequestDetailId = item.ImportRequestDetailId,
                              ImportRequestId = item.ImportRequestId,
                              DescriptionLocation = item.DescriptionLocation,
                              //BlockName = nameBlock /*+ " , " + nameShelf + " , " + nameFloor,*/
                          }).ToList();

            for (int i = 0; i < result.Count(); i++)
            {
                if (result[i].Shelf == null && result[i].Floor == null && result[i].Block == null)
                {
                    var valueNew = new WarehouseLocationItemsDto
                    {
                        Id = result[i].Id,
                        ItemCode = result[i].ItemCode,
                        WarehouseName = result[i].WarehouseName,
                        WarehouseId = result[i].WarehouseId,
                        ItemId = result[i].ItemId,
                        ParcelId = result[i].ParcelId,
                        Block = result[i].Block,
                        Shelf = result[i].Shelf,
                        Floor = result[i].Floor,
                        Quantity = result[i].Quantity,
                        ExpireDate = result[i].ExpireDate,
                        ImportDate = result[i].CreationTime,
                        ImportRequestDetailId = result[i].ImportRequestDetailId,
                        ImportRequestId = result[i].ImportRequestId,
                        DescriptionLocation = result[i].DescriptionLocation,
                        BlockName = "Chưa được sắp xếp",
                    };
                    listItem.Add(valueNew);

                }
                if (result[i].Shelf != null && result[i].Floor == null && result[i].Block == null)
                {
                    var nameLocaBlock = query3.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();

                    var valueNew = new WarehouseLocationItemsDto
                    {
                        Id = result[i].Id,
                        ItemCode = result[i].ItemCode,
                        WarehouseName = result[i].WarehouseName,
                        WarehouseId = result[i].WarehouseId,
                        ItemId = result[i].ItemId,
                        ParcelId = result[i].ParcelId,
                        Block = result[i].Block,
                        Shelf = result[i].Shelf,
                        Floor = result[i].Floor,
                        Quantity = result[i].Quantity,
                        ExpireDate = result[i].ExpireDate,
                        ImportDate = result[i].CreationTime,
                        ImportRequestDetailId = result[i].ImportRequestDetailId,
                        ImportRequestId = result[i].ImportRequestId,
                        DescriptionLocation = result[i].DescriptionLocation,
                        BlockName = nameLocaBlock[0].ToString(),
                    };
                    listItem.Add(valueNew);

                }
                if (result[i].Floor != null && result[i].Block == null)
                {
                    var nameLocaBlock = query3.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();
                    var nameLocaShefl = query3.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Name).ToList();

                    var valueNew = new WarehouseLocationItemsDto
                    {
                        Id = result[i].Id,
                        ItemCode = result[i].ItemCode,
                        WarehouseName = result[i].WarehouseName,
                        WarehouseId = result[i].WarehouseId,
                        ItemId = result[i].ItemId,
                        ParcelId = result[i].ParcelId,
                        Block = result[i].Block,
                        Shelf = result[i].Shelf,
                        Floor = result[i].Floor,
                        Quantity = result[i].Quantity,
                        ExpireDate = result[i].ExpireDate,
                        ImportDate = result[i].CreationTime,
                        ImportRequestDetailId = result[i].ImportRequestDetailId,
                        ImportRequestId = result[i].ImportRequestId,
                        DescriptionLocation = result[i].DescriptionLocation,
                        BlockName = nameLocaBlock[0].ToString() + " - " + nameLocaShefl[0].ToString(),
                    };
                    listItem.Add(valueNew);
                }
                if (result[i].Floor != null && result[i].Block != null)
                {
                    var nameLocaBlock = query3.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();
                    var nameLocaShefl = query3.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Name).ToList();
                    var nameLocaFloor = query3.Where(x => x.Id == int.Parse(result[i].Block)).Select(x => x.Name).ToList();

                    var valueNew = new WarehouseLocationItemsDto
                    {
                        Id = result[i].Id,
                        ItemCode = result[i].ItemCode,
                        WarehouseName = result[i].WarehouseName,
                        WarehouseId = result[i].WarehouseId,
                        ItemId = result[i].ItemId,
                        ParcelId = result[i].ParcelId,
                        Block = result[i].Block,
                        Shelf = result[i].Shelf,
                        Floor = result[i].Floor,
                        Quantity = result[i].Quantity,
                        ExpireDate = result[i].ExpireDate,
                        ImportDate = result[i].CreationTime,
                        ImportRequestDetailId = result[i].ImportRequestDetailId,
                        ImportRequestId = result[i].ImportRequestId,
                        DescriptionLocation = result[i].DescriptionLocation,
                        BlockName = nameLocaBlock[0].ToString() + " - " + nameLocaShefl[0].ToString() + " - " + nameLocaFloor[0].ToString(),
                    };
                    listItem.Add(valueNew);
                }
            }

            return new PagedResultDto<WarehouseLocationItemsDto>(
                    listItem.Count(),
                    listItem
                );
        }

        public async Task<int> TotalInLocation(int idlocation)
        {
            var querywarehouselocation = _warehouseLocationItemrepository.GetAll().Where(x => Int32.Parse(x.Block) == (idlocation)).Select(x => new { x.ItemId, x.Quantity, x.WarehouseId, x.ImportRequestDetailId, x.Block }).ToList();

            var abce = from line in querywarehouselocation
                       group line by line.ImportRequestDetailId into g
                       select new WarehouseLocationItemsDto
                       {
                           ItemId = g.First().ItemId,
                           Quantity = g.Sum(pc => pc.Quantity),
                           WarehouseId = g.First().WarehouseId,
                           ImportRequestDetailId = g.First().ImportRequestDetailId,
                           Block = g.First().Block,
                       };
            return 1;
        }

        public async Task<PagedResultDto<WarehouseLocationItemsDto>> GetLocationItems(WarehouseLocationItemsInput input)
        {
            try
            {
                var query2 = _warehouseLocationItemrepository.GetAll()
                    .Where(u => u.WarehouseId == input.WarehouseId)
                    .WhereIf(!string.IsNullOrEmpty(input.Block), u => u.Block == input.Block || u.Floor == input.Block || u.Shelf == input.Block)
                    .Where(u => u.ItemId == input.ItemId).ToList();



                var newItemId = ObjectMapper.Map<List<WarehouseLocationItemsDto>>(query2);
                foreach (var item in newItemId)
                {
                    if (item.Block != null)
                    {
                        var queryWarehouseLocation = _warehouseItemAppService.GetById(new EntityDto(Int32.Parse(item.Block)));
                        item.BlockName = queryWarehouseLocation.Result.Name;
                    }
                    if (item.Shelf != null)
                    {
                        var queryWarehouseLocation1 = _warehouseItemAppService.GetById(new EntityDto(Int32.Parse(item.Shelf)));
                        item.ShelfName = queryWarehouseLocation1.Result.Name;

                    }
                    if (item.Floor != null)
                    {
                        var queryWarehouseLocation2 = _warehouseItemAppService.GetById(new EntityDto(Int32.Parse(item.Floor)));
                        item.FloorName = queryWarehouseLocation2.Result.Name;

                    }

                }
                return new PagedResultDto<WarehouseLocationItemsDto>(
                           newItemId.Count(),
                           newItemId
                       );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task<PagedResultDto<WarehouseLocationItemsDto>> GetByIdBlock(WarehouseLocationItemsInput input)
        {
            try
            {
                var queryImport = _importRequestrepository.GetAll().ToList();
                var query = _importRequestDetailrepository.GetAll().ToList();
                var queryWareItem = _wareItemhouserepository.GetAll();
                var querywarehouse = _warehouserepository.GetAll().ToList();
                var queryitems = _itemrepository.GetAll().ToList();
                var querywarehouselocation = _warehouseLocationItemrepository.GetAll().Where(x => x.WarehouseId == input.WarehouseId && input.Block.Contains(x.Shelf)).ToList();

                List<WarehouseLocationItemsDto> warehouseItems = new List<WarehouseLocationItemsDto>();

                var result = (from warehouseLocation in querywarehouselocation
                              join import in queryImport on warehouseLocation.ImportRequestId equals import.Id
                              //join importDetail in query on import.Id equals importDetail.ImportRequestId
                              let unitName = (from uni in query where import.Id == uni.ImportRequestId select uni.UnitName).ToArray()
                              join warehouse in querywarehouse on warehouseLocation.WarehouseId equals warehouse.Id
                              join item in queryitems on warehouseLocation.ItemId equals item.Id
                              select new WarehouseLocationItemsDto
                              {
                                  Id = item.Id,
                                  ItemCode = item.ItemCode,
                                  ImportStatus = import.ImportStatus,
                                  WarehouseName = warehouse.Name,
                                  ExpireDate = import.RequestDate,
                                  ImportDate = import.CreationTime,
                                  WarehouseId = warehouse.Id,
                                  ImportRequestDetailId = warehouseLocation.ImportRequestDetailId,
                                  ImportRequestId = import.Id,
                                  Quantity = warehouseLocation.Quantity,
                                  Shelf = warehouseLocation.Shelf,
                                  ItemsName = item.Name,
                                  ItemId = item.Id,
                                  WarehouseLocationItemsId = warehouseLocation.Id,
                                  UnitName = unitName[0],
                                  ImportRequestCode = import.Code,
                                  Block = warehouseLocation.Block,
                                  Floor = warehouseLocation.Floor,
                                  IsItems = warehouseLocation.IsItems,
                                  QuantityReality = warehouseLocation.QuantityReality,
                                  Note = warehouseLocation.Note,
                              }).OrderByDescending(x => x.ImportRequestCode).Distinct().ToList();

                for (int i = 0; i < result.Count(); i++)
                {
                    if (result[i].Floor == null && result[i].Block == null)
                    {
                        var nameBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Code).ToList();
                        var nameLocaBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();
                        var cateCode = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.CategoryCode).ToList();
                        var valueNew = new WarehouseLocationItemsDto
                        {
                            Id = result[i].Id,
                            ItemCode = result[i].ItemCode,
                            WarehouseName = result[i].WarehouseName,
                            ImportStatus = result[i].ImportStatus,
                            ExpireDate = result[i].ExpireDate,
                            ImportDate = result[i].CreationTime,
                            WarehouseId = result[i].WarehouseId,
                            ImportRequestDetailId = result[i].ImportRequestDetailId,
                            ImportRequestId = result[i].ImportRequestId,
                            Quantity = result[i].Quantity,
                            Shelf = result[i].Shelf,
                            ItemsName = result[i].ItemsName,
                            ItemId = result[i].ItemId,
                            WarehouseLocationItemsId = result[i].WarehouseLocationItemsId,
                            UnitName = result[i].UnitName,
                            ImportRequestCode = result[i].ImportRequestCode,
                            Block = result[i].Block,
                            Floor = result[i].Floor,
                            BlockName = cateCode[0].ToString() + nameBlock[0].ToString() + "0000",
                            LocationName = nameLocaBlock[0].ToString(),
                            IsItems = result[i].IsItems,
                            QuantityReality = result[i].QuantityReality,
                            Note = result[i].Note,
                        };
                        warehouseItems.Add(valueNew);

                    }
                    if (result[i].Block == null && result[i].Floor != null)
                    {
                        var nameBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Code).ToList();
                        var nameFloor = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Code).ToList();

                        var nameLocaBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();
                        var nameLocaFloor = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Name).ToList();

                        var cateCode = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.CategoryCode).ToList();
                        var valueNew = new WarehouseLocationItemsDto
                        {
                            Id = result[i].Id,
                            ItemCode = result[i].ItemCode,
                            WarehouseName = result[i].WarehouseName,
                            ImportStatus = result[i].ImportStatus,
                            ExpireDate = result[i].ExpireDate,
                            ImportDate = result[i].CreationTime,
                            WarehouseId = result[i].WarehouseId,
                            ImportRequestDetailId = result[i].ImportRequestDetailId,
                            ImportRequestId = result[i].ImportRequestId,
                            Quantity = result[i].Quantity,
                            Shelf = result[i].Shelf,
                            ItemsName = result[i].ItemsName,
                            ItemId = result[i].ItemId,
                            WarehouseLocationItemsId = result[i].WarehouseLocationItemsId,
                            UnitName = result[i].UnitName,
                            ImportRequestCode = result[i].ImportRequestCode,
                            Block = result[i].Block,
                            Floor = result[i].Floor,
                            BlockName = cateCode[0].ToString() + nameBlock[0].ToString() + nameFloor[0].ToString() + "00",
                            LocationName = nameLocaBlock[0].ToString() + " - " + nameLocaFloor[0].ToString(),
                            IsItems = result[i].IsItems,
                            QuantityReality = result[i].QuantityReality,
                            Note = result[i].Note,
                        };
                        warehouseItems.Add(valueNew);
                    }
                    if (result[i].Floor != null && result[i].Block != null)
                    {
                        var nameBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Code).ToList();
                        var nameFloor = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Code).ToList();
                        var nameShefl = queryWareItem.Where(x => x.Id == int.Parse(result[i].Block)).Select(x => x.Code).ToList();

                        var nameLocaBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();
                        var nameLocaFloor = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Name).ToList();
                        var nameLocaShelf = queryWareItem.Where(x => x.Id == int.Parse(result[i].Block)).Select(x => x.Name).ToList();

                        var cateCode = queryWareItem.Where(x => x.Id == int.Parse(result[i].Block)).Select(x => x.CategoryCode).ToList();
                        var valueNew = new WarehouseLocationItemsDto
                        {
                            Id = result[i].Id,
                            ItemCode = result[i].ItemCode,
                            WarehouseName = result[i].WarehouseName,
                            ImportStatus = result[i].ImportStatus,
                            ExpireDate = result[i].ExpireDate,
                            ImportDate = result[i].CreationTime,
                            WarehouseId = result[i].WarehouseId,
                            ImportRequestDetailId = result[i].ImportRequestDetailId,
                            ImportRequestId = result[i].ImportRequestId,
                            Quantity = result[i].Quantity,
                            Shelf = result[i].Shelf,
                            ItemsName = result[i].ItemsName,
                            ItemId = result[i].ItemId,
                            WarehouseLocationItemsId = result[i].WarehouseLocationItemsId,
                            UnitName = result[i].UnitName,
                            ImportRequestCode = result[i].ImportRequestCode,
                            Block = result[i].Block,
                            Floor = result[i].Floor,
                            IsItems = result[i].IsItems,
                            QuantityReality = result[i].QuantityReality,
                            Note = result[i].Note,
                            BlockName = cateCode[0].ToString() + nameBlock[0].ToString() + nameFloor[0].ToString() + nameShefl[0].ToString(),
                            LocationName = nameLocaBlock[0].ToString() + " - " + nameLocaFloor[0].ToString() + " - " + nameLocaShelf[0].ToString(),
                        };
                        warehouseItems.Add(valueNew);
                    }
                }

                return new PagedResultDto<WarehouseLocationItemsDto>(warehouseItems.Count(), warehouseItems);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Delete(WarehouseLocationItemsInput input)
        {
            try
            {
                var idDelete = _warehouseLocationItemrepository.GetAll().Where(x => x.ItemId == input.ItemId && x.WarehouseId == input.WarehouseId).FirstOrDefault().Id;
                await _warehouseLocationItemrepository.DeleteAsync(idDelete);
                return idDelete;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> CountInLocation(WarehouseLocationItemsInput input)
        {
            try
            {
                var query = _warehouseLocationItemrepository.GetAll().Where(x => x.Block == input.Block || x.Shelf == input.Block || x.Floor == input.Block).Select(x => x.Quantity).ToList();
                var sum = (long)query.Sum();
                return sum;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<long> CountInLocationFree(WarehouseLocationItemsInput input)
        {
            try
            {
                var query = _warehouseLocationItemrepository.GetAll().Where(x => x.Block == input.Block || x.Shelf == input.Block || x.Floor == input.Block).Select(x => x.Quantity).ToList();
                var querywarehouseItem = _wareItemhouserepository.GetAll().Where(x => x.Id == Int32.Parse(input.Block)).First().UnitMax;
                var sum = querywarehouseItem - (long)query.Sum();
                return sum;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// cập nhật quantity lúc xuất kho
        /// </summary>
        /// <param name="input"> Id của warehouselocation </param>
        /// <returns> </returns>
        public async Task<long> Update(WarehouseLocationItemsDto input)
        {
            WarehouseLocationItems unit = await _warehouseLocationItemrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            unit.Quantity = unit.Quantity - input.Quantity;
            if (unit.Quantity == 0)
            {
                await _warehouseLocationItemrepository.DeleteAsync(unit);
            }
            await _warehouseLocationItemrepository.UpdateAsync(unit);
            return input.Id.Value;
        }

        public async Task<WarehouseLocationItemsDto> GetAsync(EntityDto itemId)
        {
            var item = _warehouseLocationItemrepository.Get(itemId.Id);
            WarehouseLocationItemsDto newItem = ObjectMapper.Map<WarehouseLocationItemsDto>(item);
            return newItem;
        }

        public async Task<PagedResultDto<WarehouseLocationItemsDto>> GetItemSameInWare(WarehouseLocationItemsInput input)
        {
            try
            {
                var query = _warehouseLocationItemrepository.GetAll().Where(x => x.WarehouseId == input.WarehouseId && x.ItemId == input.ItemId
                && x.ImportRequestDetailId == input.ImportRequestDetailId).
                WhereIf(!string.IsNullOrEmpty(input.Block), x => x.Block == input.Block)
               .WhereIf(!string.IsNullOrEmpty(input.Shelf), x => x.Shelf == input.Shelf)
                .WhereIf(!string.IsNullOrEmpty(input.Floor), x => x.Floor == input.Floor).ToList();

                var result = ObjectMapper.Map<List<WarehouseLocationItemsDto>>(query);

                return new PagedResultDto<WarehouseLocationItemsDto>(query.Count(), result);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateItemQuantity(WarehouseLocationItemsDto input)
        {
            WarehouseLocationItems unit = await _warehouseLocationItemrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            unit.Quantity = input.Quantity;
            await _warehouseLocationItemrepository.UpdateAsync(unit);
            return input.Id.Value;
        }

        public async Task<PagedResultDto<WarehouseLocationItemsDto>> GetByInImport(WarehouseLocationItemsInput input)
        {
            try
            {
                var queryImport = _importRequestrepository.GetAll().Where(x => x.Id == input.ImportId).ToList();
                var query = _importRequestDetailrepository.GetAll().Where(x => x.ImportRequestId == input.ImportId).ToList();
                var queryWareItem = _wareItemhouserepository.GetAll();
                var querywarehouse = _warehouserepository.GetAll().ToList();
                var queryitems = _itemrepository.GetAll().ToList();
                var querywarehouselocation = _warehouseLocationItemrepository.GetAll().Where(x => x.ImportRequestId == input.ImportId).ToList();

                List<WarehouseLocationItemsDto> warehouseItems = new List<WarehouseLocationItemsDto>();

                var result = (from warehouseLocation in querywarehouselocation
                              join import in queryImport on warehouseLocation.ImportRequestId equals import.Id
                              join importDetail in query on warehouseLocation.ImportRequestDetailId equals importDetail.Id
                              join warehouse in querywarehouse on warehouseLocation.WarehouseId equals warehouse.Id
                              join item in queryitems on warehouseLocation.ItemId equals item.Id
                              select new WarehouseLocationItemsDto
                              {
                                  Id = warehouseLocation.Id,
                                  ItemCode = item.ItemCode,
                                  WarehouseName = warehouse.Name,
                                  ImportStatus = import.ImportStatus,
                                  WarehouseId = warehouse.Id,
                                  ImportRequestDetailId = warehouseLocation.ImportRequestDetailId,
                                  ImportRequestId = import.Id,
                                  Quantity = warehouseLocation.Quantity,
                                  Shelf = warehouseLocation.Shelf,
                                  ItemsName = item.Name,
                                  ItemId = item.Id,
                                  WarehouseLocationItemsId = warehouseLocation.Id,
                                  UnitName = importDetail.UnitName,
                                  ImportRequestCode = import.Code,
                                  Block = warehouseLocation.Block,
                                  Floor = warehouseLocation.Floor,
                                  IsItems = warehouseLocation.IsItems,
                                  QuantityReality = warehouseLocation.QuantityReality,
                                  Note = warehouseLocation.Note,
                                  Price = importDetail.ImportPrice,
                              }).OrderByDescending(x => x.ImportRequestCode).Distinct().ToList();

                for (int i = 0; i < result.Count(); i++)
                {
                    if (result[i].Floor == null && result[i].Block == null)
                    {
                        var nameBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Code).ToList();
                        var nameLocaBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();
                        var cateCode = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.CategoryCode).ToList();
                        var valueNew = new WarehouseLocationItemsDto
                        {
                            Id = result[i].Id,
                            ItemCode = result[i].ItemCode,
                            WarehouseName = result[i].WarehouseName,
                            ImportStatus = result[i].ImportStatus,
                            WarehouseId = result[i].WarehouseId,
                            ImportRequestDetailId = result[i].ImportRequestDetailId,
                            ImportRequestId = result[i].ImportRequestId,
                            Quantity = result[i].Quantity,
                            Shelf = result[i].Shelf,
                            ItemsName = result[i].ItemsName,
                            ItemId = result[i].ItemId,
                            WarehouseLocationItemsId = result[i].WarehouseLocationItemsId,
                            UnitName = result[i].UnitName,
                            ImportRequestCode = result[i].ImportRequestCode,
                            Block = result[i].Block,
                            Floor = result[i].Floor,
                            BlockName = cateCode[0].ToString() + nameBlock[0].ToString() + "0000",
                            LocationName = nameLocaBlock[0].ToString(),
                            IsItems = result[i].IsItems,
                            QuantityReality = result[i].QuantityReality,
                            Note = result[i].Note,
                            Price = result[i].Price,
                        };
                        warehouseItems.Add(valueNew);

                    }
                    if (result[i].Block == null && result[i].Floor != null)
                    {
                        var nameBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Code).ToList();
                        var nameFloor = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Code).ToList();

                        var nameLocaBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();
                        var nameLocaFloor = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Name).ToList();

                        var cateCode = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.CategoryCode).ToList();
                        var valueNew = new WarehouseLocationItemsDto
                        {
                            Id = result[i].Id,
                            ItemCode = result[i].ItemCode,
                            ImportStatus = result[i].ImportStatus,
                            WarehouseName = result[i].WarehouseName,
                            WarehouseId = result[i].WarehouseId,
                            ImportRequestDetailId = result[i].ImportRequestDetailId,
                            ImportRequestId = result[i].ImportRequestId,
                            Quantity = result[i].Quantity,
                            Shelf = result[i].Shelf,
                            ItemsName = result[i].ItemsName,
                            ItemId = result[i].ItemId,
                            WarehouseLocationItemsId = result[i].WarehouseLocationItemsId,
                            UnitName = result[i].UnitName,
                            ImportRequestCode = result[i].ImportRequestCode,
                            Block = result[i].Block,
                            Floor = result[i].Floor,
                            BlockName = cateCode[0].ToString() + nameBlock[0].ToString() + nameFloor[0].ToString() + "00",
                            LocationName = nameLocaBlock[0].ToString() + " - " + nameLocaFloor[0].ToString(),
                            IsItems = result[i].IsItems,
                            QuantityReality = result[i].QuantityReality,
                            Note = result[i].Note,
                            Price = result[i].Price,
                        };
                        warehouseItems.Add(valueNew);
                    }
                    if (result[i].Floor != null && result[i].Block != null)
                    {
                        var nameBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Code).ToList();
                        var nameFloor = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Code).ToList();
                        var nameShefl = queryWareItem.Where(x => x.Id == int.Parse(result[i].Block)).Select(x => x.Code).ToList();

                        var nameLocaBlock = queryWareItem.Where(x => x.Id == int.Parse(result[i].Shelf)).Select(x => x.Name).ToList();
                        var nameLocaFloor = queryWareItem.Where(x => x.Id == int.Parse(result[i].Floor)).Select(x => x.Name).ToList();
                        var nameLocaShelf = queryWareItem.Where(x => x.Id == int.Parse(result[i].Block)).Select(x => x.Name).ToList();

                        var cateCode = queryWareItem.Where(x => x.Id == int.Parse(result[i].Block)).Select(x => x.CategoryCode).ToList();
                        var valueNew = new WarehouseLocationItemsDto
                        {
                            Id = result[i].Id,
                            ItemCode = result[i].ItemCode,
                            ImportStatus = result[i].ImportStatus,
                            WarehouseName = result[i].WarehouseName,
                            WarehouseId = result[i].WarehouseId,
                            ImportRequestDetailId = result[i].ImportRequestDetailId,
                            ImportRequestId = result[i].ImportRequestId,
                            Quantity = result[i].Quantity,
                            Shelf = result[i].Shelf,
                            ItemsName = result[i].ItemsName,
                            ItemId = result[i].ItemId,
                            WarehouseLocationItemsId = result[i].WarehouseLocationItemsId,
                            UnitName = result[i].UnitName,
                            ImportRequestCode = result[i].ImportRequestCode,
                            Block = result[i].Block,
                            Floor = result[i].Floor,
                            IsItems = result[i].IsItems,
                            QuantityReality = result[i].QuantityReality,
                            Note = result[i].Note,
                            Price = result[i].Price,
                            BlockName = cateCode[0].ToString() + nameBlock[0].ToString() + nameFloor[0].ToString() + nameShefl[0].ToString(),
                            LocationName = nameLocaBlock[0].ToString() + " - " + nameLocaFloor[0].ToString() + " - " + nameLocaShelf[0].ToString(),
                        };
                        warehouseItems.Add(valueNew);
                    }
                }

                return new PagedResultDto<WarehouseLocationItemsDto>(warehouseItems.Count(), warehouseItems.Distinct().ToList());
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateIpoSubmit(WarehouseLocationItemsDto input)
        {
            WarehouseLocationItems IMP = await _warehouseLocationItemrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            IMP.QuantityReality = input.QuantityReality;
            IMP.Note = input.Note;
            IMP.IsItems = input.IsItems;
            
         
            await _warehouseLocationItemrepository.UpdateAsync(IMP);
            return IMP.Id;
        }
    }
}
