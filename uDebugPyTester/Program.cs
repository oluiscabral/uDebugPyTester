using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using uDebug.API;

namespace uDebugPyTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Test())
            {
                Console.WriteLine("All tests passed");
            }
            else
            {
                Console.WriteLine("Test case error generated");
            }
        }


        private static bool Test()
        {
            Console.WriteLine("Insert the source folder");
            string folder = Console.ReadLine();
            Console.WriteLine("Insert the module name (without .py)");
            string moduleName = Console.ReadLine();
            Console.WriteLine("Insert how many tests you want to run");
            int testAmount = int.Parse(Console.ReadLine());
            string module = folder + "\\" + moduleName + ".py";
            string tester = folder + "\\tester.py";
            if (!File.Exists(module) || !File.Exists(tester))
            {
                return false;
            }

            var client = new Client();

            int problemId = int.Parse(moduleName);

            for (int i = 0; i < testAmount; i++)
            {
                string testInput = createProcess(tester);
                string output = createProcess(module, testInput);

                if (testInput is null || output is null)
                {
                    return false;
                }

                string uDebugOutput = client.GetOutput(Judge.URI, problemId, testInput);

                if (!output.Equals(uDebugOutput))
                {
                    writeErrorTestCase(folder, testInput);
                    return false;
                }
            }
            return true;
        }

        private static string createProcess(string module)
        {
            return createProcess(module, null);
        }

        private static string createProcess(string module, string input)
        {
            bool redirectStdInp = input != null;

            StringBuilder output = new StringBuilder();

            Process proc = new Process();
            proc.StartInfo.FileName = "python.exe";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = redirectStdInp;
            proc.StartInfo.UseShellExecute = false;

            proc.StartInfo.Arguments = string.Concat(module);
            proc.Start();

            if (redirectStdInp)
            {
                StreamWriter sWriter = proc.StandardInput;
                sWriter.WriteLine(input);
            }

            StreamReader sReader = proc.StandardOutput;
            string o = sReader.ReadToEnd();

            proc.WaitForExit();

            if (!string.IsNullOrEmpty(o))
            {
                output.Append(o).Replace("\r\n", "\n");
                while (output[output.Length - 1].Equals('\n'))
                {
                    output.Remove(output.Length - 1, 1);
                }
            }
            else
            {
                return null;
            }

            return output.ToString();
        }

        private static void writeErrorTestCase(string path, string testCase)
        {
            path += "error.txt";
            File.Create(path);
            new StreamWriter(path).Write(testCase);
        }
    }
}
