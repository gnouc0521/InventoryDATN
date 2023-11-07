using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Ruleses
{
    public class RulesService : ApplicationService, IRulesService
    {
        private readonly IRepository<Rules> _rulesrepository;

        public RulesService(IRepository<Rules> rulesrepository)
        {
            _rulesrepository = rulesrepository;
        }
        public async Task<int> Create(RulesListDto input)
        {
            try
            {
                Rules newItemId = ObjectMapper.Map<Rules>(input);
                if (input.ItemKey.ToLower().Contains("NGH"))
                {
                    newItemId.Order = 1;
                }else if (input.ItemKey.ToLower().Contains("NHH"))
                {
                    newItemId.Order = 2;
                }else
                {
                    newItemId.Order = 3;
                }
                var newId = await _rulesrepository.InsertAndGetIdAsync(newItemId);
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
                await _rulesrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<RulesListDto>> GetAll(RulesSearch input)
        {
            try
            {
                var query = _rulesrepository
                      .GetAll()

                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => input.SearchTerm.ToLower().Contains(u.ItemKey.ToLower())).ToList();
                var rulesCount = query.Count();
                var rulesListDto = ObjectMapper.Map<List<RulesListDto>>(query);
                return new PagedResultDto<RulesListDto>(
                  rulesCount,
                  rulesListDto
                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<RulesListDto>> GetAllKind()
        {
            try
            {
                var query = _rulesrepository
                      .GetAll()
                      .Where( u => "CHL".Contains(u.ItemKey) || u.ItemKey.Contains("CHL")).ToList();
                var rulesCount = query.Count();
                var rulesListDto = ObjectMapper.Map<List<RulesListDto>>(query);
                return rulesListDto;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<RulesListDto>> GetAllCategory()
        {
            try
            {
                var query = _rulesrepository
                      .GetAll()
                      .Where(u => "NGH".Contains(u.ItemKey) || u.ItemKey.Contains("NGH")).ToList();
                var rulesCount = query.Count();
                var rulesListDto = ObjectMapper.Map<List<RulesListDto>>(query);
                return rulesListDto;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<RulesListDto>> GetAllGroup()
        {
            try
            {
                var query = _rulesrepository
                      .GetAll()
                      .Where(u => "NHH".Contains(u.ItemKey) || u.ItemKey.Contains("NHH")).ToList();
                var rulesCount = query.Count();
                var rulesListDto = ObjectMapper.Map<List<RulesListDto>>(query);
                return rulesListDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<RulesListDto> GetAsync(EntityDto itemId)
        {
            var item = _rulesrepository.Get(itemId.Id);
      if(item == null)
      {
        return null;
      }
            RulesListDto newItem = ObjectMapper.Map<RulesListDto>(item);
            return newItem;
        }

        public async Task<int> Update(RulesListDto input)
        {
            Rules rules = await _rulesrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            input.Order = rules.Order;
            ObjectMapper.Map(input, rules);
            await _rulesrepository.UpdateAsync(rules);
            return input.Id;
        }
    }
}
