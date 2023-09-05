using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Core.Utils
{
    public static class GetAge
    {
        public static int GetRealAge(DateTime DateOfBirth)
        {
            int Age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateTime.Now.Month < DateOfBirth.Month)
            {
                Age = Age - 1;
            }
            if (DateTime.Now.Month == DateOfBirth.Month)
            {
                if(DateTime.Now.Day < DateOfBirth.Day)
                {
                    Age = Age - 1;
                }
            }
            return Age;
        }
    }
}
