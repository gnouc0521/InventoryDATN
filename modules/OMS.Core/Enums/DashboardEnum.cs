using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class DashboardEnum
    {
        public enum TypeStatus : byte
        {
            [Display(Name = "CV Ban kế hoạch")]
            Plan = 0,
            [Display(Name = " CV Mua hàng")]
            Purchase = 1,
            [Display(Name = "CV GĐ")]
            Manager = 2,
            [Display(Name = "CV Kho")]
            Warehouse = 3,
            [Display(Name = "CV Kế toán")]
            Accountant = 4,
        }
    }
}
