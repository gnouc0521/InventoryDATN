using Abp.AutoMapper;
using bbk.netcore.mdl.OMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.OMS.Application.UploadFileCVs.Dto
{
    [AutoMap(typeof(UploadFileCV))]
    public class DelUploadFileDto
    {
        public int WorkId { get; set; }

        public int Id { get; set; } 

        public string FileName { get; set; }


        public string FilePath { get; set; }


        public string FileUrl { get; set; }


        public DateTime CreateTime { get; set; }

        public string CreateBy { get; set; }
    }
}
