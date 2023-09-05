using System.Linq;
using System.Collections.Generic;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;
using Abp.Organizations;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Abp.Localization;
using System;

namespace bbk.netcore.EntityFrameworkCore.Seed.PersonalProfile
{
    public class DefaultLanguageVeaSeed
    {
        private readonly netcoreDbContext _context;

        public DefaultLanguageVeaSeed(netcoreDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateLanguages();
        }

        #region Languages VEA data
        public static List<ApplicationLanguageText> InitialLanguageTexts => GetLanguageDocuments();
        private void CreateLanguages()
        {
            foreach (var langTextUnit in InitialLanguageTexts)
            {
                AddLanguageTextIfNotExists(langTextUnit);
            }
        }
        private static List<ApplicationLanguageText> GetLanguageDocuments()
        {
            //var tenantId = netcoreConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            //var tenantId = MultiTenancyConsts.DefaultTenantId;
            return new List<ApplicationLanguageText>
            {
                /* EN Language*/
                // DocumentCategoryEnum values
                new ApplicationLanguageText() { Key = "OrgAndPayroll", Value = "Tổ chức bộ máy và biên chế", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "ZoningStaff", Value = "Quy Hoạch cán bộ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "RecruitmentPlan", Value = "Kế hoạch tuyển dụng", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                //new ApplicationLanguageText() { Key = "LaborRegimePolicyAndSalary", Value = "Chế độ chính sách LĐ & Tiền lương", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "StaffAssessment", Value = "Công tác đánh giá cán bộ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "EmulationAndReward", Value = "Thi đua khen thưởng", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "AdministrativeReform", Value = "Cải cách hành chính", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "DemocraticRegulation", Value = "Quy chế dân chủ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },            

                // DocumentTypeEnum values
                new ApplicationLanguageText() { Key = "LawOrdinance", Value = "Luật - Pháp lệnh", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Constitution", Value = "Hiến pháp", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Ordinance", Value = "Sắc lệnh - Sắc luật", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Decree", Value = "Nghị định", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Decision", Value = "Quyết định", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Circulars", Value = "Thông tư", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Documentary", Value = "Công văn", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Resolution", Value = "Nghị quyết", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Directive", Value = "Chỉ thị", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "JointCircular", Value = "Thông tư liên tịch", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "FormDocument", Value = "Biểu mẫu", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },

                new ApplicationLanguageText() { Key = "Administration", Value = "Quản trị phần mềm", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Creating", Value = "Tạo mới", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Editing", Value = "Chỉnh sửa", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Deleting", Value = "Xóa", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Viewing", Value = "Xem", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Reporting", Value = "Báo cáo", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Users", Value = "Quản lý người dùng", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "CreatingNewUser", Value = "Tạo mới người dùng", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "ChangingPermissions", Value = "Thay đổi quyền", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "DeletingUser", Value = "Xóa người dùng", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "EditingUser", Value = "Chỉnh sửa người dùng", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "LoginForUsers", Value = "Đăng nhập trang quản lý người dùng", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Dashboard", Value = "Quản lý cán bộ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Staff_View", Value = "Xem thông tin cán bộ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Staff_Create", Value = "Tạo mới cán bộ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Staff_Edit", Value = "Chỉnh sửa cán bộ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Staff_Delete", Value = "Xóa cán bộ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Dashboard", Value = "Trang chủ", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPSAdmin", Value = "Quản trị phần mềm", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Categories", Value = "Quản lý danh mục", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Documents", Value = "Quản lý tài liệu", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Documents_Create", Value = "Tạo mới tài liệu", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Documents_Edit", Value = "Chỉnh sửa tài liệu", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Documents_Delete", Value = "Xóa tài liệu", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Reports", Value = "Báo cáo", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_UIs", Value = "Quản lý giao diện", LanguageName = "en", Source = "netcore", CreationTime = DateTime.Now },

                /* VI Language*/
                new ApplicationLanguageText() { Key = "OrgAndPayroll", Value = "Tổ chức bộ máy và biên chế", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "ZoningStaff", Value = "Quy Hoạch cán bộ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "RecruitmentPlan", Value = "Kế hoạch tuyển dụng", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "StaffAssessment", Value = "Công tác đánh giá cán bộ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "EmulationAndReward", Value = "Thi đua khen thưởng", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "AdministrativeReform", Value = "Cải cách hành chính", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "DemocraticRegulation", Value = "Quy chế dân chủ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },            
                
                new ApplicationLanguageText() { Key = "LawOrdinance", Value = "Luật - Pháp lệnh", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Constitution", Value = "Hiến pháp", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Ordinance", Value = "Sắc lệnh - Sắc luật", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Decree", Value = "Nghị định", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Decision", Value = "Quyết định", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Circulars", Value = "Thông tư", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Documentary", Value = "Công văn", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Resolution", Value = "Nghị quyết", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Directive", Value = "Chỉ thị", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "JointCircular", Value = "Thông tư liên tịch", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "FormDocument", Value = "Biểu mẫu", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },

                new ApplicationLanguageText() { Key = "Administration", Value = "Quản trị phần mềm", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Creating", Value = "Tạo mới", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Editing", Value = "Chỉnh sửa", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Deleting", Value = "Xóa", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Viewing", Value = "Xem", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Reporting", Value = "Báo cáo", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Users", Value = "Quản lý người dùng", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "CreatingNewUser", Value = "Tạo mới người dùng", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "ChangingPermissions", Value = "Thay đổi quyền", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "DeletingUser", Value = "Xóa người dùng", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "EditingUser", Value = "Chỉnh sửa người dùng", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "LoginForUsers", Value = "Đăng nhập trang quản lý người dùng", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Dashboard", Value = "Quản lý cán bộ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Staff_View", Value = "Xem thông tin cán bộ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Staff_Create", Value = "Tạo mới cán bộ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Staff_Edit", Value = "Chỉnh sửa cán bộ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Staff_Delete", Value = "Xóa cán bộ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Dashboard", Value = "Trang chủ", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPSAdmin", Value = "Quản trị phần mềm", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Categories", Value = "Quản lý danh mục", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Documents", Value = "Quản lý tài liệu", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Documents_Create", Value = "Tạo mới tài liệu", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Documents_Edit", Value = "Chỉnh sửa tài liệu", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Documents_Delete", Value = "Xóa tài liệu", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_Reports", Value = "Báo cáo", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
                new ApplicationLanguageText() { Key = "Pages_PPS_UIs", Value = "Quản lý giao diện", LanguageName = "vi", Source = "netcore", CreationTime = DateTime.Now },
            };
        }
        private void AddLanguageTextIfNotExists(ApplicationLanguageText languageText)
        {
            if (_context.LanguageTexts.IgnoreQueryFilters().Any(l => l.TenantId == languageText.TenantId && l.LanguageName == languageText.LanguageName && l.Source == languageText.Source && l.Key == languageText.Key))
            {
                return;
            }

            _context.LanguageTexts.Add(languageText);
            _context.SaveChanges();
        }
        #endregion
    }
}
