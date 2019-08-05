using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace BtFileProcesserNet
{
    /// <summary>
    /// BT 檔案更新器
    /// </summary>
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
                          where GetFileRealName(m) != GetFileRealName(j)
                          select new
                          {
                              RealName = GetFileRealName(j),
                              FilePath = Path.Combine(dir, m)
                          };
                ans.ToList().ForEach(x => result.Add(x.FilePath, x.RealName));
            }
            return result;
        }

        /// <summary>
        /// 重新命名影音檔案
        /// </summary>
        /// <param name="filePath">欲更名的檔案路徑</param>
        /// <param name="realName">欲更名的檔案名稱</param>
        public void RenameVideoFile(string filePath, string realName)
        {
            var path = Path.GetDirectoryName(filePath);
            var ext = Path.GetExtension(filePath);
            var realPath = Path.Combine(path, $"{realName}.{ext}");
            File.Move(filePath, realPath);            
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
                          where dir != GetFileRealName(j)
                          select new
                          {
                              RealName = GetFileRealName(j),
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
        public void RenameDirName(string path, string realName)
        {
            var parentPath = Path.GetPathRoot(path);
            var realPath = Path.Combine(parentPath, $"{realName}");
            Directory.Move(path, realPath);            
        }

        /// <summary>
        /// 取得真正檔案名稱
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileRealName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
}
