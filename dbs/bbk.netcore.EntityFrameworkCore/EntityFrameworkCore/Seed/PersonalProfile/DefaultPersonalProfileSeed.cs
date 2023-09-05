using System.Linq;
using System.Collections.Generic;
using bbk.netcore.mdl.PersonalProfile.Core.Entities;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;
using Abp.Organizations;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace bbk.netcore.EntityFrameworkCore.Seed.PersonalProfile
{
    public class DefaultPersonalProfileSeed
    {
        private readonly netcoreDbContext _context;

        public DefaultPersonalProfileSeed(netcoreDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateProfileStaffData();
            CreateOrgStructure();
        }

        #region Profile Staff data
        private void CreateProfileStaffData()
        {
            List<string> woundedSoldiers = new List<string>()
            {
                "1/4", "2/4", "3/4", "4/4"
            };
            foreach (var d in woundedSoldiers)
            {
                Category category = new Category() { CategoryType = CategoryType.WoundedSoldier, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.WoundedSoldier && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> politicsTheoReticalLevels = new List<string>()
            {
                "Cao cấp", "Trung cấp", "Sơ cấp", "Cử nhân"
            };
            foreach (var d in politicsTheoReticalLevels)
            {
                Category category = new Category() { CategoryType = CategoryType.PoliticsTheoReticalLevel, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.PoliticsTheoReticalLevel && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> academicLevels = new List<string>()
            {
                "Giáo sư", "Phó giáo sư"
            };
            foreach (var d in academicLevels)
            {
                Category category = new Category() { CategoryType = CategoryType.AcademicLevel, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.AcademicLevel && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> diplomas = new List<string>()
            {
                "Đại học", "Cao đẳng", "Trung cấp", "Sơ cấp", "Thạc sĩ", "Tiến sĩ", "Chứng chỉ"
            };
            foreach (var d in diplomas)
            {
                Category category = new Category() { CategoryType = CategoryType.Diploma, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.Diploma && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            #region Civil Servant - Salary Level
            List<CivilServant> civilServants = new List<CivilServant>()
            {
                //Công chức cao cấp 
                new CivilServant{ Group = CivilServantGroup.CivilServantA3_1, Name = "Chuyên viên cao cấp", Code = "01.001" },
                new CivilServant{ Group = CivilServantGroup.CivilServantA3_1, Name = "Thanh tra viên cao cấp", Code = "04.023" },
                new CivilServant{ Group = CivilServantGroup.CivilServantA3_2, Name = "Kế toán viên cao cấp", Code = "06.029" },

                //Viên chức cao cấp
                new CivilServant{ Group = CivilServantGroup.OfficialsA3_1, Name = "Biên tập viên hạng I", Code = "V.11.01.01" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA3_1, Name = "Kỹ sư cao cấp", Code = "V.05.02.05" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA3_1, Name = "Biên dịch viên hạng I", Code = "V.11.03.07" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA3_1, Name = "Phóng viên hạng I", Code = "V.11.02.04" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA3_1, Name = "Nghiên cứu viên cao cấp", Code = "V.05.01.01" },

                //Công chức - ngạch chuyên viên chính
                new CivilServant{ Group = CivilServantGroup.CivilServantA2_1, Name = "Chuyên viên chính", Code = "01.002" },
                new CivilServant{ Group = CivilServantGroup.CivilServantA2_1, Name = "Thanh tra viên chính", Code = "04.024" },
                new CivilServant{ Group = CivilServantGroup.CivilServantA2_2, Name = "Kế toán viên chính", Code = "06.030" },

                //Viên chức - ngạch chuyên viên chính
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_1, Name = "Nghiên cứu viên chính", Code = "V.05.01.02" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_1, Name = "Kỹ sư chính", Code = "V.05.02.06" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_1, Name = "Quan trắc viên tài nguyên và môi trường hạng II", Code = "V.06.05.13" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_1, Name = "Điều tra viên tài nguyên môi trường hạng II", Code = "V.06.02.04" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_1, Name = "Biên tập viên hạng II", Code = "V.11.01.02" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_1, Name = "Biên dịch viên hạng II", Code = "V.11.03.08" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_1, Name = "Phóng viên hạng II", Code = "V.11.02.05" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_1, Name = "Thư viện viên hạng II", Code = "V.10.02.05" },
                new CivilServant{ Group = CivilServantGroup.OfficialsA2_2, Name = "Lưu trữ viên chính", Code = "V.01.02.01" },

                //Công chức - ngạch chuyên viên
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Chuyên viên", Code = "01.003" },
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Thanh tra viên", Code = "04.025" },
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Kế toán viên", Code = "06.031" },

                //Viên chức - ngạch chuyên viên
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Lưu trữ viên", Code = "V.01.02.02" },
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Nghiên cứu viên", Code = "V.05.01.03"},
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Kỹ sư", Code = "V.05.02.07"},
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Quan trắc viên tài nguyên và môi trường hạng III", Code = "V.06.05.14"},
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Điều tra viên tài nguyên môi trường hạng III", Code = "V.06.02.05"},
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Biên tập viên hạng III", Code = "V.11.01.03"},
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Biên dịch viên hạng III", Code = "V.11.03.09"},
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Phóng viên hạng III", Code = "V.11.02.06"},
                new CivilServant{ Group = CivilServantGroup.CivilServantA1, Name = "Thư viện viên hạng III", Code = "V.10.02.06"},

                //Công chức - ngạch cán sự
                new CivilServant{ Group = CivilServantGroup.CivilServantB, Name = "Cán sự", Code = "01.004"},
                new CivilServant{ Group = CivilServantGroup.CivilServantB, Name = "Kế toán viên trung cấp", Code = "06.032"},

                //Viên chức - ngạch cán sự
                new CivilServant{ Group = CivilServantGroup.OfficialsB, Name = "Lưu trữ viên trung cấp", Code = "V.01.02.03"},
                new CivilServant{ Group = CivilServantGroup.OfficialsB, Name = "Thư viện viên hạng IV", Code = "V.10.02.07"},
                new CivilServant{ Group = CivilServantGroup.OfficialsB, Name = "Kỹ thuật viên", Code = "13.096"},
                new CivilServant{ Group = CivilServantGroup.OfficialsB, Name = "Quan trắc viên tài nguyên và môi trường hạng IV", Code = "V.06.05.15"},
                new CivilServant{ Group = CivilServantGroup.OfficialsB, Name = "Điều tra viên tài nguyên môi trường hạng IV", Code = "V.06.02.06"},

                //Công chức - ngạch nhân viên
                new CivilServant{ Group = CivilServantGroup.CivilServantC_1, Name = "Nhân viên", Code = "01.005"},
                new CivilServant{ Group = CivilServantGroup.CivilServantC_1, Name = "Kế toán viên sơ cấp", Code = "06.033"},
                new CivilServant{ Group = CivilServantGroup.CivilServantC_2, Name = "Thủ quỹ cơ quan, đơn vị", Code = "06.035"},
                new CivilServant{ Group = CivilServantGroup.CivilServantC_2, Name = "Nhân viên thuế", Code = "06.040"},

                //Viên chức - ngạch nhân viên
            };
            foreach (var d in civilServants)
            {
                var entity = _context.CivilServants.FirstOrDefault(u => u.Name == d.Name && u.Code == d.Code);
                if (entity == null)
                {
                    _context.CivilServants.Add(d);
                }
            }


            //Lương
            List<SalaryLevel> salaries = new List<SalaryLevel>
            {
                //Công chức A3
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_1, Level = "1/6", CoefficientsSalary = "6.20" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_1, Level = "2/6", CoefficientsSalary = "6.56" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_1, Level = "3/6", CoefficientsSalary = "6.92" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_1, Level = "4/6", CoefficientsSalary = "7.28" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_1, Level = "5/6", CoefficientsSalary = "7.64" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_1, Level = "6/6", CoefficientsSalary = "8.00" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_2, Level = "1/6", CoefficientsSalary = "5.75" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_2, Level = "2/6", CoefficientsSalary = "6.11" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_2, Level = "3/6", CoefficientsSalary = "6.47" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_2, Level = "4/6", CoefficientsSalary = "6.83" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_2, Level = "5/6", CoefficientsSalary = "7.19" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA3_2, Level = "6/6", CoefficientsSalary = "7.55" },

                //Công chức A2
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_1, Level = "1/8", CoefficientsSalary = "4.40" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_1, Level = "2/8", CoefficientsSalary = "4.74" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_1, Level = "3/8", CoefficientsSalary = "5.08" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_1, Level = "4/8", CoefficientsSalary = "5.42" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_1, Level = "5/8", CoefficientsSalary = "5.76" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_1, Level = "6/8", CoefficientsSalary = "6.10" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_1, Level = "7/8", CoefficientsSalary = "6.44" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_1, Level = "8/8", CoefficientsSalary = "6.78" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_2, Level = "1/8", CoefficientsSalary = "4.00" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_2, Level = "2/8", CoefficientsSalary = "4.34" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_2, Level = "3/8", CoefficientsSalary = "4.68" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_2, Level = "4/8", CoefficientsSalary = "5.02" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_2, Level = "5/8", CoefficientsSalary = "5.36" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_2, Level = "6/8", CoefficientsSalary = "5.70" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_2, Level = "7/8", CoefficientsSalary = "6.04" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA2_2, Level = "8/8", CoefficientsSalary = "6.38" },

                //Công chức A1
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "1/9", CoefficientsSalary = "2.34" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "2/9", CoefficientsSalary = "2.67" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "3/9", CoefficientsSalary = "3.00" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "4/9", CoefficientsSalary = "3.33" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "5/9", CoefficientsSalary = "3.66" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "6/9", CoefficientsSalary = "3.99" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "7/9", CoefficientsSalary = "4.32" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "8/9", CoefficientsSalary = "4.65" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA1, Level = "9/9", CoefficientsSalary = "4.98" },

                //Công chức A0
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "1/10", CoefficientsSalary = "2.10" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "2/10", CoefficientsSalary = "2.41" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "3/10", CoefficientsSalary = "2.72" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "4/10", CoefficientsSalary = "3.03" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "5/10", CoefficientsSalary = "3.34" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "6/10", CoefficientsSalary = "3.65" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "7/10", CoefficientsSalary = "3.96" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "8/10", CoefficientsSalary = "4.27" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "9/10", CoefficientsSalary = "4.58" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantA0, Level = "10/10", CoefficientsSalary = "4.89" },

                //Công chức B
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "1/12", CoefficientsSalary = "1.86" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "2/12", CoefficientsSalary = "2.06" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "3/12", CoefficientsSalary = "2.26" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "4/12", CoefficientsSalary = "2.46" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "5/12", CoefficientsSalary = "2.66" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "6/12", CoefficientsSalary = "2.86" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "7/12", CoefficientsSalary = "3.06" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "8/12", CoefficientsSalary = "3.26" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "9/12", CoefficientsSalary = "3.46" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "10/12", CoefficientsSalary = "3.66" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "11/12", CoefficientsSalary = "3.86" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantB, Level = "12/12", CoefficientsSalary = "4.06" },

                //Công chức C
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "1/12", CoefficientsSalary = "1.65" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "2/12", CoefficientsSalary = "1.83" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "3/12", CoefficientsSalary = "2.01" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "4/12", CoefficientsSalary = "2.19" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "5/12", CoefficientsSalary = "2.37" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "6/12", CoefficientsSalary = "2.55" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "7/12", CoefficientsSalary = "2.73" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "8/12", CoefficientsSalary = "2.91" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "9/12", CoefficientsSalary = "3.09" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "10/12", CoefficientsSalary = "3.27" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "11/12", CoefficientsSalary = "3.45" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_1, Level = "12/12", CoefficientsSalary = "3.63" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "1/12", CoefficientsSalary = "1.50" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "2/12", CoefficientsSalary = "1.68" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "3/12", CoefficientsSalary = "1.86" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "4/12", CoefficientsSalary = "2.04" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "5/12", CoefficientsSalary = "2.22" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "6/12", CoefficientsSalary = "2.40" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "7/12", CoefficientsSalary = "2.58" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "8/12", CoefficientsSalary = "2.76" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "9/12", CoefficientsSalary = "2.94" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "10/12", CoefficientsSalary = "3.12" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "11/12", CoefficientsSalary = "3.30" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_2, Level = "12/12", CoefficientsSalary = "3.48" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "1/12", CoefficientsSalary = "1.35" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "2/12", CoefficientsSalary = "1.53" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "3/12", CoefficientsSalary = "1.71" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "4/12", CoefficientsSalary = "1.89" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "5/12", CoefficientsSalary = "2.07" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "6/12", CoefficientsSalary = "2.25" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "7/12", CoefficientsSalary = "2.43" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "8/12", CoefficientsSalary = "2.61" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "9/12", CoefficientsSalary = "2.79" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "10/12", CoefficientsSalary = "2.97" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "11/12", CoefficientsSalary = "3.15" },
                new SalaryLevel { Group = CivilServantGroup.CivilServantC_3, Level = "12/12", CoefficientsSalary = "3.33" },
                
                //Viên chức A3
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_1, Level = "1/6", CoefficientsSalary = "6.20" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_1, Level = "2/6", CoefficientsSalary = "6.56" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_1, Level = "3/6", CoefficientsSalary = "6.92" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_1, Level = "4/6", CoefficientsSalary = "7.28" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_1, Level = "5/6", CoefficientsSalary = "7.64" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_1, Level = "6/6", CoefficientsSalary = "8.00" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_2, Level = "1/6", CoefficientsSalary = "5.75" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_2, Level = "2/6", CoefficientsSalary = "6.11" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_2, Level = "3/6", CoefficientsSalary = "6.47" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_2, Level = "4/6", CoefficientsSalary = "6.83" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_2, Level = "5/6", CoefficientsSalary = "7.19" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA3_2, Level = "6/6", CoefficientsSalary = "7.55" },

                //Viên chức A2
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_1, Level = "1/8", CoefficientsSalary = "4.40" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_1, Level = "2/8", CoefficientsSalary = "4.74" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_1, Level = "3/8", CoefficientsSalary = "5.08" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_1, Level = "4/8", CoefficientsSalary = "5.42" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_1, Level = "5/8", CoefficientsSalary = "5.76" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_1, Level = "6/8", CoefficientsSalary = "6.10" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_1, Level = "7/8", CoefficientsSalary = "6.44" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_1, Level = "8/8", CoefficientsSalary = "6.44" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_2, Level = "1/8", CoefficientsSalary = "4.00" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_2, Level = "2/8", CoefficientsSalary = "4.34" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_2, Level = "3/8", CoefficientsSalary = "4.68" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_2, Level = "4/8", CoefficientsSalary = "5.02" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_2, Level = "5/8", CoefficientsSalary = "5.36" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_2, Level = "6/8", CoefficientsSalary = "5.70" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_2, Level = "7/8", CoefficientsSalary = "6.04" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA2_2, Level = "8/8", CoefficientsSalary = "6.38" },

                //Viên chức A0
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "1/10", CoefficientsSalary = "2.10" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "2/10", CoefficientsSalary = "2.41" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "3/10", CoefficientsSalary = "2.72" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "4/10", CoefficientsSalary = "3.03" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "5/10", CoefficientsSalary = "3.34" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "6/10", CoefficientsSalary = "3.65" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "7/10", CoefficientsSalary = "3.96" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "8/10", CoefficientsSalary = "4.27" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "9/10", CoefficientsSalary = "4.58" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsA0, Level = "10/10", CoefficientsSalary = "4.89" },

                //Viên chức B
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "1/12", CoefficientsSalary = "1.86" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "2/12", CoefficientsSalary = "2.06" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "3/12", CoefficientsSalary = "2.26" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "4/12", CoefficientsSalary = "2.46" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "5/12", CoefficientsSalary = "2.66" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "6/12", CoefficientsSalary = "2.86" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "7/12", CoefficientsSalary = "3.06" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "8/12", CoefficientsSalary = "3.26" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "9/12", CoefficientsSalary = "3.46" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "10/12", CoefficientsSalary = "3.66" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "11/12", CoefficientsSalary = "3.86" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsB, Level = "12/12", CoefficientsSalary = "4.06" },

                //Viên chức C
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "1/12", CoefficientsSalary = "1.65" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "2/12", CoefficientsSalary = "1.83" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "3/12", CoefficientsSalary = "2.01" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "4/12", CoefficientsSalary = "2.19" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "5/12", CoefficientsSalary = "2.37" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "6/12", CoefficientsSalary = "2.55" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "7/12", CoefficientsSalary = "2.73" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "8/12", CoefficientsSalary = "2.91" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "9/12", CoefficientsSalary = "3.09" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "10/12", CoefficientsSalary = "3.27" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "11/12", CoefficientsSalary = "3.45" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_1, Level = "12/12", CoefficientsSalary = "3.63" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "1/12", CoefficientsSalary = "2.00" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "2/12", CoefficientsSalary = "2.18" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "3/12", CoefficientsSalary = "2.36" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "4/12", CoefficientsSalary = "2.54" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "5/12", CoefficientsSalary = "2.72" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "6/12", CoefficientsSalary = "2.90" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "7/12", CoefficientsSalary = "3.08" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "8/12", CoefficientsSalary = "3.26" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "9/12", CoefficientsSalary = "3.44" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "10/12", CoefficientsSalary = "3.62" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "11/12", CoefficientsSalary = "3.80" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_2, Level = "12/12", CoefficientsSalary = "3.98" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "1/12", CoefficientsSalary = "1.50" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "2/12", CoefficientsSalary = "1.68" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "3/12", CoefficientsSalary = "1.86" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "4/12", CoefficientsSalary = "2.04" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "5/12", CoefficientsSalary = "2.22" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "6/12", CoefficientsSalary = "2.40" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "7/12", CoefficientsSalary = "2.58" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "8/12", CoefficientsSalary = "2.76" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "9/12", CoefficientsSalary = "2.94" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "10/12", CoefficientsSalary = "3.12" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "11/12", CoefficientsSalary = "3.30" },
                new SalaryLevel { Group = CivilServantGroup.OfficialsC_3, Level = "12/12", CoefficientsSalary = "3.48" },
            };
            foreach (var d in salaries)
            {
                var entity = _context.SalaryLevels.FirstOrDefault(u => u.Group == d.Group && u.Level == d.Level);
                if (entity == null)
                {
                    _context.SalaryLevels.Add(d);
                }
            }
            #endregion

            #region table 27 28 29 31 33 30
            List<string> workingProcessForms = new List<string>()
            {
               "Ký hợp đồng lao động", "Tuyển dụng", "Bổ nhiệm", "Bổ nhiệm lại", "Nghỉ hưu", "Thôi việc", "Chấm dứt hợp đồng lao động", "Điều động","Luân chuyển", "Biệt phái", "Miễn nhiệm", "Từ chức"
            };
            foreach (var d in workingProcessForms)
            {
                Category category = new Category() { CategoryType = CategoryType.WorkingProcessForm, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.WorkingProcessForm && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> decisionMakers = new List<string>()
            {
                "Tổng cục trưởng", "Bộ trưởng", "Thủ tướng chính phủ", "Giám đốc"
            };
            foreach (var d in decisionMakers)
            {
                Category category = new Category() { CategoryType = CategoryType.DecisionMaker, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.DecisionMaker && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> leadershipTitle = new List<string>()
            {
                "Tổng cục trưởng", "Quyền Tổng cục trưởng", "Phó Tổng cục trưởng", "Chánh Văn phòng", "Phó Chánh Văn phòng", "Phó Chánh Văn phòng phụ trách",
                "Điều hành Văn phòng", "Vụ trưởng", "Quyền Vụ trưởng", "Phó Vụ trưởng", "Phó Vụ trưởng phụ trách", "Cục trưởng", "Quyền Cục trưởng", "Phó Cục trưởng",
                "Phó Cục trưởng phụ trách", "Giám đốc", "Quyền Giám đốc", "Phó Giám đốc", "Phó Giám đốc Phụ trách", "Phó Giám đốc điều hành", "Viện trưởng", "Phó Viện trưởng",
                "Quyền Viện trưởng", "Phó Viện trưởng phụ trách", "Tổng biên tập", "Phó Tổng biên tập", "Phó Tổng biên tập phụ trách", "Trưởng phòng", "Phó Trưởng phòng",
                "Phó Trưởng phòng Phụ Trách", "Phó Trưởng phòng điều hành", "Điều hành Phòng", "Kế toán trưởng", "Phụ trách Kế toán"
            };
            foreach (var d in leadershipTitle)
            {
                Category category = new Category() { CategoryType = CategoryType.LeadershipTitle, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.LeadershipTitle && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }
                        
            List<string> proTitle = new List<string>()
            {
                "Chuyên viên chính", "Chuyên viên", "Kế toán viên", "Viên chức", "Văn thư", "Tạp vụ", "Bảo vệ", "Lái xe"
            };
            foreach (var d in proTitle)
            {
                Category category = new Category() { CategoryType = CategoryType.ProfessionalTitle, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.ProfessionalTitle && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }
                        
            List<string> laborContrac = new List<string>()
            {
                "Cán bộ hợp đồng"
            };
            foreach (var d in laborContrac)
            {
                Category category = new Category() { CategoryType = CategoryType.LaborContract, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.LaborContract && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> emulationTitles = new List<string>()
            {
                "Lao động tiên tiến", "Chiến sĩ thi đua cấp sơ sở", "Chiến sĩ thi đua cấp Bộ", "Chiến sĩ thi đua toàn quốc"
            };
            foreach (var d in emulationTitles)
            {
                Category category = new Category() { CategoryType = CategoryType.EmulationTitle, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.EmulationTitle && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }
            List<string> commendationForms = new List<string>()
            {
                "Giấy khen của tổng cục trưởng", "Bằng khen của bộ trưởng", "Bằng khen của thủ tướng"};
            foreach (var d in commendationForms)
            {
                Category category = new Category() { CategoryType = CategoryType.CommendationForm, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.CommendationForm && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> trainningTypes = new List<string>()
            {
                "Chính quy", "Tại chức", "Chuyên tu", "Từ xa", "Liên thông", "Bồi dưỡng"
            };
            foreach (var t in trainningTypes)
            {
                Category category = new Category() { CategoryType = CategoryType.TrainningType, Title = t };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.TrainningType && x.Title == t);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }
            List<string> assess = new List<string>()
            {
               "Hoàn thành nhiệm vụ", "Hoàn thành tốt nhiệm vụ", "Hoàn thành xuất sắc nhiểm vụ", "Hoàn thành nhiệm vụ nhưng còn hạn chế năng lực","Không đánh giá xếp loại","Không hoàn thành nhiệm vụ"
            };
            foreach (var t in assess)
            {
                Category category = new Category() { CategoryType = CategoryType.Assess, Title = t };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.Assess && x.Title == t);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }
            #endregion

            #region Nhóm quản lý
            List<string> OrgAndPayroll = new List<string>()
            {
               "Đề án kiện toàn tổ chức", "Chức năng, nhiệm vụ, quyền hạn và cơ cấu tổ chức các đơn vị trực thuộc", "Các Hội đồng, ban chỉ đạo, ban quản lý chương trình, dự án thuộc Tổng cục ", "Đề án vị trí việc làm ", "Kế hoạch và phương án phân bổ biên chế ", "Quyết định về nhân sự"
            };
            foreach (var d in OrgAndPayroll)
            {
                Category category = new Category() { CategoryType = CategoryType.OrgAndPayroll, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.OrgAndPayroll && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> ZoningStaff = new List<string>()
            {
               "Đối tượng quy hoạch", "Số lượng", "Giai đoạn", "Căn cứ giới thiệu vào quy hoạch", "Thẩm quyền phê duyệt"
            };
            foreach (var d in ZoningStaff)
            {
                Category category = new Category() { CategoryType = CategoryType.ZoningStaff, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.ZoningStaff && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> RecruitmentPlan = new List<string>()
            {
               "Quyết định phê duyệt", "Báo cáo kết quả thực hiện"
            };
            foreach (var d in RecruitmentPlan)
            {
                Category category = new Category() { CategoryType = CategoryType.RecruitmentPlan, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.RecruitmentPlan && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> StaffAssessment = new List<string>()
            {
               "Kết quả đánh giá tổng hợp"
            };
            foreach (var d in StaffAssessment)
            {
                Category category = new Category() { CategoryType = CategoryType.StaffAssessment, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.StaffAssessment && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> EmulationAndReward = new List<string>()
            {
               "Thi đua ", "Khen thưởng định kỳ", "Khen thưởng đột xuất", "Hội đồng thi đua, khen thưởng", "Hội đồng xét duyệt sáng kiến"
            };
            foreach (var d in EmulationAndReward)
            {
                Category category = new Category() { CategoryType = CategoryType.EmulationAndReward, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.EmulationAndReward && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> AdministrativeReform = new List<string>()
            {
               "Chương trình cải cách hành chính", "Kế hoạch cải cách hành chính", "Báo cáo kết quả cải cách hành chính"
            };
            foreach (var d in AdministrativeReform)
            {
                Category category = new Category() { CategoryType = CategoryType.AdministrativeReform, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.AdministrativeReform && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> DemocraticRegulation = new List<string>()
            {
               "Báo cáo Quy chế dân chủ ở cơ sở"
            };
            foreach (var d in DemocraticRegulation)
            {
                Category category = new Category() { CategoryType = CategoryType.DemocraticRegulation, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.DemocraticRegulation && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> GoAbroad = new List<string>()
            {
               "Quyết định cử đi"
            };
            foreach (var d in GoAbroad)
            {
                Category category = new Category() { CategoryType = CategoryType.GoAbroad, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.GoAbroad && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }

            List<string> CommunistPartyProcess = new List<string>()
            {
               "Lý lịch Đảng viên", "Đánh giá phân loại Đảng viên", "Bồi dưỡng Đảng viên", "Kết nạp Đảng viên", "Kiểm tra, giám sát", "Kiểm điểm Đảng viên", "Sinh hoạt chuyên đề"
            };
            foreach (var d in CommunistPartyProcess)
            {
                Category category = new Category() { CategoryType = CategoryType.CommunistPartyProcess, Title = d };
                var entity = _context.Categories.FirstOrDefault(x => x.CategoryType == CategoryType.CommunistPartyProcess && x.Title == d);
                if (entity == null)
                {
                    _context.Categories.Add(category);
                }
            }
            #endregion

            _context.SaveChanges();
        }
        #endregion

        #region Org VEA data
        public static List<OrganizationUnit> InitialOrganizationUnits => GetInitialOrganizationUnitVEA();
        private void CreateOrgStructure()
        {
            foreach (var orgUnit in InitialOrganizationUnits)
            {
                var existedOrg = AddOrgIfNotExists(orgUnit);
                if (existedOrg != null)
                {
                    if (existedOrg.DisplayName.Equals("Lãnh đạo Tổng cục", System.StringComparison.OrdinalIgnoreCase))
                    {
                        var childOrgs = GetInitialLDTC(existedOrg);
                        var countCode = 1;
                        foreach (var c in childOrgs)
                        {
                            c.Code = existedOrg.Code + "." + "0000" + countCode; // not greater than 10
                            c.ParentId = existedOrg.Id;
                            c.TenantId = existedOrg.TenantId;
                            AddOrgIfNotExists(c);
                            countCode++;
                        }
                    }
                    if (existedOrg.DisplayName.Equals("Cấp cục", System.StringComparison.OrdinalIgnoreCase))
                    {
                        var childOrgs = GetInitialCC(existedOrg);
                        var countCode = 1;
                        foreach (var c in childOrgs)
                        {
                            c.Code = existedOrg.Code + "." + "0000" + countCode; // not greater than 10
                            c.ParentId = existedOrg.Id;
                            c.TenantId = existedOrg.TenantId;
                            AddOrgIfNotExists(c);
                            countCode++;
                        }
                    }
                    if (existedOrg.DisplayName.Equals("Khối Văn phòng", System.StringComparison.OrdinalIgnoreCase))
                    {
                        var childOrgs = GetInitialKVP(existedOrg);
                        var countCode = 1;
                        foreach (var c in childOrgs)
                        {
                            c.Code = existedOrg.Code + "." + "0000" + countCode; // not greater than 10
                            c.ParentId = existedOrg.Id;
                            c.TenantId = existedOrg.TenantId;
                            AddOrgIfNotExists(c);
                            countCode++;
                        }
                    }
                    if (existedOrg.DisplayName.Equals("Khối Đơn vị sự nghiệp", System.StringComparison.OrdinalIgnoreCase))
                    {
                        var childOrgs = GetInitialDVSN(existedOrg);
                        var countCode = 1;
                        foreach (var c in childOrgs)
                        {
                            c.Code = existedOrg.Code + "." + "0000" + countCode; // not greater than 10
                            c.ParentId = existedOrg.Id;
                            c.TenantId = existedOrg.TenantId;
                            AddOrgIfNotExists(c);
                            countCode++;
                        }
                    }
                }
            }
        }
        private static List<OrganizationUnit> GetInitialOrganizationUnitVEA()
        {
            //var tenantId = netcoreConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            var tenantId = MultiTenancyConsts.DefaultTenantId;
            return new List<OrganizationUnit>
            {
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Lãnh đạo Tổng cục", Code = "00001" },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Cấp cục", Code = "00002" },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Khối Văn phòng", Code = "00003" },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Khối Đơn vị sự nghiệp", Code = "00004" },
            };
        }

        private static List<OrganizationUnit> GetInitialLDTC(OrganizationUnit parent)
        {
            var tenantId = netcoreConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            return new List<OrganizationUnit>
            {
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Tổng cục trưởng", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Phó Tổng cục trưởng", Parent = parent },
            };
        }

        private static List<OrganizationUnit> GetInitialCC(OrganizationUnit parent)
        {
            var tenantId = netcoreConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            return new List<OrganizationUnit>
            {
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Bảo tồn TN & Đa dạng SH", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "BVMT Miền Bắc", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "BVMT Miền Trung và TN", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "BVMT Miền Nam", Parent = parent },
            };
        }

        private static List<OrganizationUnit> GetInitialKVP(OrganizationUnit parent)
        {
            var tenantId = netcoreConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            return new List<OrganizationUnit>
            {
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Văn phòng tổng cục", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Vụ Chính sách, Pháp chế & Thanh tra", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Vụ KHCN và Hợp tác QT", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Vụ Kế hoạch - Tài chính", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Vụ Tổ chức cán bộ", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Vụ QL Chất thải", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Vụ QL Chất lượng môi trường", Parent = parent },
                new OrganizationUnit() { TenantId = tenantId, DisplayName = "Vụ Thẩm định đánh giá tác độ môi trường", Parent = parent },
            };
        }

        private static List<OrganizationUnit> GetInitialDVSN(OrganizationUnit parent)
        {
            var tenantId = netcoreConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            return new List<OrganizationUnit>
            {
                new OrganizationUnit { TenantId = tenantId, DisplayName = "Viện Khoa học môi trường", Parent = parent },
                new OrganizationUnit { TenantId = tenantId, DisplayName = "TT Thông tin và Dữ liệu môi trường", Parent = parent },
                new OrganizationUnit { TenantId = tenantId, DisplayName = "TT Tư vấn và CN Môi trường", Parent = parent },
                new OrganizationUnit { TenantId = tenantId, DisplayName = "TT Quan trắc môi trường Miền Bắc", Parent = parent },
                new OrganizationUnit { TenantId = tenantId, DisplayName = "TT Quan trắc môi trường Miền Trung và Tây Nguyên", Parent = parent },
                new OrganizationUnit { TenantId = tenantId, DisplayName = "TT Quan trắc môi trường Miền Nam", Parent = parent },
            };
        }

        private OrganizationUnit AddOrgIfNotExists(OrganizationUnit org)
        {
            if (_context.OrganizationUnits.IgnoreQueryFilters().Any(l => l.TenantId == org.TenantId && l.DisplayName == org.DisplayName))
            {
                return null;
            }

            _context.OrganizationUnits.Add(org);
            _context.SaveChanges();

            return org;
        }
        #endregion
    }
}
