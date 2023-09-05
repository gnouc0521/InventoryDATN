using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Application.ScheduleWorks.Dto;
using bbk.netcore.mdl.OMS.Application.Suppliers.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Units
{
    public class UnitService : netcoreServiceBase, IUnitService
    {
        private readonly IRepository<Unit> _unitrepository;

        public UnitService(IRepository<Unit> unitrepository)
        {
            _unitrepository = unitrepository;
        }
        public async Task<int> Create(UnitListDto input)
        {
            try
            {
                Unit newItemId = ObjectMapper.Map<Unit>(input);
                var newId = await _unitrepository.InsertAndGetIdAsync(newItemId);
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
                await _unitrepository.DeleteAsync(id);
                var dto = _unitrepository.GetAll().Where(x => x.ParrentId == id).ToList();
                foreach (var item in dto)
                {
                    await _unitrepository.DeleteAsync(item.Id);
                }
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<UnitListDto>> GetAll(UnitSearch input)
        {
            try
            {
                var query = _unitrepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Name.ToLower().Contains(input.SearchTerm.ToLower()))
                      .WhereIf(input.ParrentId.HasValue, u => u.ParrentId == input.ParrentId).ToList();
                var unitCount = query.Count();
                var unitListDto = ObjectMapper.Map<List<UnitListDto>>(query);
                return new PagedResultDto<UnitListDto>(
                  unitCount,
                  unitListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<UnitListDto> GetAsync(EntityDto itemId)
        {
            var item = _unitrepository.Get(itemId.Id);
            UnitListDto newItem = ObjectMapper.Map<UnitListDto>(item);
            return newItem;
        }

        public async Task<List<UnitListDto>> GetUnitListDtos()
        {
            try
            {
                var query = _unitrepository
                    .GetAll();
                return ObjectMapper.Map<List<UnitListDto>>(query);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }


        }

        public async Task<List<UnitListDto>> GetUnitList()
        {
            try
            {
                var query = _unitrepository.GetAll();
                var output = (from q in query
                              select new UnitListDto
                              {
                                  Id = q.Id,
                                  Name = q.Name,
                              }).ToList();

                return output;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }


        }



        public async Task<int> Update(UnitListDto input)
        {
            Unit unit = await _unitrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, unit);
            await _unitrepository.UpdateAsync(unit);
            return input.Id;
        }


        public async Task<UnitListDto> GetUnitByText(string NameText)
        {
            var item = _unitrepository.GetAll().Where(x => x.Name.Contains(NameText.Trim())).ToList();
            var newItem = ObjectMapper.Map<UnitListDto>(item[0]);
            return newItem;
        }
    }
}
