using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Users;
using bbk.netcore.mdl.OMS.Application.ExportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.WarehouseCardsDetail.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WarehouseCardsDetail
{
    public class WarehouseCardsDetailAppService : ApplicationService, IWarehouseCardsDetailAppService
  {
        private readonly IRepository<WarehouseCardDetail, long> _WarehouseCardsDetailRepository;
        public WarehouseCardsDetailAppService(IRepository<WarehouseCardDetail, long> WarehouseCardsDetailAppService
           )
        {
            _WarehouseCardsDetailRepository = WarehouseCardsDetailAppService;
        }

        public async Task<long> Create(WarehouseCardsDetailCreate input)
        {
            try
            {
                WarehouseCardDetail newItemId = ObjectMapper.Map<WarehouseCardDetail>(input);
                var newId = await _WarehouseCardsDetailRepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<long> Delete(long id)
        {
            try
            {
                await _WarehouseCardsDetailRepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }


      
      
        public async Task<long> Update(WarehouseCardsDetailCreate input)
        {
             WarehouseCardDetail items = await _WarehouseCardsDetailRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, items);
            await _WarehouseCardsDetailRepository.UpdateAsync(items);
            return items.Id;
        }

     

    }
}
