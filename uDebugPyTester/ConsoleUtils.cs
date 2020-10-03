using System;
using System.Collections.Generic;
using System.Text;

namespace uDebugPyTester
{
    public static class ConsoleUtils
    {
        public static bool getBooleanKeyAnswer()
        {
            return getBooleanKeyAnswer("[y/n]: ");
        }

        public static void writeCleared(string message)
        {
            Console.Clear();
            Console.Write(message);
        }

        public static void writeLineCleared(string message, params object[] arg)
        {
            Console.Clear();
            StringBuilder stringBuilder = new StringBuilder(message);
            for (int i = 0; i < arg.Length; i++)
            {
                stringBuilder.Replace("{" + i + "}", arg[i].ToString());
            }
            Console.WriteLine(stringBuilder.ToString());
        }

        public static bool getBooleanKeyAnswer(string message)
        {
            Console.Write(message);
            ConsoleKey keyPressed = Console.ReadKey().Key;
            while (keyPressed != ConsoleKey.Y && keyPressed != ConsoleKey.Enter && keyPressed != ConsoleKey.N)
            {
                Console.Write('\n');
                Console.WriteLine("Answer not listed");
                Console.Write(message);
                keyPressed = Console.ReadKey().Key;
            }
            Console.Write('\n');
            return keyPressed == ConsoleKey.Y || keyPressed == ConsoleKey.Enter;
        }

        public static int getIntegerInput(string message)
        {
            return getIntegerInput(message, false);
        }

        public static int getIntegerInput(string message, bool cleared)
        {
            int input;
            Action<string> writeMethod;
            if (cleared)
            {
                 writeMethod = writeCleared;
            } else
            {
                writeMethod = Console.Write;
            }
            writeMethod(message);
            while (true)
            {
                try
                {
                    input = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    writeMethod(message);
                    continue;
                }
                break;
            }
            return input;
        }
    }
}
