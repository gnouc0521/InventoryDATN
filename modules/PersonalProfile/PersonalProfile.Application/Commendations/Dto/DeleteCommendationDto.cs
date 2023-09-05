using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.Commendations.Dto
{

    public class DeleteCommendationDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int PersonId { get; set; }
    }
}
