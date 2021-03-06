﻿using System;
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
            PyTester pyTester = new PyTester(Judge.URI);

            while (true)
            {
                if (pyTester.Test())
                {
                    Console.WriteLine("All tests passed");
                } else
                {
                    Console.WriteLine("Some test failed");
                }
                if (!ConsoleUtils.getBooleanKeyAnswer("Do you want to rerun tests with the same module? [y/n]: "))
                {
                    if (ConsoleUtils.getBooleanKeyAnswer("Change source directory? [y/n]: "))
                    {
                        pyTester.completeTestSetup();
                    } else
                    {
                        pyTester.updateProblemID();
                    }
                }
            }
        }
    }
}
