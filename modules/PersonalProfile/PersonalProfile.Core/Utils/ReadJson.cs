using bbk.netcore.mdl.PersonalProfile.Core.Utils.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Core.Utils
{
    public static class ReadJson<T> where T : class
    {
        public static T ConvertJsonToObject(string file)
        {
            using (StreamReader r = new StreamReader(file))
            {
                var json = r.ReadToEnd();
                var items = JsonConvert.DeserializeObject<T>(json);
                return items;
            }
        }
    }
}
