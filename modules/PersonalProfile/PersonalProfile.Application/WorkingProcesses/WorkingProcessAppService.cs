using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Application.Categories;
using bbk.netcore.mdl.PersonalProfile.Application.Documents;
using bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using bbk.netcore.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses
{
    public class WorkingProcessAppService : ApplicationService, IWorkingProcessAppService
    {
        private readonly IRepository<WorkingProcess> _workingProcessRepository;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IDocumentAppService _documentAppService;
        private readonly IOrganizationUnitAppService _organizationUnitAppService;
        public WorkingProcessAppService(IRepository<WorkingProcess> workingProcessRepository, ICategoryAppService categoryAppService, IDocumentAppService documentAppService, IOrganizationUnitAppService organizationUnitAppService)
        {
            _categoryAppService = categoryAppService;
            _workingProcessRepository = workingProcessRepository;
            _documentAppService = documentAppService;
            _organizationUnitAppService = organizationUnitAppService;

        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Create(WorkingProcessDto workingProcessDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(workingProcessDto.OtherOrg))
                {
                    throw new Exception("Không được để trống tên đơn vị!");
                }
                if(workingProcessDto.ToDate.HasValue && workingProcessDto.FromDate > workingProcessDto.ToDate.Value)
                {
                    throw new Exception("Ngày hết hiệu lực đang sớm hơn ngày bắt đầu hiệu lực. Vui lòng nhập lại!");
                }
                if(!workingProcessDto.TypeOfChangeId.HasValue)
                {
                    throw new Exception("Không được để trống hình thức!");
                }

                if (workingProcessDto.TypeOfChangeId.HasValue)
                {
                    var typeOfChange = await _categoryAppService.GetCategorybyId(workingProcessDto.TypeOfChangeId.Value);
                    workingProcessDto.TypeOfChangeString = typeOfChange.Title;
                }
                var entity = ObjectMapper.Map<WorkingProcess>(workingProcessDto);
                int id = await _workingProcessRepository.InsertAndGetIdAsync(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Edit)]
        public async Task Update(WorkingProcessDto workingProcessDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(workingProcessDto.OtherOrg))
                {
                    throw new Exception("Không được để trống tên đơn vị!");
                }
                if (workingProcessDto.ToDate.HasValue && workingProcessDto.FromDate > workingProcessDto.ToDate.Value)
                {
                    throw new Exception("Ngày hết hiệu lực đang sớm hơn ngày bắt đầu hiệu lực. Vui lòng nhập lại!");
                }
                if (!workingProcessDto.TypeOfChangeId.HasValue)
                {
                    throw new Exception("Không được để trống hình thức!");
                }
                int id = workingProcessDto.Id;
                int personId = workingProcessDto.PersonId;
                var check = await _workingProcessRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Không tìm thấy chi tiết quá trình công tác!");
                }

                if (workingProcessDto.TypeOfChangeId.HasValue)
                {
                    var typeOfChange = await _categoryAppService.GetCategorybyId(workingProcessDto.TypeOfChangeId.Value);
                    workingProcessDto.TypeOfChangeString = typeOfChange.Title;
                }
                ObjectMapper.Map(workingProcessDto, check);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetWorkingProcessDto>> GetAll(GetAllWorkingProcessFilter filter)
        {
            try
            {
                int personId = filter.PersonId;

                var result = await _workingProcessRepository.GetAllListAsync(x=>x.PersonId == personId);
                List<GetWorkingProcessDto> dtos = new List<GetWorkingProcessDto>();
                foreach (var w in result)
                {
                    var decisionMaker = w.DecisionMakerId.HasValue ? await _categoryAppService.GetCategorybyId(w.DecisionMakerId.Value) : null;
                    var workingTitle = await _categoryAppService.GetCategorybyId(w.WorkingTitleId);
                    var document = w.DocumentId.HasValue ? await _documentAppService.GetById(w.DocumentId.Value) : null;
                    GetWorkingProcessDto dto = new GetWorkingProcessDto(w, decisionMaker, workingTitle, document);
                    dtos.Add(dto);
                }
                dtos = dtos.OrderBy(x => x.FromDate).ToList();
                return dtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<WorkingProcessDto> GetById(int id)
        {
            try
            {
                var entity = await _workingProcessRepository.FirstOrDefaultAsync(id);
                if(entity == null)
                {
                    throw new Exception("Mã quá trình công tác không tồn tại!");
                }
                var document = entity.DocumentId.HasValue ? await _documentAppService.GetById(entity.DocumentId.Value): null;
                WorkingProcessDto workingProcessDto = ObjectMapper.Map<WorkingProcessDto>(entity);
                if(document != null)
                {
                    workingProcessDto.DecisionNumber = document.DecisionNumber;
                    workingProcessDto.IssuedDate = document.IssuedDate;
                }
                else
                {
                    workingProcessDto.DecisionNumber = "";
                    workingProcessDto.IssuedDate = null as DateTime?;
                }

                return workingProcessDto;
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_Delete)]
        public async Task DeleteById(DeleteWorkingProcessDto deleteWorkingProcessDto)
        {
            try
            {
                int id = deleteWorkingProcessDto.Id;
                int personId = deleteWorkingProcessDto.PersonId;
                var check = await _workingProcessRepository.FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
                if (check == null)
                {
                    throw new Exception("Lỗi phía server!");
                }
                await _workingProcessRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<GetWorkingProcessDto>> GetAllListDetail ()
        {
            try
            {
                var result = await _workingProcessRepository.GetAllListAsync();
                List<GetWorkingProcessDto> dtos = new List<GetWorkingProcessDto>();
                foreach (var w in result)
                {
                    var decisionMaker = w.DecisionMakerId.HasValue ? await _categoryAppService.GetCategorybyId(w.DecisionMakerId.Value) : null;
                    var workingTitle = await _categoryAppService.GetCategorybyId(w.WorkingTitleId);
                    var document = w.DocumentId.HasValue ? await _documentAppService.GetById(w.DocumentId.Value) : null;
                    GetWorkingProcessDto dto = new GetWorkingProcessDto(w, decisionMaker, workingTitle, document);
                    dtos.Add(dto);
                }
                dtos = dtos.OrderBy(x => x.FromDate).ToList();
                return dtos;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public List<int> GetAllListStaffId()
        {
            var list = _workingProcessRepository.GetAll().Select(x=>x.PersonId).Distinct().ToList();
            return list;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Staff_View)]
        public async Task<List<WorkingProcessDto>> GetAllList()
        {
            try
            {
                var entity = await _workingProcessRepository.GetAllListAsync();
                return ObjectMapper.Map<List<WorkingProcessDto>>(entity);
            }
            catch (Exception e)
            {

                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
