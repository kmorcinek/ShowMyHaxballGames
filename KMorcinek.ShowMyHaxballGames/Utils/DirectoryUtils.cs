using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KMorcinek.ShowMyHaxballGames.Utils
{
    public class DirectoryUtils
    {
        public static void EnsureDirectoryExistst(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            Directory.CreateDirectory(fileInfo.Directory.FullName);
        }
    }
}