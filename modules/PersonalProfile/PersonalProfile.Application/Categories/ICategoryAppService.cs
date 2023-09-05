using Abp.Application.Services;
using Abp.Application.Services.Dto;
using bbk.netcore.mdl.PersonalProfile.Application.Categories.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Categories
{
    public class Filter
    {
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
      
        public string Keyword { get; set; }
        public  CategoryType categoryType { get; set; }
    }

    public interface ICategoryAppService : IApplicationService
    {
        Task<ListResultDto<CategoryDto>> GetListByType(StatusEnum.CategoryType type);
        Task<ListResultDto<CategoryDto>> GetList(Filter filter);
        Task<CategoryDto> GetCategorybyId(int id);
        Task<List<CategoryDto>> GetListByMultiType(List<CategoryType> categoryTypes);
        Task<List<CategoryDto>> GetListPosition();
        Task<int> GetIdCategory(int id);
       
    }
}
