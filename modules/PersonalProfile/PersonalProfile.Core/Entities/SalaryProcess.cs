using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Entities
{
    [Table("AppSalaryProcesses", Schema = netcoreConsts.SchemaName)]
    public class SalaryProcess : AuditedEntity<int>
    {
        [Required]
        public int PersonId { get; set; }
        [Required]
        public string DecisionNumber { get;set; }
        [Required]
        public DateTime IssuedTime { get; set; }
        [Required]
        public DateTime SalaryIncreaseTime { get; set; }
        [Required]
        public string Glone { get; set; }
        [Required]
        public string GloneCode { get; set; }
        [Required]
        public string PayRate { get; set; }
        [Required]
        public string CoefficientsSalary { get; set; }
        public DateTime NextSalaryIncreaseTime { get; set; }
        [Required]
        [ForeignKey(nameof(PersonId))]
        public ProfileStaff ProfileStaff { get; set; }       
      
        public string LeadershipPositionAllowance { get; set; }    
       
        public string ToxicAllowance  { get; set; }  
      
        public string AreaAllowance { get; set; }      
       
        public string ResponsibilityAllowance  { get; set; }
     
        public string MobileAllowance  { get; set; }
    }
}
