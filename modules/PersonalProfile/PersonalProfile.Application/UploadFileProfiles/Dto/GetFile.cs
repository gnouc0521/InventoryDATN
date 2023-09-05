using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.UploadFileProfiles.Dto
{
    public class GetFile
    {
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileUrl { get; set; }
    }
}
