using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.mdl.OMS.Application.PurchaseAssignments.Dto;
using bbk.netcore.mdl.OMS.Application.Works.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchaseAssignments
{
    public class PurchaseAssignmentService : ApplicationService, IPurchaseAssignmentService
    {
        private readonly IRepository<PurchaseAssignment> _purchaseAssignmentRepository;
        private readonly IRepository<UserWorkCount> _userWorkRes;
        private IHostingEnvironment _Environment;
        private readonly ISendMailService _sendMailService;
        private readonly IRepository<User, long> _user;
        public PurchaseAssignmentService(IRepository<PurchaseAssignment> purchaseAssignmentRepository, IRepository<UserWorkCount> userWorkRes,
            IHostingEnvironment Environment,
            IRepository<User, long> user,
            ISendMailService sendMailService)
        {
            _purchaseAssignmentRepository = purchaseAssignmentRepository;
            _userWorkRes = userWorkRes;
            _Environment = Environment;
            _user = user;
            _sendMailService = sendMailService;
        }


        /// <summary>
		/// Hàm để tạo mới bản ghi 
		/// created : Kiên
		/// </summary>
		/// <param name="input">Đầu vào là dữ liệu các trường thông tin trong bảng </param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		public async Task<long> Create(PurchaseAssignmentCreateDto input)
        {
            try
            {
                string email = "";
                string name = "";
                PurchaseAssignment newItemId = ObjectMapper.Map<PurchaseAssignment>(input);
                UserWorkCount userWorkCounts = new UserWorkCount();
                userWorkCounts.UserId = newItemId.UserId;
                userWorkCounts.PurchaseAssignmentId = 0;
                userWorkCounts.PurchasesRequestId = 0;
                userWorkCounts.PurchasesSynthesisesId = newItemId.PurchasesSynthesiseId;
                userWorkCounts.TypeStatus = DashboardEnum.TypeStatus.Purchase;
                userWorkCounts.OwnerStatus = PurchasesRequestEnum.OwnerStatusEnum.Assign;
                userWorkCounts.WorkStatus = newItemId.Status;
                await _userWorkRes.InsertAndGetIdAsync(userWorkCounts);

                UserWorkCount userWorkHostCounts = new UserWorkCount();
                userWorkHostCounts.UserId = ((long)AbpSession.UserId);
                userWorkHostCounts.PurchaseAssignmentId = 0;
                userWorkHostCounts.PurchasesRequestId = 0;
                userWorkHostCounts.PurchasesSynthesisesId = newItemId.PurchasesSynthesiseId;
                userWorkHostCounts.TypeStatus = DashboardEnum.TypeStatus.Purchase;
                userWorkHostCounts.OwnerStatus = PurchasesRequestEnum.OwnerStatusEnum.Host;
                userWorkHostCounts.WorkStatus = PurchasesRequestEnum.MyworkStatus.Done;
                await _userWorkRes.InsertAsync(userWorkHostCounts);


               
                var webRootPath = this._Environment.WebRootPath;
                string path = "";
                path = webRootPath + Path.DirectorySeparatorChar.ToString() + "templateEmail" + Path.DirectorySeparatorChar.ToString() + "sendMailCV.html";
                path = File.ReadAllText(path);
                var passlinkEmail = input.Link;
                var user = _user.GetAll().ToList();
                foreach (var item in user)
                {
                    if (item.Id == newItemId.UserId)
                    {
                        email = item.EmailAddress;
                        name = item.Name;
                        path = path.Replace("{{TenNguoiNhan}}", name);
                        path = path.Replace("{{LinkFromEmail}}", passlinkEmail);
                        await _sendMailService.SendEmailCvAsync(email, "Bạn có công việc mới :", path);
                    }
                }

                var newId = await _purchaseAssignmentRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Update(PurchaseAssigmentListDto input)
        {
            PurchaseAssignment producer = await _purchaseAssignmentRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, producer);
            await _purchaseAssignmentRepository.UpdateAsync(producer);
            return input.Id;
        }

        public async Task<PurchaseAssigmentListDto> GetAsync(EntityDto itemId)
        {
            var item = _purchaseAssignmentRepository.Get(itemId.Id);
            PurchaseAssigmentListDto newItem = ObjectMapper.Map<PurchaseAssigmentListDto>(item);
            return newItem;
        }

        public async Task<long> UpdateUserId(PurchaseAssigmentListDto input)
        {
            var userWorkCount = _purchaseAssignmentRepository.GetAll().Where(x => x.PurchasesSynthesiseId == input.PurchasesSynthesiseId && x.UserId == AbpSession.UserId).ToList();
            foreach (var item in userWorkCount)
            {
                input.Id = item.Id;
                input.UserId = item.UserId;
                input.CreationTime = item.CreationTime;
                input.CreatorUserId = item.CreatorUserId;
                input.GetPriceStatus = 0;
                input.ItemId = item.ItemId;
                input.PurchasesSynthesiseId = item.PurchasesSynthesiseId;
                input.Status = item.Status;
                ObjectMapper.Map(input, item);
                await _purchaseAssignmentRepository.UpdateAsync(item);
            }

            return input.Id;
        }


    }
}
