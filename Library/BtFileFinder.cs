using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace BtFileProcesserNet
{
    public class BtFileFinder
    {
        /// <summary>
        /// 取得需要重新命名的檔案列舉
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="extName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetNeedRenameFiles(string rootPath, string extName)
        {
            var result = new Dictionary<string, string>();
            var dirs = Directory.EnumerateDirectories(rootPath);
            foreach (var dir in dirs)
            {
                var ans = from m in Directory.EnumerateFiles(dir, $"*.{extName}")
                          from j in Directory.EnumerateFiles(dir, "*.jpg")
                          where m.Substring(0, m.Length - 4) != j.Substring(0, j.Length - 4)
                          select new
                          {
                              RealName = j.Substring(0, j.Length - 4),
                              FilePath = Path.Combine(dir, m)
                          };
                ans.ToList().ForEach(x => result.Add(x.FilePath, x.RealName));
            }
            return result;
        }

        /// <summary>
        /// 重新命名影音檔案
        /// </summary>
        /// <param name="rootPath">欲搜尋的根目錄</param>
        /// <param name="extName">影音檔副檔名</param>
        public void RenameVideoFile(string rootPath, string extName)
        {
            var result = GetNeedRenameFiles(rootPath, extName);
            foreach (var filePath in result.Keys)
            {
                var path = Path.GetDirectoryName(filePath);
                var realPath = Path.Combine(path, $"{result[filePath]}.{extName}");
                File.Move(filePath, realPath);
            }
        }

        /// <summary>
        /// 取得需要重新命名的資料夾列舉
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetNeededRenameDirs(string rootPath)
        {
            var result = new Dictionary<string, string>();
            var dirs = Directory.EnumerateDirectories(rootPath);
            foreach (var dir in dirs)
            {
                var ans = from j in Directory.EnumerateFiles(dir, "*.jpg")
                          where dir != j.Substring(0, j.Length - 4)
                          select new
                          {
                              RealName = j.Substring(0, j.Length - 4),
                              Path = dir
                          };
                ans.ToList().ForEach(x => result.Add(x.Path, x.RealName));
            }
            return result;
        }

        /// <summary>
        /// 重新命名資料夾名稱
        /// </summary>
        /// <param name="rootPath">欲搜尋的根目錄</param>
        public void RenameDirName(string rootPath)
        {
            var result = GetNeededRenameDirs(rootPath);
            foreach (var path in result.Keys)
            {
                var parentPath = Path.GetPathRoot(path);
                var realPath = Path.Combine(parentPath, $"{result[path]}");
                Directory.Move(path, realPath);
            }
        }
    }
}
