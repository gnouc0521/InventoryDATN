using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.StaffPlainnings.Dto
{

    public class DeleteStaffPlainningDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int PersonId { get; set; }
    }
}
