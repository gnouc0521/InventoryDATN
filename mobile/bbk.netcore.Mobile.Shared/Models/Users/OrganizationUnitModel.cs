using Abp.AutoMapper;
using bbk.netcore.Organizations.Dto;

namespace bbk.netcore.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}

