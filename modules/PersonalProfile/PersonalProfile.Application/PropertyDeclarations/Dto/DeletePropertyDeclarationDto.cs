using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.PropertyDeclarations.Dto
{

    public class DeletePropertyDeclarationDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int PersonId { get; set; }
    }
}
