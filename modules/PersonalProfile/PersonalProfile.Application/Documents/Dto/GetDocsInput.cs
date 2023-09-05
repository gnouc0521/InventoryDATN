using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using bbk.netcore.Dto;
using bbk.netcore.mdl.PersonalProfile.Core.Enums;

namespace bbk.netcore.mdl.PersonalProfile.Application.Documents.Dto
{
    public class GetDocsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        //public string Filter { get; set; }

        public string SearchTerm { get; set; }

        //public int? Role { get; set; }

        //public DateTime? StartIssuedDate { get; set; }

        //public DateTime? EndIssuedDate { get; set; }

        public DocumentEnum.DocumentCategoryEnum? DocumentCategoryEnum { get; set; }

        public int? DocumentCategoryTypeId { get; set; }

        public DocumentEnum.DocumentTypeEnum? DocumentTypeEnum { get; set; }
        

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "IssuedDate";
            }
        }
    }
}
