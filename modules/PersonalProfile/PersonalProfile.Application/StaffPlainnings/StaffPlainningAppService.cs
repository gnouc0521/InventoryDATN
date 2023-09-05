using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.Documents;
using bbk.netcore.mdl.PersonalProfile.Application.StaffPlainnings.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.StaffPlainnings
{
    public class StaffPlainningAppService : ApplicationService, IStaffPlainningAppService
    {
        private readonly IRepository<StaffPlainning> _staffPlainningRepository;
        private readonly IDocumentAppService _documentAppService;
        public StaffPlainningAppService(IRepository<StaffPlainning> staffPlainningRepository, IDocumentAppService documentAppService)
        {
            _staffPlainningRepository = staffPlainningRepository;
            _documentAppService = documentAppService;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task<int> Create(StaffPlainningDto staffPlainningDto)
        {
            try
            {
                if(staffPlainningDto.FromDate > staffPlainningDto.ToDate)
                {
                    throw new Exception("Ngày bắt đầu muộn hơn ngày kết thúc, vui lòng nhập lại!");
                }
                var document = await _documentAppService.GetById(staffPlainningDto.DocumentId.Value);
                if (document == null)
                {
                    throw new Exception("Quyết định nhập vào không đúng!");
                }
                var entity = ObjectMapper.Map<StaffPlainning>(staffPlainningDto);

                int id = await _staffPlainningRepository.InsertAndGetIdAsync(entity);
                return id;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Update(StaffPlainningDto staffPlainningDto)
        {
            try
            {
                if (staffPlainningDto.FromDate > staffPlainningDto.ToDate)
                {
                    throw new Exception("Ngày bắt đầu muộn hơn ngày kết thúc, vui lòng nhập lại!");
                }
                var id = staffPlainningDto.Id;
                var personId = staffPlainningDto.PersonId;
                var check = await _staffPlainningRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin quy hoạch không đúng!");
                }
                var document = await _documentAppService.GetById(staffPlainningDto.DocumentId.Value);
                if (document == null)
                {
                    throw new Exception("Quyết định nhập vào không đúng!");
                }
                ObjectMapper.Map(staffPlainningDto, check);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetStaffPlainningDto>> GetAll(GetAllFilter filter)
        {
            try
            {
                int personId = filter.PersonId ?? default(int);
                var entities = await _staffPlainningRepository.GetAllListAsync();
                entities = entities.WhereIf(filter.PersonId.HasValue, x=>x.PersonId == personId).ToList();
                List<GetStaffPlainningDto> dtos = new List<GetStaffPlainningDto>();
                foreach(var e in entities)
                {
                    var document = await _documentAppService.GetById(e.DocumentId);
                    GetStaffPlainningDto dto = new GetStaffPlainningDto(e,document);
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
        public async Task<StaffPlainningDto> GetById(int id)
        {
            try
            {
                var entity = await _staffPlainningRepository.FirstOrDefaultAsync(x=>x.Id == id);
                if(entity == null)
                {
                    throw new Exception("Thông tin quy hoạch không đúng!");
                }
                var doc = await _documentAppService.GetById(entity.DocumentId);
                var result = ObjectMapper.Map<StaffPlainningDto>(entity);
                result.DecisionNumber = doc.DecisionNumber;
                result.IssuedDate = doc.IssuedDate;
                return result;
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeleteStaffPlainningDto deleteStaffPlainningDto)
        {
            try
            {
                int id = deleteStaffPlainningDto.Id;
                int personId = deleteStaffPlainningDto.PersonId;
                var check = await _staffPlainningRepository.FirstOrDefaultAsync(x =>x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Thông tin quy hoạch không đúng!");
                }
                await _staffPlainningRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
