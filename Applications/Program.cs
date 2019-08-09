using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            Console.WriteLine("ProcessBt");
            var option = Console.ReadLine();
            if (option.ToLower() == "exit")
            {
                Console.WriteLine("bye bye");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("");
            ProcessDosomething(new BtFileFinder());

            ProcessOption();
        }

        private static void ProcessDosomething(object finder)
        {
            Console.Clear();
            var methodDic = GetMethodDic(finder);
            var text = new StringBuilder();
            foreach (var key in methodDic.Keys)
            {
                var m = methodDic[key];
                text.AppendLine($"{key}: {m.Name}");
            }

            Console.WriteLine(text);
            var option = Console.ReadLine();
            if (option.ToLower() == "exit")
            {
                Console.WriteLine("Finish Process");
                Console.ReadLine();
                Console.Clear();
                return;
            }

            if (!int.TryParse(option, out var optionValue) || !methodDic.Keys.Any(x => x == optionValue))
            {
                Console.WriteLine("wrong option");
                Console.ReadLine();
                ProcessDosomething(finder);
                return;
            }

            var method = methodDic[optionValue];
            var paras = method.GetParameters();
            var values = new List<object>();
            foreach (var p in paras)
            {
                Console.WriteLine($"{p.Name}({p.ParameterType.Name})");
                var inputText = Console.ReadLine();
                var realValue = ToRealType(inputText, p.ParameterType);
                values.Add(realValue);
            }
            Console.WriteLine("Invoke...");
            var result = method.Invoke(finder, values.ToArray());
            if (result != null)
            {
                if (result is Dictionary<string, string>)
                {
                    var realResult = result as Dictionary<string, string>;
                    var strResult = realResult.Select(x => $"{x.Key} => {x.Value}");
                    ShowResult(strResult);
                }

                if (result is IEnumerable<string>)
                {
                    ShowResult(result as IEnumerable<string>);
                }
            }
            Console.WriteLine("==================");
            Console.WriteLine("finished");
            Console.WriteLine("Press any key...");
            Console.ReadLine();

            ProcessDosomething(finder);
        }

        private static void ShowResult(IEnumerable<string> result)
        {
            Console.WriteLine();
            if (!result.Any())
            {
                Console.WriteLine("No Result");
                return;
            }

            Console.WriteLine("Save File(Y) or Print(else)？");
            var ans = Console.ReadLine();
            if (ans.ToUpper() == "Y")
            {
                File.WriteAllText($"R:\\{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.txt", string.Join(Environment.NewLine, result.ToArray()), Encoding.UTF8);
            }
            else
            {
                Console.WriteLine("Result:");
                foreach (var s in result)
                {
                    Console.WriteLine(s);
                }
            }
        }

        private static Dictionary<int, MethodInfo> GetMethodDic(object o)
        {
            var methods = o.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            var i = 0;
            return methods.ToDictionary(k => i++, v => v);
        }

        private static object ToRealType(string inputText, Type type)
        {
            switch (type.Name.ToLower())
            {
                case "int32":
                    return Convert.ToInt32(inputText);
                case "double":
                    return Convert.ToDouble(inputText);
                case "datetime":
                    return Convert.ToDateTime(inputText);
                case "decimal":
                    return Convert.ToDecimal(inputText);
                case "boolean":
                    return Convert.ToBoolean(inputText);
                case "string":
                default:
                    return inputText;
            }
        }
    }
}
