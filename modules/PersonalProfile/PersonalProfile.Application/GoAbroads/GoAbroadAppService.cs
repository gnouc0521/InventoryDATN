using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.Documents;
using bbk.netcore.mdl.PersonalProfile.Application.GoAbroads;
using bbk.netcore.mdl.PersonalProfile.Application.GoAbroads.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.GoAbroads
{
    public class GoAbroadAppService: ApplicationService, IGoAbroadAppService
    {
        private readonly IRepository<GoAbroad> _goAbroadRepository;
        private readonly IDocumentAppService _documentAppService;
        public GoAbroadAppService(IRepository<GoAbroad> goAbroadRepository, IDocumentAppService documentAppService)
        {
            _goAbroadRepository = goAbroadRepository;
            _documentAppService = documentAppService;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Create(GoAbroadDto goAbroadDto)
        {
            try
            {
                if (goAbroadDto.FromDate > goAbroadDto.ToDate)
                {
                    throw new Exception("Ngày bắt đầu muộn hơn ngày kết thúc, vui lòng nhập lại!");
                }
                var document = await _documentAppService.GetById(goAbroadDto.DocumentId);
                if (document == null)
                {
                    throw new Exception("Quyết định nhập vào không đúng!");
                }
                var entity = ObjectMapper.Map<GoAbroad>(goAbroadDto);

                int id = await _goAbroadRepository.InsertAndGetIdAsync(entity);
                return id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Update(GoAbroadDto goAbroadDto)
        {
            try
            {
                if (goAbroadDto.FromDate > goAbroadDto.ToDate)
                {
                    throw new Exception("Ngày bắt đầu muộn hơn ngày kết thúc, vui lòng nhập lại!");
                }
                var id = goAbroadDto.Id;
                var personId = goAbroadDto.PersonId;
                var check = await _goAbroadRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin đi nước ngoài không đúng!");
                }
                var document = await _documentAppService.GetById(goAbroadDto.DocumentId);
                if (document == null)
                {
                    throw new Exception("Quyết định nhập vào không đúng!");
                }
                ObjectMapper.Map(goAbroadDto, check);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetGoAbroadDto>> GetAll(GetAllFilter filter)
        {
            try
            {
                int personId = filter.PersonId ?? default(int);
                var entities = await _goAbroadRepository.GetAllListAsync();
                entities = entities.WhereIf(filter.PersonId.HasValue, x => x.PersonId == personId).ToList();
                List<GetGoAbroadDto> dtos = new List<GetGoAbroadDto>();
                foreach (var e in entities)
                {
                    var document = await _documentAppService.GetById(e.DocumentId);
                    GetGoAbroadDto dto = new GetGoAbroadDto(e, document);
                    dtos.Add(dto);
                }
                dtos = dtos.OrderBy(x => x.IssuedDate).ToList();
                return dtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<GoAbroadDto> GetById(int id)
        {
            try
            {
                var entity = await _goAbroadRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                {
                    throw new Exception("Thông tin đi nước ngoài không đúng!");
                }
                var doc = await _documentAppService.GetById(entity.DocumentId);
                var result = ObjectMapper.Map<GoAbroadDto>(entity);
                result.DecisionNumber = doc.DecisionNumber;
                result.IssuedDate = doc.IssuedDate;
                return result;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeleteGoAbroadDto deleteGoAbroadDto)
        {
            try
            {
                int id = deleteGoAbroadDto.Id;
                int personId = deleteGoAbroadDto.PersonId;
                var check = await _goAbroadRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin không đúng!");
                }
                await _goAbroadRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
