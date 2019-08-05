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
            Console.WriteLine("0: RenameVideoFile, 1: RenameDirName");
            var option = Console.ReadLine();
            if (option != "0" && option != "1")
            {
                Console.WriteLine("Bye");
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
        }
    }
}
