using Abp.AutoMapper;
using Abp.Domain.Entities;
using bbk.netcore.mdl.OMS.Core;
using bbk.netcore.mdl.OMS.Core.Entities;
using bbk.netcore.mdl.OMS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace bbk.netcore.mdl.OMS.Application.Works.Dto
{
    [AutoMap(typeof(Work))]
    public class WorkListDto : Entity<long>
    {
        public long? Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string WorkGroup { get; set; }
      
        public DateTime StartDate { get; set; }
       
        public DateTime EndDate { get; set; }
        public DateTime CompletionTime { get; set; }
        public WorkEnum.Status Status { get; set; }

        public WorkEnum.Priority priority { get; set; }

        public WorkEnum.JobLabel jobLabel { get; set; }
        public WorkEnum.OwnerStatusEnum OwnerStatusEnum { get; set; }

        public long UserId { get; set; }
        
        public int HostId { get; set; }
        public string HostName { get; set; }
        public int fileNum { get; set; }
        public int IdWorkGroup { get; set; }
        public int IdProfileWork { get; set; }
        public string WorkUser { get; set; }
        public string HostUser { get; set; }

        public string FileUrl { get; set; }

        public string FilePath { get; set; }

        public IFormFile[] FileUpload { get; set; }


    }
}
