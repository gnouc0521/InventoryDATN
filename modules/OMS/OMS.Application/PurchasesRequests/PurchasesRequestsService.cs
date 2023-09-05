using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.ProfileWorks;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using bbk.netcore.mdl.OMS.Application.Ruleses.Dto;
using bbk.netcore.mdl.OMS.Application.Units.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchasesRequests
{
	public class PurchasesRequestsService : ApplicationService , IPurchasesRequestsService
	{
		private readonly IRepository<PurchasesRequest,long> _puchasesRequestRepository;
		private readonly IRepository<Subsidiary,long> _subsidiaryRepository;
		private readonly IRepository<UserWorkCount> _userWorkCountRepository;
		public PurchasesRequestsService(IRepository<PurchasesRequest, long> puchasesRequestRepository, 
			IRepository<Subsidiary, long> subsidiaryRepository,
            IRepository<UserWorkCount> userWorkCountRepository) 
		{
			_puchasesRequestRepository= puchasesRequestRepository;
            _subsidiaryRepository = subsidiaryRepository;
			_userWorkCountRepository= userWorkCountRepository;

        }



		/// <summary>
		/// hàm để lấy ra danh sách các bản ghi của yêu cầu mua hàng
		/// createdby: Kiên
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		public async Task<PagedResultDto<PurchasesRequestListDto>> GetAll(PurchasesRequestSearch input)
		{
			try
			{
				var subsidiaryInfo = _subsidiaryRepository.GetAll();
				var query = _puchasesRequestRepository
					  .GetAll();
				var result = (from p in query
							  join s in subsidiaryInfo on p.SubsidiaryCompanyId equals s.Id
                              select new PurchasesRequestListDto
							  {
								  Id= p.Id,
								  SubsidiaryCompanyId = p.SubsidiaryCompanyId,
								  SubsidiaryCompany = s.NameCompany,
								  Address = s.Address,
								  PhoneNumber= s.PhoneNumber,
								  EmailAddress=s.EmailAddress,
								  RequestStatus = p.RequestStatus,
								  RequestDate= p.RequestDate,
							  }).ToList().OrderByDescending(x=>x.Id).OrderByDescending(x=>x.RequestStatus);
				return new PagedResultDto<PurchasesRequestListDto>(
                  result.Distinct().Count(),
                  result.Distinct().ToList()
                  );
			}
			catch (Exception ex)
			{

				throw new UserFriendlyException(ex.Message);
			}
		}


		/// <summary>
		/// Hàm để tạo mới bản ghi bảng yêu cầu mua hàng
		/// created : Kiên
		/// </summary>
		/// <param name="input">Đầu vào là dữ liệu các trường thông tin trong bảng </param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		public async Task<long> Create(PurchasesRequestCreateDto input)
		{
			try
			{
				PurchasesRequest newItemId = ObjectMapper.Map<PurchasesRequest>(input);
				var newId = await _puchasesRequestRepository.InsertAndGetIdAsync(newItemId);
				UserWorkCount userWorkCounts = new UserWorkCount();
				userWorkCounts.UserId = (long)newItemId.CreatorUserId;
				userWorkCounts.PurchasesRequestId = newId;
				userWorkCounts.PurchaseAssignmentId = 0;
				userWorkCounts.PurchasesSynthesisesId = 0;
				userWorkCounts.TypeStatus = DashboardEnum.TypeStatus.Plan;
				userWorkCounts.OwnerStatus = PurchasesRequestEnum.OwnerStatusEnum.Assign;
				userWorkCounts.WorkStatus = newItemId.RequestStatus;
				await _userWorkCountRepository.InsertAsync(userWorkCounts);
				return newId;
			}
			catch (Exception ex)
			{
				throw new UserFriendlyException(ex.Message);
			}
		}


		/// <summary>
		/// Hàm tạo ra để sửa bản ghi bảng yêu cầu mua hàng
		/// CreatedBy : Kiên
		/// </summary>
		/// <param name="input">Id bản ghi cần sửa</param>
		/// <returns></returns>
		public async Task<long> Update(PurchasesRequestListDto input)
		{
			PurchasesRequest purchasesRequest = await _puchasesRequestRepository.GetAsync(input.Id);
			input.PurchasesSynthesiseId = purchasesRequest.PurchasesSynthesiseId;
			ObjectMapper.Map(input, purchasesRequest);
			await _puchasesRequestRepository.UpdateAsync(purchasesRequest);
			return input.Id;
		}






		/// <summary>
		/// Hàm lấy Id bản ghi phục vụ cho mục dích sửa hoặc lấy ra bản ghi cần dùng
		/// created : Kiên
		/// </summary>
		/// <param name="itemId">id bản ghi cần lấy </param>
		/// <returns></returns>
		public async Task<PurchasesRequestListDto> GetAsync(EntityDto itemId)
		{
			var item = _puchasesRequestRepository.Get(itemId.Id);
			PurchasesRequestListDto newItem = ObjectMapper.Map<PurchasesRequestListDto>(item);
			return newItem;
		}



		/// <summary>
		/// Hàm để xoá bản ghi bảng yêu cầu mua hàng
		/// createdby : Kiên
		/// </summary>
		/// <param name="id">id của bản ghi</param>
		/// <returns></returns>
		/// <exception cref="UserFriendlyException"></exception>
		public async Task<int> Delete(int id)
		{
			try
			{
				await _puchasesRequestRepository.DeleteAsync(id);
				return id;
			}
			catch (Exception ex)
			{

				throw new UserFriendlyException(ex.Message);
			}
		}

        /// <summary>
        /// Hàm tạo ra để thêm PurchasesSynthesisesId
        /// CreatedBy : Kiên
        /// </summary>
        /// <param name="input">Id bản ghi cần sửa</param>
        /// <returns></returns>
        public async Task<long> UpdateSynId(PurchasesRequestListDto input)
        {

            PurchasesRequest purchasesRequest = await _puchasesRequestRepository.GetAsync(input.Id);
			input.CreationTime = purchasesRequest.CreationTime;
			input.SubsidiaryCompanyId = purchasesRequest.SubsidiaryCompanyId;
			input.RequestDate = purchasesRequest.RequestDate;
            var mgs =  (purchasesRequest.RequestStatus == 0 ) ? input.RequestStatus = PurchasesRequestEnum.MyworkStatus.Draf : input.RequestStatus = PurchasesRequestEnum.MyworkStatus.Done;

			if (purchasesRequest.LastModificationTime.HasValue)
			{
				input.CreatorUserId = purchasesRequest.CreatorUserId;
                input.LastModificationTime = purchasesRequest.LastModificationTime;
                input.LastModifierUserId = purchasesRequest.LastModifierUserId;
            }

            ObjectMapper.Map(input, purchasesRequest);
            await _puchasesRequestRepository.UpdateAsync(purchasesRequest);
            return input.Id;
        }
    }
}
