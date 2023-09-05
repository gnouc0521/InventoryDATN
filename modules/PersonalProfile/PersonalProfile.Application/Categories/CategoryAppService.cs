using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using bbk.netcore.mdl.PersonalProfile.Application.Categories.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using Abp.UI;
using Abp.Authorization;
using bbk.netcore.Authorization;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;

namespace bbk.netcore.mdl.PersonalProfile.Application.Categories
{
    [AbpAuthorize]
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryAppService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }
        public async Task<ListResultDto<CategoryDto>> GetListByType(StatusEnum.CategoryType type)
        {
            var list = await _categoryRepository.GetAll().Where(u => u.CategoryType == type).OrderBy(u => u.Title).ToListAsync();
            return new ListResultDto<CategoryDto>(ObjectMapper.Map<List<CategoryDto>>(list));
        }
        public async Task<CategoryDto> GetCategorybyId(int id)
        {
            try
            {
                var entity = await _categoryRepository.FirstOrDefaultAsync(id);
                if (entity == null)
                {
                    throw new Exception("Thông tin danh mục không tồn tại!");
                }
                CategoryDto categoryDto = ObjectMapper.Map<CategoryDto>(entity);
                return categoryDto;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        public async Task<List<CategoryDto>> GetListPosition()
        {
            try
            {
                var query = await _categoryRepository.GetAll().Where(u => u.CategoryType == StatusEnum.CategoryType.LeadershipTitle || u.CategoryType == StatusEnum.CategoryType.ProfessionalTitle
                || u.CategoryType == StatusEnum.CategoryType.LaborContract).OrderBy(u => u.Title).ToListAsync();
                return ObjectMapper.Map<List<CategoryDto>>(query);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        public async Task<ListResultDto<CategoryDto>> GetList(Filter filter)
        {
            if (filter.categoryType.ToString() != "All")
            {
                var list = _categoryRepository.GetAll()
                 .WhereIf(!string.IsNullOrEmpty(filter.Keyword), t => t.Title.Contains(filter.Keyword))
                 .Where(t => t.CategoryType == filter.categoryType)
                 .OrderByDescending(t => t.CreationTime);
                var lst = await list.PageBy(filter.SkipCount, filter.MaxResultCount).ToListAsync();
                int totalCount = list.Count();
                PagedResultDto<CategoryDto> result = new PagedResultDto<CategoryDto> { TotalCount = totalCount, Items = ObjectMapper.Map<List<CategoryDto>>(lst) };
                return result;
            }
            else
            {
                var list = _categoryRepository.GetAll()
                 .WhereIf(!string.IsNullOrEmpty(filter.Keyword), t => t.Title.Contains(filter.Keyword))
                 .OrderByDescending(t => t.CreationTime);
                var lst = await list.PageBy(filter.SkipCount, filter.MaxResultCount)
                .ToListAsync();
                int totalCount = list.Count();
                PagedResultDto<CategoryDto> result = new PagedResultDto<CategoryDto> { TotalCount = totalCount, Items = ObjectMapper.Map<List<CategoryDto>>(lst) };
                return result;
            }


        }
        public async Task<int> GetIdCategory(int id)
        {
            int IdCategory = 100;
            var category = _categoryRepository.Get(id);
            if (category != null)
            {
                Category cate = new Category();
                cate.CategoryType = category.CategoryType;
                IdCategory = (int)cate.CategoryType;
            }
            return IdCategory;
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Categories)]
        public async Task Create(CategoryDto categoryDto)
        {
            try
            {
                string title = categoryDto.Title.Trim();
                while (title.Contains("  ")) //2 khoảng trắng
                {
                    title = title.Replace("  ", " "); //Replace 2 khoảng trắng thành 1 khoảng trắng
                }
                //Category cate = new Category();
                //cate.CategoryType = categoryDto.CategoryType;
                //int numCategory=(int)
                var check = await _categoryRepository.GetAll().
                    Where(n => n.CategoryType == categoryDto.CategoryType && n.Title.ToLower() == title.ToLower()).ToListAsync();
                if (check.Count() > 0)
                {
                    throw new Exception("Loại danh mục đã tồn tại!");
                }
                categoryDto.Title = title;
                var entity = ObjectMapper.Map<Category>(categoryDto);
                await _categoryRepository.InsertAsync(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Categories)]
        public async Task Update(CategoryDto categoryDto)
        {
            try
            {
                string title = categoryDto.Title.Trim();
                while (title.Contains("  ")) //2 khoảng trắng
                {
                    title = title.Replace("  ", " "); //Replace 2 khoảng trắng thành 1 khoảng trắng
                }
                var check = await _categoryRepository.GetAll().
                    Where(n => n.CategoryType == categoryDto.CategoryType && n.Title.ToLower() == title.ToLower()).ToListAsync();
                if (check.Count() > 0)
                {
                    throw new Exception("Loại danh mục đã tồn tại!");
                }
                int id = categoryDto.Id;
                var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    throw new Exception("Danh mục cập nhật không đúng!");
                }
                ObjectMapper.Map(categoryDto, category);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        [AbpAuthorize(PersonalProfilePermission.Pages_PPS_Categories)]
        public async Task DeleteById(int id)
        {
            try
            {

                var category = await _categoryRepository.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    throw new Exception("Danh mục không tồn tại!");
                }
                await _categoryRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message + " -Hoặc do lỗi ràng buộc nên không thể xóa danh mục này!");
            }
        }
        public async Task<List<CategoryDto>> GetListByMultiType(List<CategoryType> categoryTypes)
        {
            var list = await _categoryRepository.GetAllListAsync(x => categoryTypes.Contains(x.CategoryType));
            var listDto = ObjectMapper.Map<List<CategoryDto>>(list);
            foreach (var l in listDto)
            {
                l.CategoryName = EnumExtensions.GetDisplayName(l.CategoryType);
            }
            return listDto;
        }
    }
}