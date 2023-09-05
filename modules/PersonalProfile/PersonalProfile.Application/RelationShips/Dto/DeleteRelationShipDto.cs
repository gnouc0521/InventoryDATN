using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.RelationShips.Dto
{

    public class DeleteRelationShipDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int PersonId { get; set; }
    }
}
