using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Controllers;
using bbk.netcore.EntityFrameworkCore;
using bbk.netcore.mdl.OMS.Application.Dashboard;
using bbk.netcore.mdl.OMS.Application.PurchasesSynthesises;
using bbk.netcore.mdl.OMS.Application.Works;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using bbk.netcore.Web.Areas.Inventorys.Models;
using bbk.netcore.Web.Areas.Inventorys.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class DashboardController : netcoreControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IRepository<UserWorkCount> _userWorkCount;
        private readonly RoleManager _role;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<ImportRequest> _imRepository;
        private readonly IRepository<ExportRequest,long> _exRepository;
        private readonly IRepository<ImportRequestSubsidiary> _impSRepository;
        private readonly IRepository<PurchasesSynthesise,long> _PurchasesSynthesise;
        private readonly IRepository<Transfer, long> _Transfer ;
        public DashboardController(
            UserManager userManager,  
            RoleManager role, IRepository<UserWorkCount> userWorkCount, 
            IRepository<Contract>
            contractRepository, IRepository<ImportRequest> imRepository, IRepository<ExportRequest, long> exRepository, 
            IRepository<ImportRequestSubsidiary> impSRepository,
            IRepository<PurchasesSynthesise, long> PurchasesSynthesise,
            IRepository<Transfer, long> Transfer)
        {
            _userManager = userManager;
            _role = role;
            _userWorkCount = userWorkCount;
            _contractRepository = contractRepository;
            _imRepository = imRepository;
            _exRepository = exRepository;
            _impSRepository = impSRepository;
            _PurchasesSynthesise= PurchasesSynthesise;
            _Transfer = Transfer;
        }
        
        public ActionResult Index()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == AbpSession.UserId);
            var roles = _userManager.GetRolesAsync(user);
            string rolename = _role.FindByNameAsync(roles.Result[0].ToString()).Result.DisplayName;
            ViewBag.name = rolename;
            ViewBag.userId = user.Id;
            if (rolename == "Giám đốc")
            {
                DashboardViewModel model = new DashboardViewModel();
               // int countALL = _contractRepository.GetAll().Count();
                int countInProcess = _contractRepository.GetAll().Where(x => x.Status.Equals(PurchasesRequestEnum.MyworkStatus.Draf)).Count();
                int countDone = _contractRepository.GetAll().Where(x => x.Status.Equals(PurchasesRequestEnum.MyworkStatus.Done)).Count();
                int countOverTime = _contractRepository.GetAll().Where(x => x.Status.Equals(PurchasesRequestEnum.MyworkStatus.OutOfDate)).Count();
                int countALL = countInProcess + countDone + countOverTime;
                model = new DashboardViewModel
                {
                    Tongcongviec = countALL,
                    Congviecxuly = countInProcess,
                    Congviechoanthanh = countDone,
                    Congviecquahan = countOverTime,
                };

                return View("Cofirm",model);
            }
            else if (rolename == "Thủ Kho")
            {
                DashboardViewModel model = new DashboardViewModel();

                int countInProcessPN = _imRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.Status.Draft)).Count();
                int countInProcessPX = _exRepository.GetAll().Where(x => x.ExportStatus.Equals(ExportEnums.Export.Waiting)).Count();
                int countInProcess = countInProcessPN + countInProcessPX;

                int countDonePN = _imRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.Status.Approve)).Count();
                int countDonePx = _exRepository.GetAll().Where(x => x.ExportStatus.Equals(ExportEnums.Export.Approve)).Count();
                int countDone = countDonePN + countDonePx;

                int countOverTime = _imRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.MyworkStatus.OutOfDate)).Count();

                int countALL = countInProcess + countDone + countOverTime;
                model = new DashboardViewModel
                {
                    Tongcongviec = countALL,
                    Congviecxuly = countInProcess,
                    Congviechoanthanh = countDone,
                    Congviecquahan = countOverTime,
                };

                return View("Thukho", model);
            }
            else if(rolename == "Chuyên viên kho")
            {
                DashboardViewModel model = new DashboardViewModel();
               // int countALLPN = _imRepository.GetAll().Count();
                int countInProcessPN = _imRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.Status.Draft)).Count();
                int countDonePN = _imRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.Status.Approve)).Count();
                int countOverTime = _imRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.MyworkStatus.OutOfDate)).Count();
                int countALLPN = countInProcessPN + countDonePN + countOverTime;
                model = new DashboardViewModel
                {
                    Tongcongviec = countALLPN,
                    Congviecxuly = countInProcessPN,
                    Congviechoanthanh = countDonePN,
                    Congviecquahan = countOverTime,
                };
                return View("CVKho", model);
            }
            else if (rolename == "Trưởng phòng kế hoạch")
            {
                DashboardViewModel model = new DashboardViewModel();

                int countInProcessTH = _PurchasesSynthesise.GetAll().Where(x=>x.PurchasesRequestStatus == PurchasesRequestEnum.YCNK.Draft ||
                x.PurchasesRequestStatus == PurchasesRequestEnum.YCNK.Original).Count();

                int countInProcessDC = _Transfer.GetAll().Where(x => x.Status == TransferEnum.TransferStatus.Original ||
                x.Status == TransferEnum.TransferStatus.Waitinng).Count();

                int countInProcess = countInProcessTH + countInProcessDC;

                int countDoneTH = _PurchasesSynthesise.GetAll().Where(x => x.PurchasesRequestStatus == PurchasesRequestEnum.YCNK.Approve).Count();
                int countDoneDC = _Transfer.GetAll().Where(x => x.Status == TransferEnum.TransferStatus.Approve).Count();
                int countDone = countDoneTH + countDoneDC;

                int countOverTimeTH = _PurchasesSynthesise.GetAll().Where(x => x.PurchasesRequestStatus == PurchasesRequestEnum.YCNK.Reject).Count();
                int countOverTimeDC = _Transfer.GetAll().Where(x => x.Status == TransferEnum.TransferStatus.Reject).Count();
                int countOverTime = countOverTimeTH + countOverTimeDC;
                int countALL = countInProcess + countDone + countOverTime;
                model = new DashboardViewModel
                {
                    Tongcongviec = countALL,
                    Congviecxuly = countInProcess,
                    Congviechoanthanh = countDone,
                    Congviecquahan = countOverTime,
                };
                return View("TPKHConfirm", model);
            }
            else if(rolename == "Admin")
            {
                DashboardViewModel model = new DashboardViewModel();
                int countReject = 0;
                //int countALL = _userWorkCount.GetAll().Where(x => x.UserId == AbpSession.UserId).Count();
                int countInProcess = _userWorkCount.GetAll().Where(x => x.WorkStatus.Equals(PurchasesRequestEnum.MyworkStatus.Draf) && x.UserId == AbpSession.UserId).Count();
                int countDone = _userWorkCount.GetAll().Where(x => x.WorkStatus.Equals(PurchasesRequestEnum.MyworkStatus.Done) && x.UserId == AbpSession.UserId).Count();
                int countOverTime = _userWorkCount.GetAll().Where(x => x.WorkStatus.Equals(PurchasesRequestEnum.MyworkStatus.OutOfDate) && x.UserId == AbpSession.UserId).Count();
                int countALL = countInProcess + countDone + countOverTime;
                model = new DashboardViewModel
                {
                    Tongcongviec = countALL,
                    Congviecxuly = countInProcess,
                    Congviechoanthanh = countDone,
                    Congviecquahan = countOverTime,
                    Congviectuchoi = countReject
                };
                return View("Admin",model);
            }
            else
            {
                DashboardViewModel model = new DashboardViewModel();
                int countReject = 0;
                int countALL = _userWorkCount.GetAll().Where(x => x.UserId == AbpSession.UserId).Count();
                int countInProcess = _userWorkCount.GetAll().Where(x => x.WorkStatus.Equals(PurchasesRequestEnum.MyworkStatus.Draf) && x.UserId == AbpSession.UserId).Count();
                int countDone = _userWorkCount.GetAll().Where(x => x.WorkStatus.Equals(PurchasesRequestEnum.MyworkStatus.Done) && x.UserId == AbpSession.UserId).Count();
                int countOverTime = _userWorkCount.GetAll().Where(x => x.WorkStatus.Equals(PurchasesRequestEnum.MyworkStatus.OutOfDate) && x.UserId == AbpSession.UserId).Count();
                if (rolename == "Trưởng phòng mua hàng")
                {
                    int countALLSPN = _impSRepository.GetAll().Count();
                    int countALLPx = _exRepository.GetAll().Where(x => x.Status != ExportEnums.ExportStatus.Complete).Count();
                    countALL = countALL + countALLSPN + countALLPx;

                    int countInProcessYCPN = _impSRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.YCNK.Draft)).Count();
                    int countInProcessYCPX = _exRepository.GetAll().Where(x => x.Status.Equals(ExportEnums.ExportStatus.Draft)).Count();
                    countInProcess = countInProcess + countInProcessYCPN + countInProcessYCPX;

                    int countDoneYCPN = _impSRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.YCNK.Approve)).Count();
                    int countDoneYCPx = _exRepository.GetAll().Where(x => x.Status.Equals(ExportEnums.ExportStatus.Approve)).Count();
                    countDone = countDone+ countDoneYCPx + countDoneYCPN;

                    int countOverTimeYCPN = _impSRepository.GetAll().Where(x => x.ImportStatus.Equals(PurchasesRequestEnum.YCNK.Reject)).Count();
                    int countOverTimeYCPx = _exRepository.GetAll().Where(x => x.Status.Equals(ExportEnums.ExportStatus.Reject)).Count();
                     countReject = countOverTimeYCPN + countOverTimeYCPx;
                }
                model = new DashboardViewModel
                {
                    Tongcongviec = countALL,
                    Congviecxuly = countInProcess,
                    Congviechoanthanh = countDone,
                    Congviecquahan = countOverTime,
                    Congviectuchoi = countReject
                };
                return View(model);
            }
        }
    }
}
