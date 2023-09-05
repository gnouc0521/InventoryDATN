using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Subsidiaries.Dto;
using bbk.netcore.mdl.OMS.Application.TransferDetails.Dto;
using bbk.netcore.mdl.OMS.Application.Transfers.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.TransferDetails
{
    public class TransferDetailAppService : ApplicationService, ITransferDetailAppService
    {
        private readonly IRepository<TransferDetail, long> _transferDetailRepository;
        private readonly IRepository<Transfer, long> _transferRepository;
        private readonly IRepository<Warehouse> _warehouseRepository;
        private readonly IRepository<Items,long> _itemsRepository;
        public TransferDetailAppService(
            IRepository<TransferDetail, long> transferDetailRepository,
            IRepository<Transfer, long> transferRepository,
            IRepository<Warehouse> warehouseRepository,
            IRepository<Items, long> itemsRepository)
        {
            _transferDetailRepository = transferDetailRepository;
            _transferRepository = transferRepository;
            _warehouseRepository = warehouseRepository;
            _itemsRepository = itemsRepository; 
        }

        public async Task<PagedResultDto<TransferDetailListDto>> GetAllItem()
        {
            try
            {
                var query = _transferDetailRepository.GetAll();

                var result = ObjectMapper.Map<List<TransferDetailListDto>>(query);

                return new PagedResultDto<TransferDetailListDto>(result.Count(), result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }

        public async Task<long> Create(TransferDetailCreateDto input)
        {
            try
            {
                TransferDetail newItemId = ObjectMapper.Map<TransferDetail>(input);
                var newId = await _transferDetailRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<TransferDetailListDto> GetAsync(EntityDto<long> itemId)
        {
            var item = _transferDetailRepository.Get(itemId.Id);
            TransferDetailListDto newItem = ObjectMapper.Map<TransferDetailListDto>(item);
            return newItem;
        }

        public async Task<PagedResultDto<TransferDetailListDto>> GetAll(TransferDetailsSearch input)
        {
            try
            {

                var query = _transferDetailRepository.GetAll().WhereIf(input.Id.HasValue, x => x.TransferId == input.Id).OrderByDescending(x => x.Id);
                var querytransfer = _transferRepository.GetAll().WhereIf(input.Id.HasValue, x => x.Id == input.Id)
                    .OrderByDescending(x => x.Id);
                var querywarehouse = _warehouseRepository.GetAll();
                var queryitems = _itemsRepository.GetAll();
                var ouput = (from detail in query 
                            join transfer in querytransfer on detail.TransferId equals transfer.Id 
                            join warehouse in querywarehouse on detail.IdWarehouseReceiving equals warehouse.Id 
                            join items in queryitems on detail.ItemId equals items.Id 
                            select new TransferDetailListDto
                            {
                               
                                WarehouseReceivingName = warehouse.Name,
                                ItemName = items.ItemCode + "-" + items.Name,
                                IdUnit = detail.IdUnit,
                                ItemCode = detail.ItemCode, 
                                QuotePrice = detail.QuotePrice,
                                QuantityTransfer = detail.QuantityTransfer,
                                QuantityInStock = detail.QuantityInStock,
                                UnitName = detail.UnitName, 
                                IdWarehouseReceiving  = detail.IdWarehouseReceiving,
                                Id = detail.Id,
                                TransferId = detail.TransferId,
                                ItemId = detail.ItemId,

                            }).ToList();    

               // var result = ObjectMapper.Map<List<TransferDetailListDto>>(query);

                return new PagedResultDto<TransferDetailListDto>(ouput.Count(), ouput);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }

        public async Task<long> Delete(long Id)
        {
            try
            {
                await _transferDetailRepository.DeleteAsync(Id);
                return Id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Update(TransferDetailListDto input)
        {
            try
            {
                TransferDetail supplier = await _transferDetailRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                supplier.IdWarehouseReceiving = input.IdWarehouseReceiving;
                supplier.QuantityTransfer = input.QuantityTransfer;
                supplier.QuantityInStock = input.QuantityInStock;

                //ObjectMapper.Map(input, supplier);
                await _transferDetailRepository.UpdateAsync(supplier);
                return input.Id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
