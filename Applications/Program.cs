using System;
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
            ProcessOption(null);
        }

        private static void ProcessOption(BtFileFinder finder)
        {
            Console.WriteLine("0: RenameVideoFile, 1: RenameDirName, 2: GetNeedRenameFiles, 3: GetNeededRenameDirs");
            var option = Console.ReadLine();
            if (option.ToLower() == "exit")
            {
                Console.WriteLine("bye bye");
                Console.ReadLine();
                return;
            }

            if (!int.TryParse(option, out var optionValue) || !(new int[] { 0, 1, 2, 3 }).Any(x => x == optionValue))
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
                finder.RenameVideoFile(rootPath, extName);
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (optionValue == 1)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                finder.RenameDirName(rootPath);
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (optionValue == 2)
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

            if (optionValue == 3)
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                var result = finder.GetNeededRenameDirs(rootPath);
                var i = 0;
                foreach (var path in result.Keys)
                {
                    Console.WriteLine($"{++i}:{path}, {result[path]}");
                }
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            ProcessOption(finder);
        }
    }
}
