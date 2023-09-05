using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using bbk.netcore.mdl.OMS.Application.UserWorkCounts.Dto;
using bbk.netcore.mdl.OMS.Application.UserWorks.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.UserWorkCounts
{
    public class UserWorkCountService : ApplicationService, IUserWorkCountService
    {
        private readonly IRepository<UserWorkCount> _userWorkCountsRepository;
        public UserWorkCountService(IRepository<UserWorkCount> userWorkCountsRepository) 
        {
            _userWorkCountsRepository = userWorkCountsRepository;
        }

        public async Task<int> Create(UserWorkCountCreateDto input)
        {
            try
            {
                UserWorkCount newItemId = ObjectMapper.Map<UserWorkCount>(input);
                var newId = await _userWorkCountsRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> UpdateRequest(UserWorkCountListDto input)
        {
            var userWorkCount =  _userWorkCountsRepository.GetAll().Where(x=>x.PurchasesRequestId == input.PurchasesRequestId ).ToList();
            foreach(var item in userWorkCount ) 
            {
                input.Id = item.Id;
                input.UserId = item.UserId;
                input.CreationTime = item.CreationTime;
                input.CreatorUserId = item.CreatorUserId;
                input.PurchaseAssignmentId = item.PurchaseAssignmentId;
                input.OwnerStatus = item.OwnerStatus;
                input.PurchasesRequestId = item.PurchasesRequestId;
                ObjectMapper.Map(input, item);
                await _userWorkCountsRepository.UpdateAsync(item);
            }    
           
            return input.Id;
        }

        public async Task<long> UpdateSys(UserWorkCountListDto input)
        {
            var userWorkCount = _userWorkCountsRepository.GetAll().Where(x => x.PurchasesSynthesisesId == input.PurchasesSynthesisesId && x.UserId == AbpSession.UserId).ToList();
            foreach (var item in userWorkCount)
            {
                input.Id = item.Id;
                input.UserId = item.UserId;
                input.CreationTime = item.CreationTime;
                input.CreatorUserId = item.CreatorUserId;
                input.PurchaseAssignmentId = item.PurchaseAssignmentId;
                input.OwnerStatus = item.OwnerStatus;
                input.PurchasesRequestId = item.PurchasesRequestId;
                input.PurchasesSynthesisesId = item.PurchasesSynthesisesId;
                ObjectMapper.Map(input, item);
                await _userWorkCountsRepository.UpdateAsync(item);
            }

            return input.Id;
        }

        public async Task<long> UpdateUserId(UserWorkCountListDto input)
        {
            var userWorkCount = _userWorkCountsRepository.GetAll().Where(x => x.UserId == input.UserId).ToList();
            foreach (var item in userWorkCount)
            {
                input.Id = item.Id;
                input.UserId = item.UserId;
                input.CreationTime = item.CreationTime;
                input.CreatorUserId = item.CreatorUserId;
                input.PurchaseAssignmentId = item.PurchaseAssignmentId;
                input.OwnerStatus = item.OwnerStatus;
                input.PurchasesRequestId = item.PurchasesRequestId;
                ObjectMapper.Map(input, item);
                await _userWorkCountsRepository.UpdateAsync(item);
            }

            return input.Id;
        }

    }
}
