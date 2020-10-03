using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using uDebug.API;

namespace uDebugPyTester
{
    public abstract class Tester
    {
        private protected Client client = new Client();
        
        private string sourceDir;

        private protected int problemID;

        private protected  string module;
        private protected string testModule;

        private protected Judge judge;
        private protected string fileExtension;

        private readonly string testRelativePath = "testers\\";
        private readonly string testExtension = ".tester";
        private readonly string errorExtension = ".error.txt";

        private string errorsDirectory;
        private int errorCounter = 0;

        public void completeTestSetup()
        {
            defineSourceDir();
            while (!defineProblemID())
            {
                Console.Clear();
                defineProblemID();
            }
        }

        public void updateProblemID()
        {
            while (!defineProblemID())
            {
                Console.Clear();
                defineProblemID();
            }
            this.errorCounter = 0;
        }

        private void defineSourceDir()
        {
            Console.Write("Insert the source folder: ");
            string sourceDir = Path.GetFullPath(Console.ReadLine());
            while (!Directory.Exists(sourceDir))
            {
                Console.Clear();
                Console.WriteLine("Especified directory does not exist");
                Console.Write("Insert the source directory: ");
                sourceDir = Path.GetFullPath(Console.ReadLine());
            }
            if (!Path.EndsInDirectorySeparator(sourceDir))
            {
                sourceDir += Path.DirectorySeparatorChar;
            }
            this.sourceDir = sourceDir;
            this.errorsDirectory = sourceDir + "\\errors\\";
        }

        private bool defineProblemID()
        {
            int problemID = int.MinValue;
            string module = null;
            string testModule = null;

            string moduleName = null;
            string testModuleNamePath = null;

            bool moduleFileExist = true;
            bool testModuleFileExist = true;
            bool chooseCreateFile = false;

            void checkFiles()
            {
                moduleFileExist = File.Exists(module);
                testModuleFileExist = File.Exists(testModule);
            }

            void showInexistentFileDialog(string moduleName)
            {
                ConsoleUtils.writeLineCleared("{0} does not exist. Do you want to create it?", moduleName);
                chooseCreateFile = ConsoleUtils.getBooleanKeyAnswer("Press Y if you going to create it or N if you want to change the problem ID. [y/n]: ");
                if (chooseCreateFile)
                {
                    ConsoleUtils.writeLineCleared("Press any key after creating {0}: ", moduleName);
                    Console.ReadKey();
                    checkFiles();
                }
            }

            do
            {
                if (!moduleFileExist)
                {
                    showInexistentFileDialog(moduleName);
                } else if (!testModuleFileExist)
                {
                    showInexistentFileDialog(testModuleNamePath);
                }
                if (!chooseCreateFile) 
                {
                    problemID = ConsoleUtils.getIntegerInput("Insert the problem ID: ");
                    moduleName = problemID + this.fileExtension;
                    module = this.sourceDir + moduleName;
                    testModuleNamePath = this.testRelativePath + problemID + this.testExtension + this.fileExtension;
                    testModule = this.sourceDir + testModuleNamePath;
                    checkFiles();
                }
            } while (!moduleFileExist || !testModuleFileExist);

            if (problemID == int.MinValue || module is null || testModule is null)
            {
                return false;
            }

            this.problemID = problemID;
            this.module = module;
            this.testModule = testModule;
            return true;
        }

        protected void generateErrorLog(string input)
        {
            Directory.CreateDirectory(this.errorsDirectory);

            while(File.Exists(this.errorsDirectory+this.problemID+"."+this.errorCounter + this.errorExtension))
            {
                this.errorCounter++;
            }
            string errorFile = this.errorsDirectory + this.problemID + "." + this.errorCounter + this.errorExtension;
            File.Create(errorFile).Close();
            new StreamWriter(errorFile).Write(input);
            Console.WriteLine("Input that generated error was created at {0}", errorFile);
        }

        public void Test()
        {
            int testAmount = ConsoleUtils.getIntegerInput("Insert how many tests you want to run: ", true);
            Test(testAmount);
        }
        abstract public void Test(int testAmount);
    }
}
