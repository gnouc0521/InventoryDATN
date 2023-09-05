using System.ComponentModel.DataAnnotations;

namespace bbk.netcore.mdl.PersonalProfile.Application.WorkingProcesses.Dto
{

    public class DeleteWorkingProcessDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}
