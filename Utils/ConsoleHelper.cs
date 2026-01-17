using System;

namespace HRSystem.Utils
{
    /// <summary>
    ///  ласс дл€ вспомогательных консольных операций.
    /// </summary>
    public class ConsoleHelper
    {
        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public static void Write(string message)
        {
            Console.Write(message);
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static void ClearScreen()
        {
            Console.Clear();
        }
    }
}
