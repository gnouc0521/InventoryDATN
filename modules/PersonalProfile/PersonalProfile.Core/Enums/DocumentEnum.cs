using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Core.Enums
{
    public class DocumentEnum
    {
        public enum DocumentCategoryEnum
        {
            [Display(Name = "Tổ chức bộ máy và biên chế")]
            OrgAndPayroll = 1,
            [Display(Name = "Quy hoạch cán bộ")]
            ZoningStaff = 2,
            [Display(Name = "Kế hoạch tuyển dụng")]
            RecruitmentPlan = 3,
            [Display(Name = "Công tác đánh giá cán bộ")]
            StaffAssessment = 4,
            [Display(Name = "Thi đua khen thưởng")]
            EmulationAndReward = 5,
            [Display(Name = "Cải cách hành chính")]
            AdministrativeReform = 6,
            [Display(Name = "Quy chế dân chủ")]
            DemocraticRegulation = 7,
            [Display(Name = "Đi nước ngoài")]
            GoAbroad = 8,
            [Display(Name = "Công tác Đảng")]
            CommunistPartyProcess = 9
        }

        [Flags]
        public enum DocumentTypeEnum
        {
            //None = 0,
            LawOrdinance = 1,           //Luật - Pháp lệnh
            Constitution = 2,             //Hiến pháp
            Ordinance = 4,          //Sắc lệnh - Sắc luật
            Decree = 8,                    //Nghị định
            Decision = 16,                  //Quyết định
            Circulars = 32,                //Thông tư
            Documentary = 64,              //Công văn
            Resolution = 128,              //Nghị quyết
            Directive = 256,        //Chỉ thị
            JointCircular = 512,           //Thông tư liên tịch 
            FormDocument = 1024           //Biểu mẫu
        }
    }
}
