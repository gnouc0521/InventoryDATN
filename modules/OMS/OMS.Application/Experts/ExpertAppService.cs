using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using bbk.netcore.Authorization.Roles;
using bbk.netcore.Authorization.Users;
using bbk.netcore.Authorization.Users.Dto;
using bbk.netcore.mdl.OMS.Application.Experts.Dto;
using bbk.netcore.mdl.OMS.Application.Itemses.Dto;
using bbk.netcore.mdl.OMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbk.netcore.mdl.OMS.Application.Experts
{
    [AbpAuthorize]
    public class ExpertAppService : ApplicationService, IExpertAppService
    {
        private readonly IRepository<Expert> _expertrepository;
        private readonly IRepository<Items, long> _Itemsrepository;
        private readonly IRepository<Assignment> _assignmentrepository;
        private readonly IUserAppService _userAppService;
        public ExpertAppService(IRepository<Expert> expertrepository,IUserAppService userAppService,
            IRepository<Assignment> assignmentrepository, IRepository<Items, long> itemsrepository)
        {
            _expertrepository = expertrepository;
            _userAppService = userAppService;
            _assignmentrepository = assignmentrepository;
            _Itemsrepository = itemsrepository;
        }

        public async Task<int> Create(ExpertCreateDto input)
        {
            try
            {
                //Warehouse warehouse = ObjectMapper.Map<Warehouse>(input);
                //var newId = await _warehouserepository.InsertAndGetIdAsync(warehouse);
                //return newId;

                var query = await _expertrepository.GetAll().ToListAsync();
                string sinhma(string ma)
                {
                    string s = ma.Substring(4, ma.Length - 4);

                    int i = int.Parse(s);
                    i++;
                    if (i < 10) return "CV-" + "00" + Convert.ToString(i);
                    else
                    if (i >= 10 && i < 100) return "CV-" + "0" + Convert.ToString(i);
                    else return "CV-" + Convert.ToString(i);

                }
                string ma;

                var count = query.Count;
                if (count == 0)

                {
                    ma = "0000000000";
                }
                else
                {
                    ma = _expertrepository.GetAll().OrderByDescending(x => x.ExpertCode).Select(x => x.ExpertCode).ToList().First();
                }

                input.ExpertCode = sinhma(ma.ToString());
                Expert newItemId = ObjectMapper.Map<Expert>(input);
                var newId = await _expertrepository.InsertAndGetIdAsync(newItemId);
                return newId;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ExpertListDto>> GetAll(GetExpertInput input)
        {
            try
            {
                var query = _expertrepository.GetAll()
                    .WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.Name.ToLower().Contains(input.SearchTerm.ToLower()) || x.ExpertCode.ToLower().Contains(input.SearchTerm.ToLower()))
                    //.WhereIf(!string.IsNullOrEmpty(input.SearchTerm), x => x.ExpertCode.Contains(input.SearchTerm))
                    .OrderBy(x => x.Id);

                var count = query.Count();

                var expertList = ObjectMapper.Map<List<ExpertListDto>>(query);
                return new PagedResultDto<ExpertListDto>(
                    count,
                    expertList
                    );

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<UserListDto>> GetAllUser()
        {
            try
            {
                var text = "Chuyên viên";
                GetUsersInput getUsersInput = new GetUsersInput();
                getUsersInput.MaxResultCount = 1000;
                var users = _userAppService.GetUsers(getUsersInput).Result.Items;
             

                var expertlist = _expertrepository.GetAll().Select(x => x.UserId).ToArray();
                var query = users.Where(x => !expertlist.Contains(x.Id)).ToList();
                List<UserListDto> listDtos = new List<UserListDto>();
                for (int i = 0; i < query.Count(); i++)
                {
                    for (int j = 0; j < query[i].Roles.Count(); j++)
                    {
                        if (query[i].Roles[j].RoleName.ToLower() == text.ToLower().Trim())
                        {
                            listDtos.Add(query[i]);
                        }
                    }
                }

                //var result = ObjectMapper.Map<List<UserListDto>>(query);

                return new PagedResultDto<UserListDto>(listDtos.Count(), listDtos);

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ItemsListDto>> GetAllItem()
        {
            try
            {
                var itemList = _Itemsrepository.GetAll().ToList();
                var expertitem = _assignmentrepository.GetAll().Select(x => x.ItemId).ToArray();
                var query = itemList.Where(x => !expertitem.Contains(x.Id)).ToList();

                var result = ObjectMapper.Map<List<ItemsListDto>>(query);

                return new PagedResultDto<ItemsListDto>(
                    result.Count(), result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ExpertListDto> GetAsync(EntityDto Id)
        {
            try
            {
                var query = _expertrepository.Get(Id.Id);
                ExpertListDto result = ObjectMapper.Map<ExpertListDto>(query);
                return result;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<int> Update(ExpertEditDto input)
        {
            try
            {
                Expert expert = await _expertrepository.FirstOrDefaultAsync(x => x.Id == input.Id);
                input.ExpertCode = expert.ExpertCode;
                input.Name = expert.Name;
                input.Email = expert.Email;
                ObjectMapper.Map(input, expert);
                await _expertrepository.UpdateAsync(expert);
                return input.Id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ItemsListDto>> GetAllItemExpert(int Id)
        {
            try
            {
                var itemList = _Itemsrepository.GetAll().ToList();
                List<Items> listItem = new List<Items>();

                //Lay het hang hoa chua dc phan cong 
                var expertitem = _assignmentrepository.GetAll().Select(x => x.ItemId).ToArray();
                var query = itemList.Where(x => !expertitem.Contains(x.Id)).ToList();
                listItem.AddRange(query);


                //Lay hang hoa dc phan cong cua chuyen vien do
                var itemsEx = _assignmentrepository.GetAll().Where(x => x.UserId == Id).Select(x => x.ItemId).ToArray();
                var query1 = itemList.Where(x => itemsEx.Contains(x.Id)).ToList();
                listItem.AddRange(query1);


                var result = ObjectMapper.Map<List<ItemsListDto>>(listItem);

                return new PagedResultDto<ItemsListDto>(
                    result.Count(), result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);

            }
        }


        public async Task<PagedResultDto<ExpertListDto>> GetAllUserALL()
        {
            try
            {
                var query = _expertrepository.GetAll()
                    .OrderBy(x => x.Id);

                var count = query.Count();

                var expertList = ObjectMapper.Map<List<ExpertListDto>>(query);
                return new PagedResultDto<ExpertListDto>(
                    count,
                    expertList
                    );


            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

    }
}
