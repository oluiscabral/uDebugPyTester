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

            for (int i = 0; i < testAmount; i++)
            {
                string input = inputProc.getOutput();
                string output = outputProc.getOutput(input);

                if (input is null || output is null)
                {
                    return false;
                }

                int errorCounter = 0;
                string uDebugOutput = this.client.GetOutput(this.judge, this.problemID, input);
                while (!output.Equals(uDebugOutput))
                {
                    if (!string.IsNullOrEmpty(uDebugOutput))
                    {
                        errorCounter++;
                    }
                    uDebugOutput = this.client.GetOutput(this.judge, this.problemID, input);
                    if (errorCounter == 3)
                    {
                        generateErrorLog(input);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
