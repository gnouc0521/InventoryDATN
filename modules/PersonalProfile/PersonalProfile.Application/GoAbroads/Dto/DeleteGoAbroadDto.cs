using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.GoAbroads.Dto
{

    public class DeleteGoAbroadDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int PersonId { get; set; }
    }
}
