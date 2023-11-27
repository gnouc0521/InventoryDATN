using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.WarehouseItems.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseItems
{
  [AbpAuthorize]
    public class WarehouseItemAppService : ApplicationService , IWarehouseItemAppService
    {
        private readonly IRepository<Warehouse> _warehouserepository;
        private readonly IRepository<WarehouseItem> _warehouseItemrepository;

        public WarehouseItemAppService(IRepository<Warehouse> warehouserepository, IRepository<WarehouseItem> warehouseItemrepository)
        {
            _warehouserepository = warehouserepository;
           _warehouseItemrepository= warehouseItemrepository;
        }

        public async Task<int> Create(WarehouseItemCreateDto input)
        {
            try
            {
                if(input.ParrentId == 0)
                {
                    var query = await _warehouseItemrepository.GetAll().Where(x => x.WarehouseId == input.WarehouseId && input.ParrentId == 0).ToListAsync();

                    string sinhma(string ma)
                    {
                        string s = ma.Substring(0, ma.Length);

                        int i = int.Parse(s);
                        i++;
                        if (i < 10) return "0" + Convert.ToString(i);
                        else
                        if (i >= 10 && i < 100) return Convert.ToString(i);
                        else return Convert.ToString(i);

                    }
                    string ma;

                    var count = query.Count;
                    if (count == 0)
                    {
                        ma = "0000";
                    }
                    else
                    {
                        ma = _warehouseItemrepository.GetAll().Where(x => x.WarehouseId == input.WarehouseId && input.ParrentId == 0).OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                    }

                    input.Code = sinhma(ma.ToString());
                    WarehouseItem newItemId = ObjectMapper.Map<WarehouseItem>(input);
                    var newId = await _warehouseItemrepository.InsertAndGetIdAsync(newItemId);
                    return newId;
                }
                else
                {
                    var query = await _warehouseItemrepository.GetAll().Where(x => x.WarehouseId == input.WarehouseId && x.ParrentId != 0 && input.ParrentId != 0 && input.ParrentId == x.ParrentId).ToListAsync();

                    string sinhma(string ma)
                    {
                        string s = ma.Substring(0, ma.Length);

                        int i = int.Parse(s);
                        i++;
                        if (i < 10) return "0" + Convert.ToString(i);
                        else
                        if (i >= 10 && i < 100) return Convert.ToString(i);
                        else return Convert.ToString(i);

                    }
                    string ma;

                    var count = query.Count;
                    if (count == 0)
                    {
                        ma = "0000";
                    }
                    else
                    {
                        ma = _warehouseItemrepository.GetAll().Where(x => x.WarehouseId == input.WarehouseId && input.ParrentId != 0 && x.ParrentId != 0 && input.ParrentId == x.ParrentId).OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                    }

                    input.Code = sinhma(ma.ToString());
                    WarehouseItem newItemId = ObjectMapper.Map<WarehouseItem>(input);
                    var newId = await _warehouseItemrepository.InsertAndGetIdAsync(newItemId);
                    return newId;
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

        }

        public async Task<PagedResultDto<WarehouseItemListDto>> GetAll(int warehouseid)
        {
            try
            {
                var query = _warehouseItemrepository.GetAll().Where(x => x.ParrentId == 0);
                var queryWare = _warehouserepository.GetAll();
                
                var query2 = (from i in query
                              join q in queryWare on i.WarehouseId equals q.Id
                              where q.Id == warehouseid
                              let count =query.Count()
                              select new WarehouseItemListDto
                              {
                                  Id = i.Id,
                                  Name = i.Name,
                                  WarehouseId = i.WarehouseId,
                                  UnitMax = i.UnitMax,  
                              }).ToList();
                return new PagedResultDto<WarehouseItemListDto>(
                  query2.Count(),
                  query2
                  );
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
                await _warehouseItemrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Update(WarehouseItemListDto input)
        {
            try
            {
                WarehouseItem warehouseItem = await _warehouseItemrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                input.Code = warehouseItem.Code;
                input.Color = warehouseItem.Color;
                input.PositionX = warehouseItem.PositionX;
                input.PositionY = warehouseItem.PositionY;
                input.PositionZ = warehouseItem.PositionZ;
                input.WarehouseLevel = warehouseItem.WarehouseLevel;

                ObjectMapper.Map(input, warehouseItem);
                await _warehouseItemrepository.UpdateAsync(warehouseItem);
                return input.Id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WarehouseItemListDto>> GetAsync(EntityDto itemId)
        {
            var item = _warehouseItemrepository.GetAll().Where(x => x.Id == itemId.Id);
            var newItem = ObjectMapper.Map<List<WarehouseItemListDto>>(item);
            return new PagedResultDto<WarehouseItemListDto>(
                  newItem.Count(),
                  newItem
                  );
        }

        public async Task<PagedResultDto<WarehouseItemListDto>> GetAllItemRoot(int idParent)
        {
            try
            {
                var query = _warehouseItemrepository.GetAll().Where(x => x.Id == idParent && x.ParrentId == 0);
                var queryWare = _warehouseItemrepository.GetAll().Where(x => x.ParrentId != 0);

                var query2 = (from i in query
                              join q in queryWare on i.Id equals q.ParrentId
                              let count = query.Count()
                              select new WarehouseItemListDto
                              {
                                  Id = q.Id,
                                  Name = q.Name,
                                  WarehouseId = i.WarehouseId,
                                  NumberChild = count,
                                  ParentName = i.Name,
                                  UnitMax = i.UnitMax,
                              }).ToList();

                //var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<WarehouseItemListDto>(
                  query2.Count(),
                  query2
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WarehouseItemListDto>> GetAllItemSub(int idParent)
        {
            try
            {
                var query = _warehouseItemrepository.GetAll().Where(x => x.ParrentId != 0 && x.Id == idParent);
                var query1 = _warehouseItemrepository.GetAll().Where(x => x.ParrentId != 0);

                var query2 = (from i in query
                              join z in query1 on i.Id equals z.ParrentId
                              select new WarehouseItemListDto
                              {
                                  Id = z.Id,
                                  Name = z.Name,
                                  ParrentId = z.ParrentId,
                                  WarehouseLevel = z.WarehouseLevel,
                                  UnitMax= z.UnitMax,
                              }).ToList();


                //var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<WarehouseItemListDto>(
                  query2.Count(),
                  query2
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WarehouseItemListDto>> GetAllBin(long warehouseId)
        {
            try
            {
                var queryparrenId = _warehouseItemrepository.GetAll().Select(x => x.ParrentId).ToList();
                var query = _warehouseItemrepository.GetAll().Where(x=>!queryparrenId.ToString().Contains(x.Id.ToString()) && warehouseId == x.WarehouseId).ToList();
                var result = ObjectMapper.Map<List<WarehouseItemListDto>>(query);
                return new PagedResultDto<WarehouseItemListDto>(result.Count(), result);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<string> GetinfoBin(int Id)
        {
            try
            {
                string output = null; 
                var query = _warehouseItemrepository.GetAll().Where(x => x.Id ==  Id).FirstOrDefault().ParrentId;
                var query2 = _warehouseItemrepository.GetAll().Where(x => x.Id ==  Id).FirstOrDefault().Name;
                output = query2;
                long dem = query;
                while(dem != 0)
                {
                  var  query1 = _warehouseItemrepository.GetAll().Where(x => x.Id == dem).FirstOrDefault().Name;

                    var query3 = _warehouseItemrepository.GetAll().Where(x => x.Id == dem).FirstOrDefault().ParrentId;
                    dem = query3;
                    output = query1 + " - " + output;
                }
                return output;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<WarehouseItemListDto> GetById(EntityDto itemId)
        {
            var item = _warehouseItemrepository.Get(itemId.Id);
            WarehouseItemListDto newItem = ObjectMapper.Map<WarehouseItemListDto>(item);
            return newItem;
        }

        public async Task<long> UpdateCount(int idParent)
        {
            try
            {
                var sum = 0;
                var idSet = 0;
                var result = 0;
                var query = _warehouseItemrepository.Get(idParent);
                idSet = ((int)query.ParrentId);
                do
                {
                    sum = 0;
                    var query1 = _warehouseItemrepository.GetAll().Where(x => x.ParrentId == idSet).ToList();
                    WarehouseItem warehouseItem = await _warehouseItemrepository.FirstOrDefaultAsync(x => x.Id == idSet);
                    //idSet = ((int)warehouseItem.ParrentId);

                    for (int i = 0; i < query1.Count; i++)
                    {
                        sum += query1[i].UnitMax;
                    }

                    if (sum > warehouseItem.UnitMax)
                    {
                        warehouseItem.UnitMax = sum;
                        //ObjectMapper.Map(input, warehouseItem);
                        await _warehouseItemrepository.UpdateAsync(warehouseItem);
                        result = warehouseItem.Id;
                        idSet = ((int)warehouseItem.ParrentId);
                    }
                    else
                    {
                        warehouseItem.UnitMax = warehouseItem.UnitMax;
                        //ObjectMapper.Map(input, warehouseItem);
                        await _warehouseItemrepository.UpdateAsync(warehouseItem);
                        result = warehouseItem.Id;
                        idSet = ((int)warehouseItem.ParrentId);
                    }

                } while (idSet != 0);
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WarehouseItemListDto>> GetAllItemOutLayout(int id)
        {
            try
            {
                var query = _warehouseItemrepository.GetAll().Where(x => x.ParrentId == 0 && x.PositionX == null && x.PositionY == null);
                var queryWare = _warehouserepository.GetAll();

                var query2 = (from i in query
                              join q in queryWare on i.WarehouseId equals q.Id
                              where q.Id == id
                              let count = query.Count()
                              select new WarehouseItemListDto
                              {
                                  Id = i.Id,
                                  Name = i.Name,
                                  WarehouseId = i.WarehouseId,
                                  Lenght = i.Lenght,
                                  Height = i.Height,
                                  Width = i.Width,
                                  PositionX = i.PositionX,
                                  PositionY = i.PositionY,
                                  PositionZ = i.PositionZ,
                                  Color = i.Color,
                              }).ToList();

                //var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<WarehouseItemListDto>(
                  query2.Count(),
                  query2
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WarehouseItemListDto>> GetAllItemInLayout(int id)
        {
            try
            {
                var query = _warehouseItemrepository.GetAll().Where(x => x.ParrentId == 0 && x.PositionX != null && x.PositionY != null);
                var queryWare = _warehouserepository.GetAll();

                var query2 = (from i in query
                              join q in queryWare on i.WarehouseId equals q.Id
                              where q.Id == id
                              let count = query.Count()
                              select new WarehouseItemListDto
                              {
                                  Id = i.Id,
                                  Name = i.Name,
                                  WarehouseId = i.WarehouseId,
                                  Lenght = i.Lenght,
                                  Height = i.Height,
                                  Width = i.Width,
                                  PositionX = i.PositionX,
                                  PositionY = i.PositionY,
                                  PositionZ = i.PositionZ,
                                  Color = i.Color,
                              }).ToList();

                //var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<WarehouseItemListDto>(
                  query2.Count(),
                  query2
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> SaveLayOut(WarehouseItemListDto input)
        {
            try
            {
                WarehouseItem warehouseItem = await _warehouseItemrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if(warehouseItem.Color != null)
                {
                    input.Code = warehouseItem.Code;
                    input.Name = warehouseItem.Name;
                    input.WarehouseId = warehouseItem.WarehouseId;
                    input.Height = warehouseItem.Height;
                    input.Width = warehouseItem.Width;
                    input.Lenght = warehouseItem.Lenght;
                    input.ParrentId = warehouseItem.ParrentId;
                    input.Description = warehouseItem.Description;
                    input.TypeCode = warehouseItem.TypeCode;
                    input.MaxKgVolume = warehouseItem.MaxKgVolume;
                    input.MaxM3Volume = warehouseItem.MaxM3Volume;
                    input.DeleteFlag = warehouseItem.DeleteFlag;
                    input.UnitId = warehouseItem.UnitId;
                    input.UnitMax = warehouseItem.UnitMax;
                    input.CategoryCode = warehouseItem.CategoryCode;
                    input.WarehouseLevel = warehouseItem.WarehouseLevel;
                    input.Color = warehouseItem.Color;
                }
                else
                {
                    input.Code = warehouseItem.Code;
                    input.Name = warehouseItem.Name;
                    input.WarehouseId = warehouseItem.WarehouseId;
                    input.Height = warehouseItem.Height;
                    input.Width = warehouseItem.Width;
                    input.Lenght = warehouseItem.Lenght;
                    input.ParrentId = warehouseItem.ParrentId;
                    input.Description = warehouseItem.Description;
                    input.TypeCode = warehouseItem.TypeCode;
                    input.MaxKgVolume = warehouseItem.MaxKgVolume;
                    input.MaxM3Volume = warehouseItem.MaxM3Volume;
                    input.DeleteFlag = warehouseItem.DeleteFlag;
                    input.UnitId = warehouseItem.UnitId;
                    input.UnitMax = warehouseItem.UnitMax;
                    input.CategoryCode = warehouseItem.CategoryCode;
                    input.WarehouseLevel = warehouseItem.WarehouseLevel;
                }

                ObjectMapper.Map(input, warehouseItem);
                await _warehouseItemrepository.UpdateAsync(warehouseItem);
                return input.Id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<string> GetCodeLocationBin(int Id)
        {
            try
            {
                string output = null;
                var query = _warehouseItemrepository.GetAll().Where(x => x.Id == Id).FirstOrDefault().ParrentId;
                var query2 = _warehouseItemrepository.GetAll().Where(x => x.Id == Id).FirstOrDefault().Code;
                var query4 = _warehouseItemrepository.GetAll().Where(x => x.Id == Id).FirstOrDefault().CategoryCode;
                output = query2;
                long dem = query;
                while (dem != 0)
                {
                    var query1 = _warehouseItemrepository.GetAll().Where(x => x.Id == dem).FirstOrDefault().Code;

                    var query3 = _warehouseItemrepository.GetAll().Where(x => x.Id == dem).FirstOrDefault().ParrentId;
                    dem = query3;
                    output = query1 + output;
                }

                var result = query4 + output;
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
