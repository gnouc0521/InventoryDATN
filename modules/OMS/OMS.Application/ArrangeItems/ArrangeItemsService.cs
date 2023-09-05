using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.ArrangeItems.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ArrangeItems
{
    public class ArrangeItemsService : ApplicationService, IArrangeItemsService
    {
        private readonly IRepository<WarehouseLocationItems, long> _warehouseLocationItemsRepository;

        public ArrangeItemsService(IRepository<WarehouseLocationItems, long> warehouseLocationItemsRepository)
        {
            _warehouseLocationItemsRepository = warehouseLocationItemsRepository;
        }
        public async Task<PagedResultDto<ArrangeItemsListDto>> GetAll()
        {
            try
            {
                var query = _warehouseLocationItemsRepository.GetAll().ToList();
                var page = query.Count();
                var output = ObjectMapper.Map<List<ArrangeItemsListDto>>(query);
                return new PagedResultDto<ArrangeItemsListDto>(
                    page,
                    output
                    );
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}
