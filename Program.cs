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
            ProcessOption();
        }

        private static void ProcessOption()
        {
            Console.WriteLine("0: RenameVideoFile, 1: RenameDirName, 2: GetNeedRenameFiles, 3: GetNeededRenameDirs");
            var option = Console.ReadLine();
            if (option.ToLower() == "exit")
            {
                Console.WriteLine("bye bye");
                Console.ReadLine();
                return;
            }

            if (option != "0" && option != "1" && option != "2" && option != "3")
            {
                Console.WriteLine("wrong option");
                Console.ReadLine();
            }

            var finder = new BtFileFinder();
            if (option == "0")
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                Console.WriteLine("Ext Name");
                var extName = Console.ReadLine();
                finder.RenameVideoFile(rootPath, extName);
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (option == "1")
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                finder.RenameDirName(rootPath);
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (option == "2")
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                Console.WriteLine("Ext Name");
                var extName = Console.ReadLine();
                var result = finder.GetNeedRenameFiles(rootPath, extName);
                foreach (var file in result.Keys)
                {
                    Console.WriteLine(file, result[file]);
                }
                Console.WriteLine("=================================");
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            if (option == "3")
            {
                Console.WriteLine("RootPath:");
                var rootPath = Console.ReadLine();
                var result = finder.GetNeededRenameDirs(rootPath);
                foreach (var path in result.Keys)
                {
                    Console.WriteLine(path, result[path]);
                }
                Console.WriteLine("Done");
                Console.ReadLine();
            }

            ProcessOption();
        }
    }
}
