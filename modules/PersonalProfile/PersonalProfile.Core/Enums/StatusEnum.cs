using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Core.Enums
{
    public class StatusEnum
    {
        public enum AssessedByYearEnum : byte
        {
            [Display(Name = "Công chức")]
            CivilServant = 0,
            [Display(Name = "Viên chức")]
            PublicServant = 1,
            [Display(Name = "Hợp đồng lao động")]
            LaborContract = 2,
        }
        public enum FluctuationEnum : byte
        {
            [Display(Name = "Hết hạn hợp đồng")]
            ContractExpiration = 0,            
            [Display(Name = "Tăng lương")]
            SalaryIncrease = 1,           
            [Display(Name = "Nghỉ hưu")]
            Retirement = 2,            
            [Display(Name = "Bổ nhiệm lại")]
            Reappointed = 3

        }
        public enum BoolEnum : byte{
            [Display(Name = "Không")]
            No = 0,
            [Display(Name = "Có")]
            Yes = 1,
            [Display(Name = "Chưa xác định")]
            Undefined = 7
        }

        public enum Gender : byte
        {
            [Display(Name = "Nam")]
            Male = 0,

            [Display(Name = "Nữ")]
            Female = 1
        }

        public enum PropertyDeclarationBoolConst : byte
        {
            [Display(Name = "Không")]
            No = 0,
            [Display(Name = "Có")]
            Yes = 1
        }

        public enum IsLinkBoolConst : byte
        {
            [Display(Name = "Không")]
            No = 0,
            [Display(Name = "Có")]
            Yes = 1
        }

        public enum BoolConst : byte
        {
            [Display(Name = "Không")]
            No = 0,
            [Display(Name = "Có")]
            Yes = 1,
            [Display(Name = "Chưa xác định")]
            Undefined = 7
        }
        public enum RelationType : byte
        {
            [Display(Name = "Quan hệ của bản thân")]
            Self = 0,

            [Display(Name = "Quan hệ bên vợ/chồng")]
            Other = 1
        }    
        public enum AllowanceType : byte
        {
            [Display(Name = "Độc hại")]
            Toxic  = 1,
            [Display(Name = "Chức vụ lãnh đạo")]
            LeadershipPosition = 2,
            [Display(Name = "Khu vực")]
            Area = 3,
            [Display(Name = "Trách nhiệm")]
            Responsibility = 4,
            [Display(Name = "Lưu động")]
            Mobile = 5,
        }
        public enum CategoryType : byte
        {
            //Position = 0,
            [Display(Name = "Hạng thương binh")]
            WoundedSoldier = 1,
            //PayRate = 2,
            //[Display(Name = "Trình độ chuyên môn")]
            //Qualification = 3,
            [Display(Name = "Lý luận chính trị")]
            PoliticsTheoReticalLevel = 4,
            [Display(Name = "Bằng cấp")]
            Diploma = 5,
            [Display(Name = "Loại hình đào tạo")]
            TrainningType = 6,
            [Display(Name = "Học vị")]
            AcademicLevel = 7,
            [Display(Name = "Cấp ban hành")]
            DecisionMaker = 8,
            [Display(Name = "Danh hiệu thi đua")]
            EmulationTitle = 9,
            [Display(Name = "Công chức")]
            CivilServant = 10,
            [Display(Name = "Viên chức")]
            PublicServant = 11,
            [Display(Name = "Hình thức khen thưởng")]
            CommendationForm = 12,
            [Display(Name = "Hình thức thay đổi quá trình công tác")]
            WorkingProcessForm = 13,
            [Display(Name = "Đánh giá")]
            Assess = 14,
            [Display(Name = "Hợp đồng lao động")]
            LaborContract = 15,
            [Display(Name = "Chức danh(chức vụ) lãnh đạo")]
            LeadershipTitle = 16,
            [Display(Name = "Chức danh(chức vụ) chuyên môn")]
            ProfessionalTitle = 17,
            [Display(Name = "Tổ chức bộ máy và biên chế")]
            OrgAndPayroll = 18,
            [Display(Name = "Quy hoạch cán bộ")]
            ZoningStaff = 19,
            [Display(Name = "Kế hoạch tuyển dụng")]
            RecruitmentPlan = 20,
            [Display(Name = "Công tác đánh giá cán bộ")]
            StaffAssessment = 21,
            [Display(Name = "Thi đua khen thưởng")]
            EmulationAndReward = 22,
            [Display(Name = "Cải cách hành chính")]
            AdministrativeReform = 23,
            [Display(Name = "Quy chế dân chủ")]
            DemocraticRegulation = 24,
            [Display(Name = "Đi nước ngoài")]
            GoAbroad = 25,
            [Display(Name = "Công tác Đảng")]
            CommunistPartyProcess = 26,
            [Display(Name = "Tất cả")]
            All = 100
        }

        public enum DocumentType : byte
        {
            [Display(Name = "Đề án")]
            Scheme = 0,
            [Display(Name = "Quyết định")]
            Decision = 1
        }
        public enum ManagementGroupType : byte
        {
            [Display(Name = "Quy hoạch cán bộ")]
            PlainningStaff = 0,

            [Display(Name = "Thi đua khen thưởng")]
            Commendation = 1
        }

        public enum CivilServantGroup : byte
        {
            CivilServantA3_1 = 0,
            CivilServantA3_2 = 1,
            CivilServantA3_3 = 2,
            CivilServantA2_1 = 3,
            CivilServantA2_2 = 4,
            CivilServantA1 = 5,
            CivilServantA0 = 6,
            CivilServantB = 7,
            CivilServantC_1 = 8,
            CivilServantC_2 = 9,
            CivilServantC_3 = 10,
            OfficialsA3_1 = 11,
            OfficialsA3_2 = 12,
            OfficialsA3_3 = 13,
            OfficialsA2_1 = 14,
            OfficialsA2_2 = 15,
            OfficialsA1 = 16,
            OfficialsA0 = 17,
            OfficialsB = 18,
            OfficialsC_1 = 19,
            OfficialsC_2 = 20,
            OfficialsC_3 = 21,
        }
    }
}
