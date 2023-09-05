using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Application.UploadFileProfiles.Dto
{
    public class DeleteFileDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }

    }
}
