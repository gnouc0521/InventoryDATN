using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace bbk.netcore.Authorization
{
    public class PersonalProfileAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //var PPSAdminPages = context.GetPermissionOrNull(PersonalProfilePermission.Pages_PPSAdmin)
            //    ?? context.CreatePermission(PersonalProfilePermission.Pages_PPSAdmin, L("Pages_PPSAdmin"));
            //if (PPSAdminPages != null)
            //{
            //    PPSAdminPages.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Reports, L("Pages_PPS_Reports"));
            //    PPSAdminPages.CreateChildPermission(PersonalProfilePermission.Pages_PPS_UIs, L("Pages_PPS_UIs"));
            //    PPSAdminPages.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Categories, L("Pages_PPS_Categories"));
            //    var PPSDoc = PPSAdminPages.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Documents, L("Pages_PPS_Documents"));
            //    PPSDoc.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Documents_Create, L("Pages_PPS_Documents_Create"));
            //    PPSDoc.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Documents_Edit, L("Pages_PPS_Documents_Edit"));
            //    PPSDoc.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Documents_Delete, L("Pages_PPS_Documents_Delete"));
            //}

            //var PPSDashboard = context.GetPermissionOrNull(PersonalProfilePermission.Pages_PPS_Management)
            //    ?? context.CreatePermission(PersonalProfilePermission.Pages_PPS_Management, L("Pages_PPS_Dashboard"));
            //if (PPSDashboard != null)
            //{
            //    PPSDashboard.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Staff_View, L("Pages_PPS_Staff_View"));
            //    PPSDashboard.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Staff_Create, L("Pages_PPS_Staff_Create"));
            //    PPSDashboard.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Staff_Edit, L("Pages_PPS_Staff_Edit"));
            //    PPSDashboard.CreateChildPermission(PersonalProfilePermission.Pages_PPS_Staff_Delete, L("Pages_PPS_Staff_Delete"));
            //}
            //var Congviec = context.CreatePermission(PersonalProfilePermission.PersonalProfileCV, L("Quản lý công việc"));
            //Congviec.CreateChildPermission(PersonalProfilePermission.PersonalProfileCV_Create, L("Thêm công việc"));
            //Congviec.CreateChildPermission(PersonalProfilePermission.PersonalProfileCV_Edit, L("Sửa công việc"));
            //Congviec.CreateChildPermission(PersonalProfilePermission.PersonalProfileCV_Delete, L("Xoá công việc"));
            //Congviec.CreateChildPermission(PersonalProfilePermission.PersonalProfileCV_Calendar, L("Đặt lịch"));

            //var ProfileWork = context.CreatePermission(PersonalProfilePermission.PersonalProfileHSCV, L("Hồ sơ công việc"));
            //ProfileWork.CreateChildPermission(PersonalProfilePermission.PersonalProfileHSCV_Create, L("Thêm hồ sơ công việc"));
            //ProfileWork.CreateChildPermission(PersonalProfilePermission.PersonalProfileHSCV_Edit, L("Sửa hồ sơ công việc"));
            //ProfileWork.CreateChildPermission(PersonalProfilePermission.PersonalProfileHSCV_Delete, L("Xoá hồ sơ công việc"));

            //var DayOff = context.CreatePermission(PersonalProfilePermission.PersonalProfileDO, L("Quản lý ngày nghỉ"));
            //DayOff.CreateChildPermission(PersonalProfilePermission.PersonalProfileDO_Create, L("Thêm ngày nghỉ"));
            //DayOff.CreateChildPermission(PersonalProfilePermission.PersonalProfileDO_Edit, L("Sửa ngày nghỉ"));
            //DayOff.CreateChildPermission(PersonalProfilePermission.PersonalProfileDO_Delete, L("Xóa ngày nghỉ"));


            //var WorkGroup = context.CreatePermission(PersonalProfilePermission.PersonalProfileWG, L("Quản lý nhóm công việc"));
            //WorkGroup.CreateChildPermission(PersonalProfilePermission.PersonalProfileWG_Create, L("Thêm nhóm công việc"));
            //WorkGroup.CreateChildPermission(PersonalProfilePermission.PersonalProfileWG_Edit, L("Sửa nhóm công việc"));


            //var Sw = context.CreatePermission(PersonalProfilePermission.PersonalProfileSW, L("Lịch làm việc"));
            //Sw.CreateChildPermission(PersonalProfilePermission.PersonalProfileSW_Create, L("Thêm lịch"));
            //Sw.CreateChildPermission(PersonalProfilePermission.PersonalProfileSW_Edit, L("Sửa lịch"));
            //Sw.CreateChildPermission(PersonalProfilePermission.PersonalProfileSW_Delete, L("Xóa lịch"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, netcoreConsts.LocalizationSourceName);
        }
    }
}
