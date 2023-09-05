using System;
using System.IO;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Timing;
using bbk.netcore.Controllers;
using bbk.netcore.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Settings;
using bbk.netcore.mdl.PersonalProfile.Application.Settings.Dto;
using bbk.netcore.mdl.PersonalProfile.Core;
using bbk.netcore.Net.MimeTypes;
using bbk.netcore.Storage.FileSystem;
using bbk.netcore.Web.Areas.Inventorys.Models.Settings;
using Microsoft.AspNetCore.Mvc;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class SettingsController : netcoreControllerBase
    {
        private readonly IFileSystemBlobProvider _fileSystemBlobProvider;
        private readonly ISettingUIAppService _settingAppService;
        public SettingsController(ISettingUIAppService settingAppService,
        IFileSystemBlobProvider fileSystemBlobProvider)
        {
            _settingAppService = settingAppService;
            _fileSystemBlobProvider = fileSystemBlobProvider;
        }
        public IActionResult Index()
        {
            var model = _settingAppService.Get();
            ViewBag.data = model;
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Index(IndexViewModel model)
        {
            try
            {
                UpdateDto dto = new UpdateDto()
                {
                    Header = model.Header,
                    Footer = model.Footer
                };
                if (model.Logo != null)
                {
                    var logoFile = model.Logo;

                    FileInfo fileInfo = new FileInfo(logoFile.FileName);

                    FileDto fileDto = new FileDto(PersonalProfileCoreConsts.SettingUi + "\\" + Path.GetFileName(fileInfo.Name) + Clock.Now.ToString("yyyyMMddhhmmss") + fileInfo.Extension, MimeTypeNames.ImagePng);

                    var filePath = Path.Combine(Path.GetFileName(fileInfo.Name) + Clock.Now.ToString("yyyyMMddhhmmss") + fileInfo.Extension);
                    using (var stream = logoFile.OpenReadStream())
                    {
                        string fullFilePath = await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, stream));
                        // HARDCODE:
                        dto.LogoPath = fullFilePath;
                        dto.LogoUrl = "/" + fullFilePath.Substring(fullFilePath.IndexOf("_cdn")).Replace("\\", "/");
                    }
                }
                if (model.Banner != null)
                {
                    var bannerFile = model.Banner;
                    FileInfo fileInfo = new FileInfo(bannerFile.FileName);

                    FileDto fileDto = new FileDto(PersonalProfileCoreConsts.SettingUi + "\\" + Path.GetFileName(fileInfo.Name) + Clock.Now.ToString("yyyyMMddhhmmss") + fileInfo.Extension, MimeTypeNames.ImagePng);

                    var filePath = Path.Combine(Path.GetFileName(fileInfo.Name) + Clock.Now.ToString("yyyyMMddhhmmss") + fileInfo.Extension);
                    using (var stream = bannerFile.OpenReadStream())
                    {
                        string fullFilePath = await _fileSystemBlobProvider.SaveAsync(new Storage.StorageProviderSaveArgs(fileDto.FileName, stream));
                        // HARDCODE:
                        dto.BannerPath = fullFilePath;
                        dto.BannerUrl = "/" + fullFilePath.Substring(fullFilePath.IndexOf("_cdn")).Replace("\\", "/");
                    }
                }
                await _settingAppService.Update(dto);
                return new JsonResult("Cập nhật thành công!");
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }

        }
    }

}