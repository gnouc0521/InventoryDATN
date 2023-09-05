using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using static bbk.netcore.mdl.OMS.Core.Enums.DayOffEnum;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class DayOffEnum
    {

        public enum TypeDayOff : byte
        {
            [Display(Name = "Nghỉ thường")]
            DayOffNormal = 0, //nghỉ thường

            [Display(Name = "Nghỉ phép")]
            DayOffAllow = 1, //nghỉ phép

            [Display(Name = "Nghỉ lễ")]
            DayOffHoliday = 2, //nghỉ lễ
        }

    }
}
