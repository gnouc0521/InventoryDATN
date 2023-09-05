using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.Documents;
using bbk.netcore.mdl.PersonalProfile.Application.RecruitmentInfomations.Dtos;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.RecruitmentInfomations
{
    public class RecruitmentInfomationAppService : ApplicationService, IRecruitmentInfomationAppService
    {
        private readonly IRepository<RecruitmentInfomation> _recruitmentInfomation;
        private readonly IDocumentAppService _documentAppService;

        public RecruitmentInfomationAppService(IRepository<RecruitmentInfomation> recruitmentInfomation, IDocumentAppService documentAppService)
        {
            _recruitmentInfomation = recruitmentInfomation;
            _documentAppService = documentAppService;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<ListResultDto<RecruitmentInfomationDto>> GetAll(int staffId)
        {
            var RecruitmentInfomation = await _recruitmentInfomation
                .GetAll()
                .Where(u => u.ProfileStaffId == staffId)
                .OrderByDescending(t => t.Id)
                .ToListAsync();

            return new ListResultDto<RecruitmentInfomationDto>(
                ObjectMapper.Map<List<RecruitmentInfomationDto>>(RecruitmentInfomation)
            );
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Create(RecruitmentInfomationDto input)
        {
            try
            {
              
                var document = await _documentAppService.GetById(input.DocumentId);
                if (document == null)
                {
                    throw new Exception("Quyết định nhập vào không đúng!");
                }
                var entity = ObjectMapper.Map<RecruitmentInfomation>(input);
                await _recruitmentInfomation.InsertAndGetIdAsync(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(int Id)
        {
            try
            {
                var recruitmentInfomation = _recruitmentInfomation.Get(Id);
                if (recruitmentInfomation == null)
                {
                    throw new Exception("Id không tồn tại");
                }
                await _recruitmentInfomation.DeleteAsync(recruitmentInfomation);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<RecruitmentInfomationDto> GetById(int Id)
        {
            try
            {
                var entity = await _recruitmentInfomation.FirstOrDefaultAsync(Id);
                if (entity == null)
                {
                    throw new Exception("Thông tin tuyển dụng không đúng!");
                }
                RecruitmentInfomationDto recruitmentInfomationDto = ObjectMapper.Map<RecruitmentInfomationDto>(entity);
                return recruitmentInfomationDto;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Update(RecruitmentInfomationDto recruitmentInfomationDto)
        {
            try
            {
                int id = recruitmentInfomationDto.Id;              
                var checkRecruitmentInfomation = await _recruitmentInfomation.FirstOrDefaultAsync(x => x.Id == id);
                if (checkRecruitmentInfomation == null)
                {
                    throw new Exception("Chi tiết tuyển dụng không đúng!");
                }
                ObjectMapper.Map(recruitmentInfomationDto, checkRecruitmentInfomation);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<ListResultDto<RecruitmentInfomationDto>> GetAllList()
        {
            var RecruitmentInfomation = await _recruitmentInfomation
                    .GetAll()
                    .ToListAsync();

            return new ListResultDto<RecruitmentInfomationDto>(
                ObjectMapper.Map<List<RecruitmentInfomationDto>>(RecruitmentInfomation)
            );
        }
    }
}
