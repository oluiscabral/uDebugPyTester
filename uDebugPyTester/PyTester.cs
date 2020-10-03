using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using uDebug.API;

namespace uDebugPyTester
{
    public class PyTester : Tester
    {
        private static int workingCounter = 0;
        private static int workingLimit = 2;
        private static int processedCounter = 0;

        private string executor = "python.exe";
        public PyTester(Judge judge)
        {
            this.judge = judge;
            this.fileExtension = ".py";
            completeTestSetup();
        }

        override public void Test(int testAmount)
        {
            workingCounter = 0;
            processedCounter = 0;

            Console.WriteLine("Executing all requested tests...");
            for(int i = 0; i < testAmount; i++)
            {
                while (workingCounter >= workingLimit)
                {
                    Thread.Sleep(100);
                }
                workingCounter += 1;
                new Thread(ProcessTest).Start();
            }
            while (processedCounter < testAmount)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine("All tests have been completed");
        }

        private void ProcessTest()
        {
            try
            {
                string input = TesterProcess.getStaticOutput(this.testModule, executor);
                string output = TesterProcess.getStaticOutput(this.module, executor, false, input);
                if (input is null || output is null)
                {
                    return;
                }
                string uDebugOutput = this.client.GetOutput(this.judge, this.problemID, input);
                if (!output.Equals(uDebugOutput))
                {
                    generateErrorLog(input);
                    return;
                }
            }
            catch (Exception exc)
            {
                // ignore for now
            }
            finally
            {
                Interlocked.Decrement(ref workingCounter);
                Interlocked.Increment(ref processedCounter);
            }
        }
    }
}
