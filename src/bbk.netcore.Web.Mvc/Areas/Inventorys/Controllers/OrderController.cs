using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.Controllers;
using bbk.netcore.mdl.OMS.Application.Contracts;
using bbk.netcore.mdl.OMS.Application.Orders;
using bbk.netcore.mdl.OMS.Application.Suppliers;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.Web.Areas.Inventorys.Models.Order;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.Web.Areas.Inventorys.Controllers
{
    [Area("Inventorys")]
    [AbpMvcAuthorize]
    public class OrderController : netcoreControllerBase
    {
        private readonly IContractAppService _ContractAppServiceService;
        private readonly ISupplierAppService _SupplierAppService;
        private readonly IOrderAppService _OrderAppService;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<User, long> _user;

        public OrderController(IContractAppService ContractAppServiceService,
                                ISupplierAppService SupplierAppService,
                                IOrderAppService OrderAppService,
                               IUserAppService userAppService,
                                IRepository<User, long> user)
        {
             _ContractAppServiceService = ContractAppServiceService;
             _SupplierAppService = SupplierAppService;
             _OrderAppService = OrderAppService;
            _userAppService = userAppService;
            _user= user;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<PartialViewResult> CreateOrder(int id)
        {

            var contract = await _ContractAppServiceService.GetAsync(new EntityDto(id)); 
            var supplier  = await _SupplierAppService.GetAsync(new EntityDto((int)contract.SupplierId));
            //GetUsersInput getUsersInput = new GetUsersInput();
            //getUsersInput.MaxResultCount = 500;
            //var user = _userAppService.GetUsers(getUsersInput).Result.Items.Where(x => x.Id == AbpSession.UserId).ToList();
            var user = _user.FirstOrDefault(x=>x.Id == AbpSession.UserId);
            ViewBag.UserName = user.Surname + " " + user.Name;
            OrderViewModel contractModel = new OrderViewModel
            {
               Contract = contract,
               Supplier = supplier, 
            };
            return PartialView("_CreateModal", contractModel);
        }
        public async Task<ActionResult> OrderDetail(long id)
        {
            var model = await  _OrderAppService.GetAsync(new EntityDto<long>(id));
            var contract = await _ContractAppServiceService.GetAsync(new EntityDto((int)model.ContractId));
            var supplier = await _SupplierAppService.GetAsync(new EntityDto((int)contract.SupplierId));

            //GetUsersInput getUsersInput = new GetUsersInput();
            //getUsersInput.MaxResultCount = 500;
            //var user = _userAppService.GetUsers(getUsersInput).Result.Items.Where(x => x.Id == model.UserId).ToList();
            //ViewBag.UserName = user[0].Surname + " " + user[0].Name;
            var user = _user.FirstOrDefault(x => x.Id == AbpSession.UserId);
            ViewBag.UserName = user.Surname + " " + user.Name;
            OrderViewModel contractModel = new OrderViewModel
            {
                Contract = contract,
                orderListDto = model,
                 Supplier = supplier,
            };
            return View(contractModel);
        }

        public IActionResult Downloadfile(long id)
        {
            var memory = DownloadSinghFile("PO.xlsx", "wwwroot\\data\\tenants\\1\\ExportPO");
            return File(memory.ToArray(), "application/vnd.ms-excel", "PO.xlsx");
        }

        private MemoryStream DownloadSinghFile(string filename, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();

            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;

            }
            memory.Position = 0;
            return memory;
        }

    }

}
