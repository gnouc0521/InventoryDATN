using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.UploadFileProfiles.Dto
{
    [AutoMap(typeof(bbk.netcore.mdl.PersonalProfile.Core.Entities.UploadFile))]
    public class UploadFileProfileDto
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public string FileName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string FileUrl { get; set; }
        [AutoMapper.Configuration.Annotations.Ignore]
        public int Id { get; set; }
    }
}
