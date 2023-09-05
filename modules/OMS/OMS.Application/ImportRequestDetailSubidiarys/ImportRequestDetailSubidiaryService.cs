using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.ImportRequestDetailSubidiarys.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.ImportRequestDetailSubidiarys
{
    [Authorize]
    public class ImportRequestDetailSubidiaryService : ApplicationService, IImportRequestDetailSubidiaryService
    {
        private readonly IRepository<ImportRequestSubsidiary> _importRequestSub;
        private readonly IRepository<Items, long> _items;
        private readonly IRepository<ImportRequestSubsidiaryDetail> _importRequestSetailSub;

        public ImportRequestDetailSubidiaryService(
            IRepository<ImportRequestSubsidiary> importRequestSub , 
            IRepository<Items, long> items, 
            IRepository<ImportRequestSubsidiaryDetail> importRequestSetailSub)
        {
            _importRequestSetailSub= importRequestSetailSub;
            _items= items;
            _importRequestSub= importRequestSub;
        }

        public async Task<PagedResultDto<ImportRequestDetailSubListDto>> GetAll(GetInput input)
        {
            try
            {
                var itemsList = _items.GetAll();
                var query = _importRequestSetailSub
                      .GetAll()
                      .Where(x => x.ImportRequestSubsidiaryId == input.importRequesSubtId)
                      .ToList()
                      .OrderBy(x => x.Id);
                var IMPDCount = query.Count();
                var result = (from i in query
                              join it in itemsList on i.ItemId equals it.Id
                              select new ImportRequestDetailSubListDto
                              {
                                  Id = i.Id,
                                  Itemcode = it.ItemCode +"-"+it.Name,
                                  UnitId = i.UnitId,
                                  UnitName = i.UnitName,
                                  Price =i.Price,
                                  Quantity = i.Quantity,
                                  ItemId = i.ItemId,
                                  TimeNeeded = i.TimeNeeded,
                              }).ToList();


                return new PagedResultDto<ImportRequestDetailSubListDto>(
                  result.Distinct().Count(),
                  result.Distinct().ToList()
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        public async Task<long> Create(ImportRequestDetailSubCreateDto input)
        {
            try
            {
                ImportRequestSubsidiaryDetail newItemId = ObjectMapper.Map<ImportRequestSubsidiaryDetail>(input);
                var newId = await _importRequestSetailSub.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }



        public async Task<long> Update(ImportRequestDetailSubListDto input)
        {
            ImportRequestSubsidiaryDetail IMP = await _importRequestSetailSub.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, IMP);
            await _importRequestSetailSub.UpdateAsync(IMP);
            return input.Id;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                await _importRequestSetailSub.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ImportRequestDetailSubListDto> GetAsync(EntityDto itemId)
        {
            var item = _importRequestSetailSub.Get(itemId.Id);
            ImportRequestDetailSubListDto newItem = ObjectMapper.Map<ImportRequestDetailSubListDto>(item);
            return newItem;
        }

        public async Task<PagedResultDto<ImportRequestDetailSubListDto>> GetAllItem()
        {
            try
            {
                var query = _importRequestSetailSub.GetAll();

                var result = ObjectMapper.Map<List<ImportRequestDetailSubListDto>>(query);

                return new PagedResultDto<ImportRequestDetailSubListDto>(result.Count(), result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }

    }
}
