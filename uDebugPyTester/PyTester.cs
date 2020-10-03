using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using uDebug.API;

namespace uDebugPyTester
{
    public class PyTester : Tester
    {
        public PyTester(Judge judge)
        {
            this.judge = judge;
            this.fileExtension = ".py";
            completeTestSetup();
        }

        override public bool Test(int testAmount)
        {
            for (int i = 0; i < testAmount; i++)
            {
                string input = generateOutput(this.testModule);
                string output = generateOutput(this.module, input);

                if (input is null || output is null)
                {
                    return false;
                }

                string uDebugOutput = this.client.GetOutput(this.judge, this.problemID, input);

                if (!output.Equals(uDebugOutput))
                {
                    generateErrorLog(input);
                    return false;
                }
            }
            return true;
        }

        private static string generateOutput(string module)
        {
            return generateOutput(module, null);
        }

        private static string generateOutput(string module, string input)
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
    }
}
