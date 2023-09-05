using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace bbk.netcore.mdl.OMS.Core.Enums
{
    public class ScheduleWorkEnum 
    {
        public enum ColorPriority : byte
        {
            [Display(Name = "Thấp")]
            Lightblue = 0,
            [Display(Name = "Trung bình")]
            Lightreen = 1,
            [Display(Name = "Cao")]
            Lightcoral = 2,
        }

    }


}
