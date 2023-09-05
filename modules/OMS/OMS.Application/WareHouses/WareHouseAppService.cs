using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.WareHouses.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using bbk.netcore.Storage.FileSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WareHouses
{
    [AbpAuthorize]
    public class WareHouseAppService : ApplicationService, IWareHouseAppService
    {
        private readonly IRepository<Warehouse> _warehouserepository;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;

        public WareHouseAppService(IRepository<Warehouse> warehouserepository, IFileSystemBlobProvider fileSystemBlobProvider)
        {
            _warehouserepository = warehouserepository;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }

        public async Task<int> Create(WarehouseCreateDto input)
        {
            try
            {
                //Warehouse warehouse = ObjectMapper.Map<Warehouse>(input);
                //var newId = await _warehouserepository.InsertAndGetIdAsync(warehouse);
                //return newId;

                var query = await _warehouserepository.GetAll().ToListAsync();
                string sinhma(string ma)
                {
                    string s = ma.Substring(4, ma.Length - 4);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "KH-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "KH-" + "0" + Convert.ToString(i);
                    else return "KH-" + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _warehouserepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }

                input.Code = sinhma(ma.ToString());
                Warehouse newItemId = ObjectMapper.Map<Warehouse>(input);
                var newId = await _warehouserepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _warehouserepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        

        public async Task<PagedResultDto<WarehouseListDto>> GetAll(GetWarehouseInput input)
        {
            try
            {
                var query = _warehouserepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Name.Contains(input.SearchTerm))
                    .OrderBy(x => x.Id);

                //Lay file data Json
                string fileProvince = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.DataAddress + @"\\" + "province.json"));
                string fileDistrict = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.DataAddress + @"\\" + "district.json"));
                string fileVillage = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.DataAddress + @"\\" + "village.json"));

                var dataProvince = ReadJson<Address>.ConvertJsonToObject(fileProvince);
                var dataDistrict = ReadJson<Address>.ConvertJsonToObject(fileDistrict);
                var dataVillage = ReadJson<Address>.ConvertJsonToObject(fileVillage);

                var query1 = (from city in dataProvince.Addresses
                              join district in dataDistrict.Addresses on city.Id equals district.SuperiorId
                              join village in dataVillage.Addresses on district.Id equals village.SuperiorId
                              join q in query on village.Id equals q.WardsId
                              select new WarehouseListDto
                              {
                                  Id = q.Id,
                                  Name = q.Name,
                                  TypeWarehouse = q.TypeWarehouse,
                                  Code = q.Code,
                                  CityId = q.CityId,
                                  DistrictId = q.DistrictId,
                                  WardsId = q.WardsId,
                                  Number = q.Number,
                                  Longitude = q.Longitude,
                                  Lattitude = q.Lattitude,
                                  Description = q.Description,
                                  TypeCode = q.TypeCode,
                                  DeleteFlag = q.DeleteFlag,
                                  CityName = city.Name,
                                  DistrictName = district.Name,
                                  WardsName = village.Name
                              }).OrderByDescending(x => x.Id).ToList();

                var count = query1.Count();

                return new PagedResultDto<WarehouseListDto>(
                    count, query1);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<int> Update(WarehouseEditDto input)
        {
            try
            {
                Warehouse warehouse = await _warehouserepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                input.Code = warehouse.Code;
                ObjectMapper.Map(input, warehouse);
                await _warehouserepository.UpdateAsync(warehouse);
                return input.Id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<Address> GetAddress(string filePath, string superiorId)
        {
            string file = _fileSystemBlobProvider.GetFilePath(new Storage.StorageProviderGetArgs(PersonalProfileCoreConsts.DataAddress + @"\\" + filePath));
            var data = ReadJson<Address>.ConvertJsonToObject(file);
            Address result = new Address
            {
                Addresses = data.Addresses.WhereIf(!string.IsNullOrEmpty(superiorId), u => u.SuperiorId == superiorId).OrderBy(u => u.Name).ToList()
            };
            return await Task.FromResult(result);
        }

        public async Task<WarehouseListDto> GetAsync(EntityDto<long> itemId)
        {
            try
            {
                var item = _warehouserepository.Get((int)(itemId.Id));
                WarehouseListDto newItem = ObjectMapper.Map<WarehouseListDto>(item);
                return newItem;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<WarehouseListDto>> GetWarehouseList()
        {
            try
            {
                var query = _warehouserepository.GetAll().OrderBy(x => x.Id);

                var warehouseList = ObjectMapper.Map<List<WarehouseListDto>>(query);
                return warehouseList;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WarehouseListDto>> GetAllExceptId(GetWarehouseInput input)
        {
            try
            {
                var query = _warehouserepository.GetAll().Where(x => x.Id != input.Id)
                    .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Name.Contains(input.SearchTerm))
                    .OrderBy(x => x.Id).ToList();

                var warehouseList = ObjectMapper.Map<List<WarehouseListDto>>(query);

                return new PagedResultDto<WarehouseListDto>(warehouseList.Count(), warehouseList);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
