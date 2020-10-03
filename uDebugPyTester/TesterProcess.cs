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

        public TesterProcess(string module, string fileName, bool useShellExecute)
        {
            process.StartInfo.FileName = fileName;
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
            this.process.StartInfo.RedirectStandardInput = input != null;

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

        public static string getStaticOutput(string module, string fileName)
        {
            return getStaticOutput(module, fileName, false);
        }

        public static string getStaticOutput(string module, string fileName, bool useShellExecute)
        {
            return getStaticOutput(module, fileName, useShellExecute, null);
        }

        public static string getStaticOutput(string module, string fileName, bool useShellExecute, string input)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
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
