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
            TesterProcess inputProc = new TesterProcess(this.testModule, "python.exe", false);
            TesterProcess outputProc = new TesterProcess(this.module, "python.exe", false);

            for (int i = 0; i < testAmount; i++)
            {
                string input = inputProc.getOutput();
                string output = outputProc.getOutput(input);

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
    }
}
