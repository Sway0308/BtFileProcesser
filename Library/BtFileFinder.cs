﻿using System;
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
        public void RenameVideoFile(string rootPath, string extName)
        {
            var result = GetNeedRenameFiles(rootPath, extName);
            foreach (var filePath in result.Keys)
            {
                var path = Path.GetDirectoryName(filePath);
                var ext = Path.GetExtension(filePath);
                var realPath = Path.Combine(path, $"{result[filePath]}.{ext}");
                File.Move(filePath, realPath);
            }
        }

        /// <summary>
        /// 取得資料夾中存有超過指定數目檔案的資料夾名稱列舉
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="overCount"></param>
        /// <returns></returns>
        public IEnumerable<string> FindOverFiles(string rootPath, int overCount)
        {
            var dirs = from d in Directory.EnumerateDirectories(rootPath)
                       where Directory.EnumerateFiles(d).Count() >= overCount
                       select d;
            return dirs;
        }

        /// <summary>
        /// 取得真正檔案名稱
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFileRealName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        /// <summary>
        /// 找到圖檔有完整名稱，但資料夾卻沒有完整名稱的資料
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetFullImgFileNameButSimpleDirName(string rootPath)
        {
            var dirs = Directory.EnumerateDirectories(rootPath);
            var jpgs = from d in dirs
                       from j in Directory.EnumerateFiles(d, "*.jpg")
                       where j.Contains(d)
                        && Path.GetFileName(j).Length > d.Replace(Path.GetDirectoryName(d) + "\\", "").Length + 4
                        && d.Replace(Path.GetDirectoryName(d) + "\\", "").Length == 9
                       select new {
                           Path = d,
                           Jpg = j
                       };
            return jpgs.ToDictionary(k => k.Jpg, v => v.Path);
        }

        /// <summary>
        /// 重新命名資料夾名稱
        /// </summary>
        /// <param name="rootPath">欲搜尋的根目錄</param>
        public void RenameDirName(string rootPath)
        {
            var result = GetFullImgFileNameButSimpleDirName(rootPath);
            foreach (var jpg in result.Keys)
            {
                var realName = Path.GetFileNameWithoutExtension(jpg);
                var path = result[jpg];

                var parentPath = Path.GetDirectoryName(path);
                var realPath = Path.Combine(path, $"{realName}");
                Directory.Move(path, realPath);
            }
        }

        /// <summary>
        /// 搜尋一個資料夾中有多個資料夾，但無其他檔案的資料夾
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetNestingDirs(string rootPath)
        {
            var result = new Dictionary<string, string>();
            foreach (var d in Directory.EnumerateDirectories(rootPath))
            {
                var dirs = GetDirInDirButNoFiles(d);
                if (!dirs.Any())
                    continue;
                result.Add(d, string.Concat(dirs));
            }

            return result;
        }

        private string[] GetDirInDirButNoFiles(string path)
        {
            var dirs = from d in Directory.EnumerateDirectories(path)
                       where !Directory.EnumerateFiles(d).Any()
                       select d;
            return dirs.ToArray();
        }
    }
}
