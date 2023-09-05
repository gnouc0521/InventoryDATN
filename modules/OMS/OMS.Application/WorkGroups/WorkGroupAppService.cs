using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.WorkGroups.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.WorkGroups
{
    [AbpAuthorize]
    public class WorkGroupAppService : ApplicationService, IWorkGroupAppService
    {
        private readonly IRepository<WorkGroup> _workGrouprepository;
        public WorkGroupAppService(IRepository<WorkGroup> workGrouprepository)
        {
            _workGrouprepository = workGrouprepository;
        }

        public async Task<int> Create(WordGroupCreateDto input)
        {
            try
            {
                WorkGroup newItemId = ObjectMapper.Map<WorkGroup>(input);
                var newId = await _workGrouprepository.InsertAndGetIdAsync(newItemId);
                return newId;
                //var query = _workGrouprepository.GetAll().Count();
                //var MaxOrder = _workGrouprepository.GetAll().Where(x => x.Order > 0).Max(x => x.Order);
                //WorkGroup workGroup = new WorkGroup();
                //if(query == 0)
                //{
                //    workGroup.Title = input.Title;
                //    workGroup.ParentId = input.ParentId;
                //    workGroup.Order = 1;
                //    workGroup.WorkGroupLevel = 1;
                //    var newId = await _workGrouprepository.InsertAndGetIdAsync(workGroup);
                //    return newId;
                //}
                //else
                //{
                //    workGroup.Title = input.Title;
                //    workGroup.ParentId = input.ParentId;
                //    workGroup.Order = MaxOrder + 1;
                //    workGroup.WorkGroupLevel = 1;
                //    var newId = await _workGrouprepository.InsertAndGetIdAsync(workGroup);
                //    return newId;
                //}
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        public async Task<int> Delete(int id)
        {
            try
            {
                await _workGrouprepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WorkGroupListDto>> GetAll(GetWorkGroupInput input)
        {
            try
            {
                
                var query =  _workGrouprepository
                      .GetAll()
                      .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), u => u.Title.Contains(input.SearchTerm))
                      .WhereIf(input.IdWorkGroup != null, u => u.Id.Equals(input.IdWorkGroup))
                      .OrderBy(x => x.Order);

                if (input.IdWorkGroup == null)
                {
                    var queryRoot = query.Where(x => x.ParentId == null);
                    var worksCount = queryRoot.Count();
                    var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(queryRoot);
                    return new PagedResultDto<WorkGroupListDto>(
                      worksCount,
                      WorkListDto
                      );
                }
                else
                {
                    var test =  query.Where(x => x.ParentId == null);
                    var isParentId = from c in test
                                     select c.Id;
                    if (test.Count() == 0)
                    {
                        var worksCount = query.Count();
                        var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                        return new PagedResultDto<WorkGroupListDto>(
                          worksCount,
                          WorkListDto
                          );
                    }
                    else
                    {
                        var query2 =  _workGrouprepository.GetAll().Where(x => x.ParentId == isParentId.ToArray()[0]);
                       
                        var worksCount = query2.Count();
                        var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query2);
                        return new PagedResultDto<WorkGroupListDto>(
                          worksCount,
                          WorkListDto
                          );
                    }
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WorkGroupListDto>> GetAllList()
        {
            try
            {
                var query = _workGrouprepository
                      .GetAll();
                var worksCount = query.Count();


                var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<WorkGroupListDto>(
                  worksCount,
                  WorkListDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WorkGroupListDto>> GetAllListParent()
        {
            try
            {
                var query = _workGrouprepository.GetAll().Where(x => x.ParentId == null);
                var query1 = _workGrouprepository.GetAll().Where(x => x.ParentId != null);

                var query2 = (from i in query
                              let count = (from z in query1 where i.Id == z.ParentId select z.Id).Count()
                              select new WorkGroupListDto
                              {
                                  Id = i.Id,
                                  Title = i.Title,
                                  Order = i.Order,
                                  NumItemsChild = count,
                                  ParentId = i.ParentId,
                              }).ToList();

                //var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<WorkGroupListDto>(
                  query2.Count(),
                  query2
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<WorkGroupListDto>> GetAllListItem(int id)
        {
            try
            {
                var query = _workGrouprepository.GetAll().Where(x => x.ParentId != null && x.ParentId == id);
                var query1 = _workGrouprepository.GetAll().Where(x => x.ParentId != null);

                var query2 = (from i in query
                              let count = (from z in query1 where i.Id == z.ParentId select z.Id).Count()
                              select new WorkGroupListDto
                              {
                                  Id = i.Id,
                                  Title = i.Title,
                                  Order = i.Order,
                                  NumItemsChild = count,
                                  ParentId = i.ParentId,
                              }).ToList();


                //var WorkListDto = ObjectMapper.Map<List<WorkGroupListDto>>(query);
                return new PagedResultDto<WorkGroupListDto>(
                  query2.Count(),
                  query2
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        
        public async Task<PagedResultDto<WorkGroupListDto>> GetAsync(EntityDto itemId)
        {
            var item = _workGrouprepository.GetAll().Where(x=>x.Id==itemId.Id);
            var query1 = _workGrouprepository.GetAll().Where(x => x.ParentId != null);

            var query2 = (from i in item
                          let count = (from z in query1 where i.Id == z.ParentId select z.Id).Count()
                          select new WorkGroupListDto
                          {
                              Id = i.Id,
                              Title = i.Title,
                              Order = i.Order,
                              NumItemsChild = count,
                              ParentId = i.ParentId,
                              WorkGroupLevel = i.WorkGroupLevel,
                          }).OrderByDescending(x => x.Id).ToList();

            //var newItem = ObjectMapper.Map<List<WorkGroupListDto>>(item);
            return new PagedResultDto<WorkGroupListDto>(
                  query2.Count(),
                  query2
                  );
        }

        public async Task<int> Update(WorkGroupEditDto input)
        {
            WorkGroup workGroup = await _workGrouprepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, workGroup);
            await _workGrouprepository.UpdateAsync(workGroup);
            return input.Id;
        }
    }
}
