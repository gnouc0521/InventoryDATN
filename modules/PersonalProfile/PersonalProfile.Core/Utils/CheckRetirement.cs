using System;
using System.Collections.Generic;
using Abp.Timing;
using System.Text;
using static bbk.netcore.mdl.PersonalProfile.Core.Enums.StatusEnum;

namespace bbk.netcore.mdl.PersonalProfile.Core.Utils
{
    public static class CheckRetirement
    {
        public static DateTime? CheckRetire(Gender gender, DateTime dateOfBirth)
        {
            // ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month

            DateTime? retireTime = null;

            var ageMonths = (Math.Abs(Clock.Now.Year - dateOfBirth.Year) * 12) + (Clock.Now.Month - dateOfBirth.Month);

            if (gender == Gender.Male)
            {
                if (dateOfBirth >= new DateTime(1961, 01, 01) && dateOfBirth <= new DateTime(1961, 09, 30))
                {
                    var retireMonths = 60 * 12 + 3;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1961, 10, 01) && dateOfBirth <= new DateTime(1962, 06, 30))
                {
                    var retireMonths = 60 * 12 + 6;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1962, 07, 01) && dateOfBirth <= new DateTime(1963, 03, 31))
                {
                    var retireMonths = 60 * 12 + 9;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1963, 04, 01) && dateOfBirth <= new DateTime(1963, 12, 31))
                {
                    var retireMonths = 61 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1964, 01, 01) && dateOfBirth <= new DateTime(1964, 09, 30))
                {
                    var retireMonths = 61 * 12 + 3;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1964, 10, 01) && dateOfBirth <= new DateTime(1965, 06, 30))
                {
                    var retireMonths = 61 * 12 + 6;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1965, 07, 01) && dateOfBirth <= new DateTime(1966, 03, 31))
                {
                    var retireMonths = 61 * 12 + 9;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1966, 04, 01))
                {
                    var retireMonths = 62 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
            }

            if (gender == Gender.Female)
            {
                if (dateOfBirth >= new DateTime(1966, 01, 01) && dateOfBirth <= new DateTime(1966, 08, 31))
                {
                    var retireMonths = 55 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1966, 09, 01) && dateOfBirth <= new DateTime(1967, 04, 30))
                {
                    var retireMonths = 55 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1967, 05, 01) && dateOfBirth <= new DateTime(1967, 12, 31))
                {
                    var retireMonths = 56 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1968, 01, 01) && dateOfBirth <= new DateTime(1968, 08, 31))
                {
                    var retireMonths = 56 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1968, 09, 01) && dateOfBirth <= new DateTime(1969, 05, 31))
                {
                    var retireMonths = 56 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1969, 06, 01) && dateOfBirth <= new DateTime(1969, 12, 31))
                {
                    var retireMonths = 57 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1970, 01, 01) && dateOfBirth <= new DateTime(1970, 08, 31))
                {
                    var retireMonths = 57 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1970, 09, 01) && dateOfBirth <= new DateTime(1971, 04, 30))
                {
                    var retireMonths = 57 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1971, 05, 01) && dateOfBirth <= new DateTime(1971, 12, 31))
                {
                    var retireMonths = 58 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1972, 01, 01) && dateOfBirth <= new DateTime(1972, 08, 31))
                {
                    var retireMonths = 58 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1972, 09, 01) && dateOfBirth <= new DateTime(1973, 04, 30))
                {
                    var retireMonths = 58 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1973, 05, 01) && dateOfBirth <= new DateTime(1973, 12, 31))
                {
                    var retireMonths = 59 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1974, 01, 01) && dateOfBirth <= new DateTime(1974, 08, 31))
                {
                    var retireMonths = 59 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1974, 09, 01) && dateOfBirth <= new DateTime(1975, 04, 30))
                {
                    var retireMonths = 59 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1975, 05, 01))
                {
                    var retireMonths = 60 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
            }
            return retireTime;
        }

        public static DateTime? CheckRetire2(Gender gender, DateTime dateOfBirth, DateTime decisionDate)
        {
            // ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month

            DateTime? retireTime = null;

            var ageMonths = (Math.Abs(decisionDate.Year - dateOfBirth.Year) * 12) + (decisionDate.Month - dateOfBirth.Month);

            if (gender == Gender.Male)
            {
                if (dateOfBirth >= new DateTime(1961, 01, 01) && dateOfBirth <= new DateTime(1961, 09, 30))
                {
                    var retireMonths = 60 * 12 + 3;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1961, 10, 01) && dateOfBirth <= new DateTime(1962, 06, 30))
                {
                    var retireMonths = 60 * 12 + 6;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1962, 07, 01) && dateOfBirth <= new DateTime(1963, 03, 31))
                {
                    var retireMonths = 60 * 12 + 9;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1963, 04, 01) && dateOfBirth <= new DateTime(1963, 12, 31))
                {
                    var retireMonths = 61 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1964, 01, 01) && dateOfBirth <= new DateTime(1964, 09, 30))
                {
                    var retireMonths = 61 * 12 + 3;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1964, 10, 01) && dateOfBirth <= new DateTime(1965, 06, 30))
                {
                    var retireMonths = 61 * 12 + 6;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1965, 07, 01) && dateOfBirth <= new DateTime(1966, 03, 31))
                {
                    var retireMonths = 61 * 12 + 9;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1966, 04, 01))
                {
                    var retireMonths = 62 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
            }

            if (gender == Gender.Female)
            {
                if (dateOfBirth >= new DateTime(1966, 01, 01) && dateOfBirth <= new DateTime(1966, 08, 31))
                {
                    var retireMonths = 55 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1966, 09, 01) && dateOfBirth <= new DateTime(1967, 04, 30))
                {
                    var retireMonths = 55 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1967, 05, 01) && dateOfBirth <= new DateTime(1967, 12, 31))
                {
                    var retireMonths = 56 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1968, 01, 01) && dateOfBirth <= new DateTime(1968, 08, 31))
                {
                    var retireMonths = 56 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1968, 09, 01) && dateOfBirth <= new DateTime(1969, 05, 31))
                {
                    var retireMonths = 56 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1969, 06, 01) && dateOfBirth <= new DateTime(1969, 12, 31))
                {
                    var retireMonths = 57 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1970, 01, 01) && dateOfBirth <= new DateTime(1970, 08, 31))
                {
                    var retireMonths = 57 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1970, 09, 01) && dateOfBirth <= new DateTime(1971, 04, 30))
                {
                    var retireMonths = 57 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1971, 05, 01) && dateOfBirth <= new DateTime(1971, 12, 31))
                {
                    var retireMonths = 58 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1972, 01, 01) && dateOfBirth <= new DateTime(1972, 08, 31))
                {
                    var retireMonths = 58 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1972, 09, 01) && dateOfBirth <= new DateTime(1973, 04, 30))
                {
                    var retireMonths = 58 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1973, 05, 01) && dateOfBirth <= new DateTime(1973, 12, 31))
                {
                    var retireMonths = 59 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1974, 01, 01) && dateOfBirth <= new DateTime(1974, 08, 31))
                {
                    var retireMonths = 59 * 12 + 4;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1974, 09, 01) && dateOfBirth <= new DateTime(1975, 04, 30))
                {
                    var retireMonths = 59 * 12 + 8;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
                else if (dateOfBirth >= new DateTime(1975, 05, 01))
                {
                    var retireMonths = 60 * 12;
                    if (ageMonths >= retireMonths - 4)
                    {
                        retireTime = dateOfBirth.AddMonths(retireMonths);
                    }
                }
            }
            return retireTime;
        }
    }
}

