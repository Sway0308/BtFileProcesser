using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtFileProcesserNet
{
    /// <summary>
    /// 空資料夾找尋器
    /// </summary>
    public class EmptyDirFinder
    {
        public IEnumerable<string> FindEmptyDir(string rootPath)
        {
            var dirs = from d in Directory.EnumerateDirectories(rootPath)
                       where !Directory.EnumerateFiles(d).Any()
                        && !Directory.EnumerateDirectories(d).Any()
                       select d;
            return dirs;
        }

        /// <summary>
        /// 刪除資料夾
        /// </summary>
        /// <param name="dir"></param>
        public void DeleteDir(string dir)
        {
            Directory.Delete(dir);
        }
    }
}
