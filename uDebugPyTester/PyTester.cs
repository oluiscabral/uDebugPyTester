using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using uDebug.API;

namespace uDebugPyTester
{
    public class PyTester : Tester
    {
        readonly private object monitor = new object();
        private string executor = "python.exe";
        public PyTester(Judge judge)
        {
            this.judge = judge;
            this.fileExtension = ".py";
            completeTestSetup();
        }

        override public bool Test(int testAmount)
        {
            TesterProcess inputProc = new TesterProcess(this.testModule, executor, false);
            TesterProcess outputProc = new TesterProcess(this.module, executor, false);
            int n = 1;
            Parallel.For(0, testAmount, i =>
            {
                Console.WriteLine("Processing test "+i);
                string input = TesterProcess.getStaticOutput(testModule, executor);
                string output = TesterProcess.getStaticOutput(module,executor,false, input);

                if (input is null || output is null)
                {
                    return;
                }
                int errorCounter = 0;
                string uDebugOutput = new Client().GetOutput(this.judge, this.problemID, input);
                while (!output.Equals(uDebugOutput))
                {
                    if (!string.IsNullOrEmpty(uDebugOutput))
                    {
                        errorCounter++;
                    }
                    uDebugOutput = new Client().GetOutput(this.judge, this.problemID, input);
                    if (errorCounter == 5)
                    {
                        generateErrorLog(input);
                        return;
                    }
                }
            });
            return true;
        }
    }
}
