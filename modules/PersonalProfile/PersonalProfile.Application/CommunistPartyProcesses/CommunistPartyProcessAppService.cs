using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.CommunistPartyProcesses.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.Storage.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;

namespace bbk.netcore.mdl.PersonalProfile.Application.CommunistPartyProcesses
{
    public class CommunistPartyProcessAppService : ApplicationService, ICommunistPartyProcessAppService
    {
        private readonly IRepository<CommunistPartyProcess> _communistPartyProcessRepository;

        public CommunistPartyProcessAppService(IRepository<CommunistPartyProcess> communistPartyProcessRepository)
        {
            _communistPartyProcessRepository = communistPartyProcessRepository;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Create(CommunistPartyProcessDto dto)
        {
            try
            {
                var checkExist = await _communistPartyProcessRepository.FirstOrDefaultAsync(x => x.PersonId == dto.PersonId && x.Year == dto.Year);
                if (checkExist != null)
                {
                    throw new Exception($"Năm {dto.Year} đã có trong danh sách");
                }
                var entity = ObjectMapper.Map<CommunistPartyProcess>(dto);
                int id = await _communistPartyProcessRepository.InsertAndGetIdAsync(entity);
                return id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Update(CommunistPartyProcessDto dto)
        {
            try
            {
                var id = dto.Id;
                var personId = dto.PersonId;
                var check = await _communistPartyProcessRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin công tác Đảng không đúng!");
                }
                var checkExist = await _communistPartyProcessRepository.FirstOrDefaultAsync(x => x.PersonId == dto.PersonId && x.Year == dto.Year && x.Id != id);
                if (checkExist != null)
                {
                    throw new Exception($"Năm {dto.Year} đã có trong danh sách");
                }
                ObjectMapper.Map(dto, check);
                return check.Id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetCommunistPartyProcessDto>> GetAll(GetAllFilter filter)
        {
            try
            {
                int personId = filter.PersonId ?? default(int);
                var result = await _communistPartyProcessRepository.GetAllListAsync();
                result = result.WhereIf(filter.PersonId.HasValue, x => x.PersonId == personId).OrderBy(x => x.Year).ToList();
                List<GetCommunistPartyProcessDto> dtos = new List<GetCommunistPartyProcessDto>();
                foreach (var r in result)
                {
                    GetCommunistPartyProcessDto dto = new GetCommunistPartyProcessDto();
                    dto.Id = r.Id;
                    dto.Year = r.Year;
                    dto.PartyMemberBackground = EnumExtensions.GetDisplayName(r.PartyMemberBackground);
                    dto.EvaluatePartyMember = EnumExtensions.GetDisplayName(r.EvaluatePartyMember);
                    dtos.Add(dto);
                }
                return dtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<CommunistPartyProcessDto> GetById(int id)
        {
            try
            {
                var entity = await _communistPartyProcessRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                {
                    throw new Exception("Thông tin công tác Đảng không đúng!");
                }
                var result = ObjectMapper.Map<CommunistPartyProcessDto>(entity);
                return result;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeleteCommunistPartyProcessDto dto)
        {
            try
            {
                int id = dto.Id;
                int personId = dto.PersonId;
                var check = await _communistPartyProcessRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin công tác Đảng không đúng!");
                }
                await _communistPartyProcessRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
