﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtFileProcesserNet
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessOption();
        }

        private static void ProcessOption()
        {
            Console.WriteLine("0: DeleteEmptyDir, 1: ProcessBt");
            var option = Console.ReadLine();
            if (option.ToLower() == "exit")
            {
                Console.WriteLine("bye bye");
                Console.ReadLine();
                return;
            }

            if (!int.TryParse(option, out var optionValue) || !(new int[] { 0, 1 }).Any(x => x == optionValue))
            {
                Console.WriteLine("wrong option");
                Console.ReadLine();
            }

            Console.WriteLine("");

            if (optionValue == 0)
                DeleteEmptyDir(null);

            if (optionValue == 1)
                ProcessBt(null);

            ProcessOption();
        }

        private static void DeleteEmptyDir(EmptyDirFinder finder)
        {
            Console.WriteLine("0: FindEmptyDir, 1: DeleteDir");
            var option = Console.ReadLine();
            if (option.ToLower() == "exit")
            {
                Console.WriteLine("Finish DeleteEmptyDir");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            if (!int.TryParse(option, out var optionValue) || !(new int[] { 0, 1 }).Any(x => x == optionValue))
            {
                Console.WriteLine("wrong option");
                Console.ReadLine();
            }

            finder = finder ?? new EmptyDirFinder();
            if (optionValue == 0)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                var result = finder.FindEmptyDir(rootPath);
                foreach (var filePath in result)
                {
                    Console.WriteLine(filePath);
                }
                Console.WriteLine("==================================");
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (optionValue == 1)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                var result = finder.FindEmptyDir(rootPath);
                foreach (var path in result)
                {
                    finder.DeleteDir(path);
                }
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            DeleteEmptyDir(finder);
        }

        private static void ProcessBt(BtFileFinder finder)
        {
            var text = new StringBuilder();
            text.AppendLine("0: GetNeedRenameFiles");
            text.AppendLine("1: RenameVideoFile");
            text.AppendLine("2: GetNeededRenameDirs");
            text.AppendLine("3: RenameDirName");
            text.AppendLine("4: FindOverFiles");

            Console.WriteLine(text);
            var option = Console.ReadLine();
            if (option.ToLower() == "exit")
            {
                Console.WriteLine("Finish ProcessBt");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            if (!int.TryParse(option, out var optionValue) || !(new int[] { 0, 1, 2, 3, 4 }).Any(x => x == optionValue))
            {
                Console.WriteLine("wrong option");
                Console.ReadLine();
            }

            finder = finder ?? new BtFileFinder();
            if (optionValue == 0)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                Console.WriteLine("Ext Name");
                var extName = Console.ReadLine();
                var result = finder.GetNeedRenameFiles(rootPath, extName);
                var i = 0;
                foreach (var file in result.Keys)
                {
                    Console.WriteLine($"{++i}:{file}, {result[file]}");
                }
                Console.WriteLine("=================================");
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (optionValue == 1)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                Console.WriteLine("Ext Name");
                var extName = Console.ReadLine();
                var result = finder.GetNeedRenameFiles(rootPath, extName);
                foreach (var filePath in result.Keys)
                {
                    finder.RenameVideoFile(filePath, result[filePath]);
                }
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (optionValue == 2)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                var result = finder.GetNeededRenameDirs(rootPath);
                var i = 0;
                foreach (var path in result.Keys)
                {
                    Console.WriteLine($"{++i}:{path}, {result[path]}");
                }
                Console.WriteLine("=================================");
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (optionValue == 3)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                var result = finder.GetNeededRenameDirs(rootPath);
                foreach (var path in result.Keys)
                {
                    finder.RenameDirName(path, result[path]);
                }
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (optionValue == 4)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                Console.WriteLine("Over Count:");
                var overCount = Console.ReadLine();
                if (!int.TryParse(overCount, out var overCountValue))
                {
                    Console.WriteLine("Wrong number");
                    ProcessBt(finder);
                }

                var result = finder.FindOverFiles(rootPath, overCountValue);
                foreach (var path in result)
                {
                    Console.WriteLine($"{path}");
                }
                Console.WriteLine("=================================");
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            ProcessBt(finder);
        }
    }
}
