using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using AutoMapper.Configuration.Annotations;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using System.ComponentModel.DataAnnotations;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;
using bbk.netcore.mdl.PersonalProfile.Core.Utils;

namespace bbk.netcore.mdl.PersonalProfile.Application.PropertyDeclarations.Dto
{
    public class GetAllFilter
    {
        public int? PersonId { get; set; }
    }

    [AutoMap(typeof(PropertyDeclaration))]
    public class PropertyDeclarationDto : AuditedEntityDto<int>
    {
        public PropertyDeclarationDto(int Year, PropertyDeclarationBoolConst? isExist)
        {
            this.Year = Year;
            this.IsExist = isExist; 
            this.Title = EnumExtensions.GetDisplayName(this.IsExist);
        }
        [Required]
        public int PersonId { get; set; }

        [Required]
        public int Year { get; set; }

        public PropertyDeclarationBoolConst? IsExist { get; set; }

        public string FileUrl { get; set; }

        public string FilePath { get; set; }
        [Ignore]
        public string Title { get; set; }
        public ProfileStaff ProfileStaff { get; set; }
    }
}
