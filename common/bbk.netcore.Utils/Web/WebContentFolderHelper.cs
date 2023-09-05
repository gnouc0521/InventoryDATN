using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace bbk.netcore.Web
{
    //public static class WebContentDirectoryFinder
    //{
    //    public static string CalculateContentRootFolder()
    //    {
    //        var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(WebContentDirectoryFinder).GetAssembly().Location);
    //        if (coreAssemblyDirectoryPath == null)
    //        {
    //            throw new Exception("Could not find location of ModularTodoApp.Core assembly!");
    //        }

    //        var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
    //        while (!DirectoryContains(directoryInfo.FullName, "ModularTodoApp.sln"))
    //        {
    //            if (directoryInfo.Parent == null)
    //            {
    //                throw new Exception("Could not find content root folder!");
    //            }

    //            directoryInfo = directoryInfo.Parent;
    //        }

    //        var webMvcFolder = Path.Combine(directoryInfo.FullName, "src", "ModularTodoApp.Web.Mvc");
    //        if (Directory.Exists(webMvcFolder))
    //        {
    //            return webMvcFolder;
    //        }

    //        throw new Exception("Could not find root folder of the web project!");
    //    }

    //    private static bool DirectoryContains(string directory, string fileName)
    //    {
    //        return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
    //    }
    //}
}
