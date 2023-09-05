using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AutoMapper.Configuration.Annotations;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Application.Categories.Dto
{
    [AutoMap(typeof(Category))]
    public class CategoryDto : EntityDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public CategoryType CategoryType { get; set; }
        [Ignore]
        public string CategoryName { get; set; }
        [Ignore]
        public string Translate { get; set; }
        public CategoryDto()
        {
            
        }
        public CategoryDto(CategoryType categoryType)
        {
            Translate = EnumExtensions.GetDisplayName(categoryType);
            CategoryName = categoryType.ToString();
            this.CategoryType = categoryType;
        }

    }
}
