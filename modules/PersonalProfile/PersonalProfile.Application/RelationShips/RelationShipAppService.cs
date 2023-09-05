using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.RelationShips.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.RelationShips
{
    public class RelationShipAppService : ApplicationService, IRelationShipAppService
    {
        private readonly IRepository<RelationShip> _relationShipRepository;

        public RelationShipAppService(IRepository<RelationShip> relationShipRepository)
        {
            _relationShipRepository = relationShipRepository;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Create(RelationShipDto relationShipDto)
        {
            try
            {
                if(relationShipDto.Type == RelationType.Self)
                {
                    if( !relationShipDto.YearBirth.HasValue || string.IsNullOrWhiteSpace(relationShipDto.Info))
                    {
                        throw new Exception("Hãy nhập đầy đủ thông tin");
                    }
                }
                var entity = ObjectMapper.Map<RelationShip>(relationShipDto);
                int id = await _relationShipRepository.InsertAndGetIdAsync(entity);
                return id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Update(RelationShipDto relationShipDto)
        {
            try
            {
                var id = relationShipDto.Id;
                var personId = relationShipDto.PersonId;
                var check = await _relationShipRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Mối quan hệ không đúng!");
                }
                ObjectMapper.Map(relationShipDto, check);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<RelationShipDto>> GetAll(GetAllFilter filter)
        {
            try
            {
                int personId = filter.PersonId ?? default(int);
                RelationType type = filter.Type.GetValueOrDefault();
                var result = await _relationShipRepository.GetAllListAsync();
                result = result.WhereIf(filter.PersonId.HasValue, x=>x.PersonId == personId).ToList();
                result = result.WhereIf(filter.Type.HasValue && Enum.IsDefined(typeof(RelationType), type), x=>x.Type == type).ToList();
                List<RelationShipDto> dtos = new List<RelationShipDto>();
                ObjectMapper.Map(result, dtos);
                return dtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<RelationShipDto> GetById(int id)
        {
            try
            {
                var entity = await _relationShipRepository.FirstOrDefaultAsync(x=>x.Id == id);
                if(entity == null)
                {
                    throw new Exception("Thông tin quan hệ gia đình không đúng!");
                }
                var result = ObjectMapper.Map<RelationShipDto>(entity);
                return result;
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeleteRelationShipDto deleteRelationShipDto)
        {
            try
            {
                int id = deleteRelationShipDto.Id;
                int personId = deleteRelationShipDto.PersonId;
                var check = await _relationShipRepository.FirstOrDefaultAsync(x =>x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin mối quan hệ không đúng!");
                }
                await _relationShipRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
