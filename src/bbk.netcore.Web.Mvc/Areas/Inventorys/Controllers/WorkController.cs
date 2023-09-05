using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.Web.Models;
using AutoMapper;
using bbk.netcore.Authorization;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.ProfileWorks;
using bbk.netcore.mdl.OMS.Application.WorkGroups;
using bbk.netcore.mdl.OMS.Application.Works;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Application.WorkUsers;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using bbk.netcore.mdl.PersonalProfile.Application.Categories.Dto;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.Web.Areas.Inventorys.Models.Work;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static bbk.netcore.mdl.OMS.Core.Enums.WorkEnum;
using Path = System.IO.Path;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class WorkController : netcoreControllerBase
    {

        private readonly IWorkAppService _workAppService;
        private readonly IProfileWorkAppService _profileWorkAppService;
        private readonly IWorkGroupAppService _workGroupAppService;
        private readonly IRepository<DayOff> _dayoffrepository;
        private readonly UserManager _userManager;
        private readonly IWebHostEnvironment _iWebHostEnvironment;
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        private readonly IUserWorkAppService _iUserWorkAppService;
        private readonly IRepository<UploadFileCV> _uploadFileRepository;
        private readonly IRepository<Work> _workRepository;
        private readonly IRepository<UserWork> _userWorkrepository;
        public WorkController(
             IWorkAppService workAppService,
             UserManager userManager,
             IRepository<DayOff> dayoffrepository,
             IWebHostEnvironment iWebHostEnvironment,
             IWorkGroupAppService workGroupAppService,
             IProfileWorkAppService profileWorkAppService,
             IFileSystemBlobProvider fileSystemBlobProvider,
             IUserWorkAppService iUserWorkAppService,
             IRepository<UploadFileCV> uploadFileRepository,
             IRepository<Work> workRepository,
             IRepository<UserWork> userWorkrepository)
        {
            _workAppService = workAppService;
            _userManager = userManager;
            _dayoffrepository = dayoffrepository;
            _iWebHostEnvironment = iWebHostEnvironment;
            _workGroupAppService = workGroupAppService;
            _profileWorkAppService = profileWorkAppService;
            _fileSystemBlobProvider = fileSystemBlobProvider;
            _iUserWorkAppService = iUserWorkAppService;
            _uploadFileRepository = uploadFileRepository;
            _workRepository = workRepository;
            _userWorkrepository = userWorkrepository;
        }
        public async Task<ActionResult> Index(int? StatusId)
        {
            if (StatusId.HasValue == true)
            {
                IndexWorkViewModel modal = new IndexWorkViewModel()
                {
                    statusId = (int)StatusId.Value,
                };
                return View(modal);
            }
            else
            {
                return View();
            }
            

        }

        [AbpAuthorize(PersonalProfilePermission.PersonalProfileCV)]
        public async Task<PartialViewResult> CreateWork(WorkCreateDto model)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var role in _userManager.Users.Where(x => x.UserName != "admin"))
                listItems.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.Surname + " " + role.Name });
            ViewBag.Name = listItems;


            var listWorkGroup = await _workGroupAppService.GetAllList();
            var listProfileWork = await _profileWorkAppService.GetAllList();

            model = new WorkCreateDto
            {
                workGroupListDtos = listWorkGroup.Items,
                profileWorkGroupListDtos = listProfileWork.Items
            };
            return PartialView("_CreateModal", model);
        }

        [AbpAuthorize(PersonalProfilePermission.PersonalProfileCV_Edit)]
        public async Task<IActionResult> EditWorkModal(int Id)
        {
            var listWorkGroup = await _workGroupAppService.GetAllList();
            var listProfileWork = await _profileWorkAppService.GetAllList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var role in _userManager.Users.Where(x => x.UserName != "admin"))
                listItems.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.Surname + " " + role.Name });
            ViewBag.Name = listItems;
            var model = new WorkEditDto()
            {
                workGroupListDtos = listWorkGroup.Items,
                profileWorkGroupListDtos = listProfileWork.Items,
            };

            var Dto = await _workAppService.GetAsync(new EntityDto(Id));
            ViewBag.FileName = Path.GetFileName(Dto.FilePath);
            List<SelectListItem> listItems1 = new List<SelectListItem>();
            foreach (var role in _userManager.Users.Where(x => x.Id != AbpSession.UserId && x.Id != Dto.HostId && x.UserName != "admin"))
                listItems1.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.Surname + " " + role.Name });
            ViewBag.UserCoWord = listItems1;
            if (Dto != null)
            {
                var docTypeId = _workAppService.GetById(Id).Result.IdWorkGroup;
                var profileWorkTypeId = _workAppService.GetById(Id).Result.IdProfileWork;
                model.Id = Dto.Id;
                model.IdWorkGroup = docTypeId;
                model.IdProfileWork = profileWorkTypeId;
                model.Title = Dto.Title;
                model.Description = Dto.Description;
                model.EndDate = Dto.EndDate;
                model.StartDate = Dto.StartDate;
                model.HostId = Dto.HostId;
                model.UserId = Dto.UserId;
                model.Status = Dto.Status;
                model.jobLabel = Dto.jobLabel;
                model.priority = Dto.priority;
                model.CompletionTime = Dto.CompletionTime;
            }
            return PartialView("_EditModal", model);
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocument(WorkCreateDto model)
        {
            try
            {
                if (Request.Form.Files.Count() == 0)
                {
                    await _workAppService.Create(model);
                }
                else
                {
                    var file = Request.Form.Files.ToList();
                    Work newItemId = ObjectMapper.Map<Work>(model);
                    var newId = await _workRepository.InsertAndGetIdAsync(newItemId);
                    if (newItemId.HostId == AbpSession.UserId)
                    {
                        // Create particular user
                        UserWork userWork = new UserWork();
                        userWork.WorkId = newId;
                        //userWork.WorkId = newItem.Id;
                        userWork.Status = newItemId.Status;
                        userWork.UserId = newItemId.HostId;
                        userWork.OwnerStatus = WorkEnum.OwnerStatusEnum.Host;
                        await _userWorkrepository.InsertAsync(userWork);
                    }
                    else
                    {
                        // Create particular user
                        UserWork userWork = new UserWork();
                        userWork.WorkId = newId;
                        //userWork.WorkId = newItem.Id;
                        userWork.Status = newItemId.Status;
                        userWork.UserId = newItemId.HostId;
                        userWork.OwnerStatus = WorkEnum.OwnerStatusEnum.Host;
                        await _userWorkrepository.InsertAsync(userWork);

                        UserWork userAssignWork = new UserWork();
                        userAssignWork.WorkId = newId;
                        //userAssignWork.WorkId = newItem.Id;
                        userAssignWork.Status = newItemId.Status;
                        userAssignWork.UserId = newItemId.CreatorUserId.Value;
                        userAssignWork.OwnerStatus = WorkEnum.OwnerStatusEnum.Assign;
                        await _userWorkrepository.InsertAsync(userAssignWork);
                    }

                    foreach (var item in file)
                    {
                        FileInfo fileInfo = new FileInfo(item.FileName);
                        var filePath = Path.Combine(Path.GetFileName(fileInfo.Name.ToString().Replace(fileInfo.Extension, "")) + Clock.Now.ToString("yyyyMMddhhmmss") + fileInfo.Extension);
                        using (var stream = item.OpenReadStream())
                        {
                           

                            UploadFileCV uploadFile = new UploadFileCV();
                            string fullFilePath = await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(PersonalProfileCoreConsts.AttactFile + @"\\" + filePath, stream)); ;
                            uploadFile.FilePath = fullFilePath;
                            int index = fullFilePath.IndexOf("\\data");
                            uploadFile.FileUrl = fullFilePath.Substring(index);
                            uploadFile.FileName = filePath;
                            uploadFile.WorkId = newId;
                            await _uploadFileRepository.InsertAsync(uploadFile);
                           
                        }

                    }
                    //await _workAppService.Create(model);

                }
                return Json(new AjaxResponse(new { data = model.Title}));
            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> EditUploadDocument(WorkEditDto model)
        {
            try
            {
                WorkEditDto item = new WorkEditDto();
                if (Request.Form.Files.Count() == 0 && model.Id.HasValue)
                {
                    item = await _workAppService.GetAsync(new EntityDto((int)model.Id));
                    item.FilePath = model.FilePath;
                    item.FileUrl = model.FileUrl;
                    item.Id = model.Id;
                    item.IdWorkGroup = model.IdWorkGroup;
                    item.IdProfileWork = model.IdProfileWork;
                    item.Title = model.Title;
                    item.Description = model.Description;
                    item.EndDate = model.EndDate;
                    item.StartDate = model.StartDate;
                    item.HostId = model.HostId;
                    item.Status = model.Status;
                    item.jobLabel = model.jobLabel;
                    item.priority = model.priority;
                    item.HostId = model.HostId;
                    item.CompletionTime = model.CompletionTime; 
                    await _workAppService.Update(model);

                }
                else
                {
                    var file = Request.Form.Files.ToList();
                    Work lecture = _workRepository.Get(((int)model.Id));
                    ObjectMapper.Map(model, lecture);
                    var changstatus2 = _userWorkrepository.GetAll().Where(x => x.WorkId == lecture.Id);
                    foreach (var status in changstatus2)
                    {
                        item.Status = lecture.Status;
                        await _userWorkrepository.UpdateAsync(status);
                    }
                    if (lecture.Status == WorkEnum.Status.Done)
                    {
                        lecture.CompletionTime = DateTime.Now;
                    }
                    await _workRepository.UpdateAsync(lecture);
                    foreach (var item1 in file)
                    {
                        FileInfo fileInfo = new FileInfo(item1.FileName);
                        var filePath = Path.Combine(Path.GetFileName(fileInfo.Name.ToString().Replace(fileInfo.Extension, "")) + Clock.Now.ToString("yyyyMMddhhmmss") + fileInfo.Extension);
                        using (var stream = item1.OpenReadStream())
                        {


                            UploadFileCV uploadFile = new UploadFileCV();
                            string fullFilePath = await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(PersonalProfileCoreConsts.AttactFile + @"\\" + filePath, stream)); ;
                            uploadFile.FilePath = fullFilePath;
                            int index = fullFilePath.IndexOf("\\data");
                            uploadFile.FileUrl = fullFilePath.Substring(index);
                            uploadFile.FileName = filePath;
                            uploadFile.WorkId = lecture.Id;
                            await _uploadFileRepository.InsertAsync(uploadFile);

                        }
                    }
                }
                return Json(new AjaxResponse(new { data = model.FileUrl }));
            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }



        public IActionResult Filtter(int? StatusId)
        {
            if (StatusId.HasValue == true)
            {
                IndexWorkViewModel modal = new IndexWorkViewModel()
                {
                    statusId = (int)StatusId.Value,
                };
                return View(modal);
            }
            else
            {
                return View();
            }
        }


        //Code Hà
        public async Task<ActionResult> SelectUser()
        {
            return PartialView("_UserModal");
        }

        //Xem công việc
        public async Task<IActionResult> ViewDetails(int Id)
        {
            var listWorkGroup = await _workGroupAppService.GetAllList();
            var listProfileWork = await _profileWorkAppService.GetAllList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            foreach (var role in _userManager.Users.Where(x => x.UserName != "admin"))
                listItems.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.Surname + " " + role.Name });
            ViewBag.Name = listItems;
            var model = new WorkEditDto()
            {
                workGroupListDtos = listWorkGroup.Items,
                profileWorkGroupListDtos = listProfileWork.Items,
            };

            var Dto = await _workAppService.GetAsync(new EntityDto(Id));
            ViewBag.FileName = Path.GetFileName(Dto.FilePath);
            List<SelectListItem> listItems1 = new List<SelectListItem>();
            foreach (var role in _userManager.Users.Where(x => x.Id != AbpSession.UserId && x.Id != Dto.HostId && x.UserName != "admin"))
                listItems1.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.Surname + " " + role.Name });
            ViewBag.UserCoWord = listItems1;
            if (Dto != null)
            {
                var docTypeId = _workAppService.GetById(Id).Result.IdWorkGroup;
                var profileWorkTypeId = _workAppService.GetById(Id).Result.IdProfileWork;
                model.Id = Dto.Id;
                model.IdWorkGroup = docTypeId;
                model.IdProfileWork = profileWorkTypeId;
                model.Title = Dto.Title;
                model.Description = Dto.Description;
                model.EndDate = Dto.EndDate;
                model.StartDate = Dto.StartDate;
                model.HostId = Dto.HostId;
                model.UserId = Dto.UserId;
                model.Status = Dto.Status;
                model.jobLabel = Dto.jobLabel;
                model.priority = Dto.priority;
               
            }
            return PartialView("ViewDetails", model);
        }


    }
}
