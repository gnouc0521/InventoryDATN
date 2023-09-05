using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.ScheduleWorks.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ScheduleWorks
{
    [AbpAuthorize]
    public class ScheduleWorkAppService : ApplicationService, IScheduleWorkAppService
    {
        private readonly IRepository<ScheduleWork> _scheduleWorkrepository;

        public ScheduleWorkAppService(IRepository<ScheduleWork> scheduleWorkrepository)
        {
            _scheduleWorkrepository = scheduleWorkrepository;
        }

        public async Task<int> Create(ScheduleWorkCreateDto input)
        {
            try
            {
                ScheduleWork newItemId = ObjectMapper.Map<ScheduleWork>(input);
                var newId = await _scheduleWorkrepository.InsertAndGetIdAsync(newItemId);
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
                await _scheduleWorkrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ScheduleWorkListDto>> GetAll(GetScheduleWorkInput input)
        {
            try
            {
                var query = _scheduleWorkrepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Title.ToLower().Contains(input.SearchTerm.ToLower()) || u.Address.ToLower().Contains(input.SearchTerm.ToLower()))
                      .WhereIf(input.IdLevel != null, u => ((int)u.Color).Equals(input.IdLevel))
                      .OrderBy(x => x.Id);
                var worksCount = query.Count();


                var WorkListDto = ObjectMapper.Map<List<ScheduleWorkListDto>>(query);
                return new PagedResultDto<ScheduleWorkListDto>(
                  worksCount,
                  WorkListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ScheduleWorkListDto> GetAsync(EntityDto itemId)
        {
            var item = _scheduleWorkrepository.Get(itemId.Id);
            ScheduleWorkListDto newItem = ObjectMapper.Map<ScheduleWorkListDto>(item);
            return newItem;
        }

        public async Task<int> Update(ScheduleWorkEditDto input)
        {
            ScheduleWork dayoff = await _scheduleWorkrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, dayoff);
            await _scheduleWorkrepository.UpdateAsync(dayoff);
            return input.Id;
        }
    }
}
