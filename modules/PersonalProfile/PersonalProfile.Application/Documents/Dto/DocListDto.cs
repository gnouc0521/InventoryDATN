using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using AutoMapper.Configuration.Annotations;
using bbk.netcore.mdl.PersonalProfile.Application.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto
{
    [AutoMap(typeof(Document))]
    public class DocListDto : EntityDto<long>
    {
        public long? Id { get; set; }

        public string DecisionNumber { get; set; }

        public string Description { get; set; }

        public string DocumentTypeName { get; set; }

        public string IssuedOrganization { get; set; }

        public DateTime IssuedDate { get; set; }
        [Required]
        public int DocumentCategoryTypeId { get; set; }

        public DocumentEnum.DocumentCategoryEnum? DocumentCategoryEnum { get; set; }

        public DocumentEnum.DocumentTypeEnum? DocumentTypeEnum { get; set; }

        public string FileUrl { get; set; }

        public string FilePath { get; set; }

        public FileDto FileUrlInfo { get { return new FileDto(FileUrl); } }
    }
}
