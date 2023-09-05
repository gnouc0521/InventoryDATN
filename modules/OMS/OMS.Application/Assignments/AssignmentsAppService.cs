using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.mdl.OMS.Application.Assignments.Dto;
using bbk.netcore.mdl.OMS.Application.ImportRequests.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Assignments
{
    [AbpAuthorize]
    public class AssignmentsAppService : ApplicationService, IAssignmentsAppService
    {
        private readonly IRepository<Assignment> _assignmentrepository;
        private readonly IRepository<Items, long> _Itemsrepository;
        private readonly IRepository<Producer> _producerrepository;

        public AssignmentsAppService(IRepository<Assignment> assignmentrepository, IRepository<Items, long> itemsrepository, IRepository<Producer> producerrepository)
        {
            _assignmentrepository = assignmentrepository;
            _Itemsrepository = itemsrepository;
            _producerrepository = producerrepository;
        }

        public async Task<int> Create(AssignmentsListDto input)
        {
            try
            {
                var newItem = ObjectMapper.Map<Assignment>(input);
                newItem.CreatorUserId = AbpSession.UserId;
                var itemSubject = await _assignmentrepository.InsertAsync(newItem);
                return itemSubject.Id;
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
                await _assignmentrepository.DeleteAsync(id);
                return id;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<int> DeletebyItem(int idtem)
        {
            try
            {
                var query = _assignmentrepository.GetAll().Where(x => x.ItemId == idtem).Select(x => x.Id).ToArray();
                await _assignmentrepository.DeleteAsync(query[0]);
                return query[0];
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<AssignmentsListDto>> GetAll()
        {
            try
            {
                var query = _assignmentrepository.GetAll().ToList();
                var count = query.Count();

                var assiList = ObjectMapper.Map<List<AssignmentsListDto>>(query);
                return new PagedResultDto<AssignmentsListDto>(count, assiList);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<GetAssignmentsListDto>> GetAllByUser(int Id)
        {
            try
            {
                var query = _assignmentrepository.GetAll().Where(x => x.UserId == Id).ToList();
                var result = ObjectMapper.Map<List<GetAssignmentsListDto>>(query);

                return new PagedResultDto<GetAssignmentsListDto>(
                    result.Count(), result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }

        public async Task<PagedResultDto<ItemsListDto>> GetAllItemByUserId(long userId)
        {
            try
            {
                var assign = _assignmentrepository.GetAll().Where(x => x.UserId == userId).Select(x => x.ItemId).Distinct();
                var items = _Itemsrepository.GetAll();
                var querysupplier = _producerrepository.GetAll();

                var query = (from asi in assign
                             join item in items on asi equals item.Id
                             join product in querysupplier on item.ProducerCode equals product.Code into gj
                             from subpet in gj.DefaultIfEmpty()
                             select new ItemsListDto
                             {
                                 Id = item.Id,
                                 ItemCode = item.ItemCode,
                                 Name = item.ItemCode + "-"+ item.Name,
                                 ProducerCode = item.ProducerCode,
                                 SupplierName = subpet.Name ?? string.Empty
                             }).ToList();

                return new PagedResultDto<ItemsListDto>(
                    query.Count(), query
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }


        //kiebn
        public async Task<long> Update(AssignmentsListDto input)
        {
            var ass = _assignmentrepository.GetAll().Where(x => x.ItemId == input.ItemId).ToList();
            foreach (var item in ass)
            {
                input.Id = item.Id;
                input.CreationTime = item.CreationTime;
                input.CreatorUserId = item.CreatorUserId;
                ObjectMapper.Map(input, item);
                await _assignmentrepository.UpdateAsync(item);
            }
            return input.Id;
            //return input.Id;
            //Assignment ass = await _assignmentrepository.FirstOrDefaultAsync(x => x.ItemId == input.ItemId);
            //input.Id = ass.Id;
            //input.UserId= ass.UserId;
            //input.CreationTime = ass.CreationTime;
            //input.CreatorUserId = ass.CreatorUserId;
            //ObjectMapper.Map(input, ass);
            //await _assignmentrepository.UpdateAsync(ass);
            //return input.Id;
        }

    }
}
