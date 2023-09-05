using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;


namespace bbk.netcore.mdl.OMS.Application.Ruleses.Dto
{
    [AutoMap(typeof(Rules))]
    public class RulesListDto : EntityDto
    {
        public string ItemKey { get; set; }
        public int Order { get; set; }
        public string ItemText { get; set; }
        public string ItemValue { get; set; }
        public string Culture { get; set; }
        public string Remark { get; set; }
    }
}
