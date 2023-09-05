using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails.Dto;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests;
using bbk.netcore.mdl.OMS.Application.PurchasesRequests.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.PurchasesRequestDetails
{
    public class PurchasesRequestDetailService : ApplicationService, IPurchasesRequestDetailService
    {
        private readonly IRepository<PurchasesRequestDetail, long> _puchasesRequestDetailRepository;
        private readonly IRepository<Items, long> _items;
        private readonly IRepository<Supplier> _supplier;
        private readonly IRepository<Unit> _unit;

        public PurchasesRequestDetailService(
            IRepository<PurchasesRequestDetail,
            long> puchasesRequestDetailRepository,
            IRepository<Items, long> items,
            IRepository<Supplier> supplier,
            IRepository<Unit> unit)
        {
            _puchasesRequestDetailRepository = puchasesRequestDetailRepository;
            _items = items;
            _supplier = supplier;
            _unit = unit;
        }



        /// <summary>
        /// hàm để lấy ra danh sách các bản ghi của chi tiet yêu cầu mua hàng
        /// createdby: Kiên
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<PagedResultDto<PurchasesRequestDetailListDto>> GetAll(PurchasesRequestDetailSearch input)
        {
            try
            {
                var query = _puchasesRequestDetailRepository
                      .GetAll().Where(x => x.PurchasesRequestId == input.PurchasesRequestId);
                var itemInfo = _items.GetAll();
                var unitInfo = _unit.GetAll();
                var supInfo = _supplier.GetAll();

                var results = (from p in query
                               join i in itemInfo on p.ItemId equals i.Id
                               join u in unitInfo on p.UnitId equals u.Id
                               join s in supInfo on p.SupplierId equals s.Id
                               select new PurchasesRequestDetailListDto
                               {
                                   Id= p.Id,
                                   NameItem = i.ItemCode + "-" + i.Name,
                                   NameNCC = s.Name,
                                   NameUnit = u.Name,
                                   Quantity = p.Quantity,
                                   Uses = p.Uses,
                                   TimeNeeded = p.TimeNeeded,
                                   Note = p.Note,
                                   ItemId=p.ItemId,
                                   UnitId=p.UnitId,
                                   SupplierId=p.SupplierId,
                               });

                return new PagedResultDto<PurchasesRequestDetailListDto>(
                     results.Distinct().Count(),
                     results.Distinct().ToList()

                  );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }


        /// <summary>
        /// Hàm để tạo mới bản ghi bảng chi tiet yêu cầu mua hàng
        /// created : Kiên
        /// </summary>
        /// <param name="input">Đầu vào là dữ liệu các trường thông tin trong bảng </param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<long> Create(PurchasesRequestDetailCreateDto input)
        {
            try
            {
                PurchasesRequestDetail newItemId = ObjectMapper.Map<PurchasesRequestDetail>(input);
                var newId = await _puchasesRequestDetailRepository.InsertAndGetIdAsync(newItemId);
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
        public async Task<long> Update(PurchasesRequestDetailListDto input)
        {
            PurchasesRequestDetail purchasesRequestDetail = await _puchasesRequestDetailRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, purchasesRequestDetail);
            await _puchasesRequestDetailRepository.UpdateAsync(purchasesRequestDetail);
            return input.Id;
        }






        /// <summary>
        /// Hàm lấy Id bản ghi phục vụ cho mục dích sửa hoặc lấy ra bản ghi cần dùng
        /// created : Kiên
        /// </summary>
        /// <param name="itemId">id bản ghi cần lấy </param>
        /// <returns></returns>
        public async Task<PurchasesRequestDetailListDto> GetAsync(EntityDto itemId)
        {
            var item = _puchasesRequestDetailRepository.Get(itemId.Id);
            PurchasesRequestDetailListDto newItem = ObjectMapper.Map<PurchasesRequestDetailListDto>(item);
            return newItem;
        }



        /// <summary>
        /// Hàm để xoá bản ghi bảng chi tiet yêu cầu mua hàng
        /// createdby : Kiên
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<int> Delete(int id)
        {
            try
            {
                await _puchasesRequestDetailRepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
        /// <summary>
        /// Cường
        /// </summary>
        /// <param name="input">id cảu bản ggi</param>
        /// <returns> object change quantity </returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<long> UpdateQuantity(PurchasesRequestDetailListDto input)
        {
            PurchasesRequestDetail purchasesRequestDetail = await _puchasesRequestDetailRepository.GetAsync(input.Id);
            purchasesRequestDetail.Quantity = input.Quantity;   
            await _puchasesRequestDetailRepository.UpdateAsync(purchasesRequestDetail);
            return input.Id;
        }
    }
}
