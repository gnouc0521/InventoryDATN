using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Units;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseTypes.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseTypes
{
    public class WarehouseTypesService : netcoreServiceBase, IWarehouseTypesService
    {
        private readonly IRepository<WarehouseType> _warehouseTyperepository;

        public WarehouseTypesService(IRepository<WarehouseType> warehouseTyperepository)
        {
            _warehouseTyperepository = warehouseTyperepository;
        }

        public async Task<int> Create(WarehouseTypeListDto input)
        {
            try
            {
                string sinhma(string ma)
                {
                    string s = ma.Substring(4, ma.Length - 4);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "LKH-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "LKH-" + "0" + Convert.ToString(i);
                    else return "LKH-" + Convert.ToString(i);

                }
                string ma;
                var query = await _warehouseTyperepository.GetAll().ToListAsync();
                var count = query.Count;
                if (count == 0)

                {
                    ma = "00000000";
                }
                else
                {
                    ma = _warehouseTyperepository.GetAll().OrderByDescending(x => x.Code).Select(x => x.Code).ToList().First();
                }

                input.Code = sinhma(ma.ToString()); 
                WarehouseType newItemId = ObjectMapper.Map<WarehouseType>(input);
                var newId = await _warehouseTyperepository.InsertAndGetIdAsync(newItemId);
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
                await _warehouseTyperepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WarehouseTypeListDto>> GetAll(WarehouseTypeSearch input)
        {
            try
            {
                var query = _warehouseTyperepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Name.ToLower().Contains(input.SearchTerm.ToLower())).ToList();
                var warehouseTypeCount = query.Count();
                var WarehouseTypeListDto = ObjectMapper.Map<List<WarehouseTypeListDto>>(query);
                return new PagedResultDto<WarehouseTypeListDto>(
                  warehouseTypeCount,
                  WarehouseTypeListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<WarehouseTypeListDto> GetAsync(EntityDto itemId)
        {
            var item = _warehouseTyperepository.Get(itemId.Id);
            WarehouseTypeListDto newItem = ObjectMapper.Map<WarehouseTypeListDto>(item);
            return newItem;
        }

        public async Task<int> Update(WarehouseTypeListDto input)
        {
            
            WarehouseType warehouseType = await _warehouseTyperepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.Code = warehouseType.Code;
            ObjectMapper.Map(input, warehouseType);
            await _warehouseTyperepository.UpdateAsync(warehouseType);
            return input.Id;
        }

        //hà thêm
        public async Task<List<WarehouseTypeListDto>> GetAllWarehouseType()
        {
            try
            {
                var query = _warehouseTyperepository
                      .GetAll()
                      .ToList();
                var WarehouseTypeListDto = ObjectMapper.Map<List<WarehouseTypeListDto>>(query);
                return WarehouseTypeListDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

    }
}

