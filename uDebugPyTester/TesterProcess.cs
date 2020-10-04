using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace uDebugPyTester
{
    public class TesterProcess
    {
        private Process process = new Process();

        public TesterProcess(string module, string executor, bool useShellExecute)
        {
            process.StartInfo.FileName = executor;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = useShellExecute;
            process.StartInfo.Arguments = string.Concat(module);
        }

        public string getOutput()
        {
            return getOutput(null);
        }

        public string getOutput(string input)
        {
            this.process.StartInfo.RedirectStandardInput = !string.IsNullOrEmpty(input);

            process.Start();

            if (this.process.StartInfo.RedirectStandardInput)
            {
                StreamWriter sWriter = process.StandardInput;
                sWriter.WriteLine(input);
            }

            StreamReader sReader = process.StandardOutput;
            string output = sReader.ReadToEnd();

            process.Close();

            if (!string.IsNullOrEmpty(output))
            {
                return output.Replace("\r\n", "\n").TrimEnd();
            }
            else
            {
                return null;
            }
        }

        public static string getStaticOutput(string module, string executor)
        {
            return getStaticOutput(module, executor, false);
        }

        public static string getStaticOutput(string module, string executor, bool useShellExecute)
        {
            return getStaticOutput(module, executor, useShellExecute, null);
        }

        public static string getStaticOutput(string module, string executor, bool useShellExecute, string input)
        {
            Process process = new Process();
            process.StartInfo.FileName = executor;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = useShellExecute;
            process.StartInfo.Arguments = string.Concat(module);
            process.StartInfo.RedirectStandardInput = input != null;

            process.Start();

            if (process.StartInfo.RedirectStandardInput)
            {
                StreamWriter sWriter = process.StandardInput;
                sWriter.WriteLine(input);
            }

            StreamReader sReader = process.StandardOutput;
            string output = sReader.ReadToEnd();

            process.Close();

            if (!string.IsNullOrEmpty(output))
            {
                return output.Replace("\r\n", "\n").TrimEnd();
            }
            else
            {
                return null;
            }
        }
    }
}
