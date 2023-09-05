using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.InventoryTicketDetails.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.InventoryTicketDetails
{
    [AbpAuthorize]
    public class InventoryTicketDetailService : ApplicationService, IInventoryTicketDetailService
    {
        private readonly IRepository<InventoryTicketDetail> _inventoryTicketDetail;
        private readonly IRepository<Items,long> _itemsRepository;
        private readonly IRepository<ImportRequestDetail> _importRequestsRepository;
        private readonly IRepository<Inventory, long> _ivenrepository;
        private readonly IRepository<InventoryTicket> _inventoryTicket;
        private readonly IRepository<ExportRequestDetail, long> _exprepository;
        public InventoryTicketDetailService(IRepository<InventoryTicketDetail> inventoryTicketDetail, IRepository<Items, long> itemsRepository, 
            IRepository<ImportRequestDetail> importRequestsRepository, 
            IRepository<Inventory, long> ivenrepository,
            IRepository<InventoryTicket> inventoryTicket,
            IRepository<ExportRequestDetail, long> exprepository)
        {
            _inventoryTicketDetail = inventoryTicketDetail;
            _itemsRepository = itemsRepository;
            _importRequestsRepository = importRequestsRepository;
            _ivenrepository = ivenrepository;
            _inventoryTicket = inventoryTicket;
            _exprepository = exprepository;
        }


        public async Task<PagedResultDto<InventoryTicketDetailListDto>> GetAll(GetInventoryTicketDetailInput input)
        {
            try
            {
                var itemsList = _itemsRepository.GetAll();
                var query = _inventoryTicketDetail.GetAll();
                var imp = _ivenrepository.GetAll();
                var iven =_inventoryTicket.GetAll().Where(x=>x.Id == input.InventoryTicketsId);
                var exp = _exprepository.GetAll();
                var result = (from i in query
                              join it in itemsList on i.ItemId equals it.Id
                              join im in imp on it.Id equals im.ItemId
                              join iv in iven on i.InventoryTicketId equals iv.Id
                              join ex in exp on i.ItemId equals ex.ItemId into t
                              from ex in t.DefaultIfEmpty()
                              where iv.WarehouseId == im.WarehouseId
                              select new InventoryTicketDetailListDto
                              {
                                  WarehouseSourceId= iv.WarehouseId,
                                  Id = i.Id,
                                  Quantity = i.Quantity,
                                  ItemId = i.ItemId,
                                  CodeItem= it.ItemCode,
                                  NameItem = it.Name,
                                  SoluongHT = im.Quantity,
                                  NameCode = it.ItemCode +"-"+it.Name,
                                  QuantityIN = im.Quantity,
                                  QuantityOUT = ex.Quantity,
                              });


                return new PagedResultDto<InventoryTicketDetailListDto>(
                  result.Distinct().Count(),
                  result.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Create(InventoryTicketDetailCreateDto input)
        {
            try
            {
                InventoryTicketDetail newItemId = ObjectMapper.Map<InventoryTicketDetail>(input);
                var newId = await _inventoryTicketDetail.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }



        public async Task<long> Update(InventoryTicketDetailListDto input)
        {
            InventoryTicketDetail IMP = await _inventoryTicketDetail.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, IMP);
            await _inventoryTicketDetail.UpdateAsync(IMP);
            return input.Id;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _inventoryTicketDetail.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<InventoryTicketDetailListDto> GetAsync(EntityDto itemId)
        {
            var item = _inventoryTicketDetail.Get(itemId.Id);
            InventoryTicketDetailListDto newItem = ObjectMapper.Map<InventoryTicketDetailListDto>(item);
            return newItem;
        }
    }
}
