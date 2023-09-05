using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.Commendations.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Documents;
using bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.DocumentEnum;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Commendations
{
    public class CommendationAppService : ApplicationService, ICommendationAppService
    {
        private readonly IRepository<Commendation> _commendationRepository;
        private readonly IDocumentAppService _documentAppService;

        public CommendationAppService(IRepository<Commendation> commendationRepository, IDocumentAppService documentAppService)
        {
            _commendationRepository = commendationRepository;
            _documentAppService = documentAppService;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Create(CommendationDto commendationDto)
        {
            try
            {
                var document = await _documentAppService.GetById(commendationDto.DocumentId.Value);
                if (document == null)
                {
                    throw new Exception("Thông tin quyết định không đúng!");
                }
                var entity = ObjectMapper.Map<Commendation>(commendationDto);
                int id = await _commendationRepository.InsertAndGetIdAsync(entity);
                return id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Update(CommendationDto commendationDto)
        {
            try
            {
                var id = commendationDto.Id;
                var personId = commendationDto.PersonId;
                var check = await _commendationRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin chi tiết khen thưởng không đúng, vui lòng nhập lại!");
                }
                var document = await _documentAppService.GetById(commendationDto.DocumentId.Value);
                if (document == null)
                {
                    throw new Exception("Thông tin quyết định không đúng!");
                }
                commendationDto.DecisionNumber = document.DecisionNumber;
                ObjectMapper.Map(commendationDto, check);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetCommendationDto>> GetAll(GetAllFilter filter)
        {
            try
            {
                int personId = filter.PersonId ?? default(int);
                CategoryType type = filter.Type.GetValueOrDefault();
                var result = _commendationRepository.GetAll().Include(x=>x.CommendationTitle).ToList();
                result = result.WhereIf(filter.PersonId.HasValue, x => x.PersonId == personId).ToList();
                result = result.WhereIf(filter.Type.HasValue && Enum.IsDefined(typeof(CategoryType), type), x => x.CommendationTitle.CategoryType == type).ToList();
                List<GetCommendationDto> dtos = new List<GetCommendationDto>();
                foreach(var r in result)
                {
                    var doc = await _documentAppService.GetById(r.DocumentId);
                    GetCommendationDto dto = new GetCommendationDto(r,doc);
                    dtos.Add(dto);
                }
                dtos = dtos.OrderBy(x => x.CommendationYear).ToList();
                return dtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<CommendationDto> GetById(int id)
        {
            try
            {
                var entity = await _commendationRepository.FirstOrDefaultAsync(x=>x.Id == id);
                if(entity == null)
                {
                    throw new Exception("Có lỗi xảy ra!");
                }
                var doc = await _documentAppService.GetById(entity.DocumentId);
                var result = ObjectMapper.Map<CommendationDto>(entity);
                result.DecisionNumber = doc.DecisionNumber;
                return result;
            }
            catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeleteCommendationDto deleteCommendationDto)
        {
            try
            {
                int id = deleteCommendationDto.Id;
                int personId = deleteCommendationDto.PersonId;
                var check = await _commendationRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin chi tiết khen thưởng không đúng, vui lòng nhập lại!");
                }
                await _commendationRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetCommendationDto>> GetAllList()
        {
            try
            {
                var result = _commendationRepository.GetAll().Include(x => x.CommendationTitle).ToList();
                result = result.Where(x => x.CommendationTitle.CategoryType == CategoryType.EmulationTitle || x.CommendationTitle.CategoryType == CategoryType.CommendationForm).ToList();
                List<GetCommendationDto> dtos = new List<GetCommendationDto>();
                foreach (var r in result)
                {
                    var doc = await _documentAppService.GetById(r.DocumentId);
                    GetCommendationDto dto = new GetCommendationDto(r, doc);
                    dtos.Add(dto);
                }
                dtos = dtos.OrderBy(x => x.CommendationYear).ToList();
                return dtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
