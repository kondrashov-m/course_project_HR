using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HRSystem.Utils
{
    /// <summary>
    /// Helper wrapping Console to allow testing and encoding fallbacks.
    /// </summary>
    public class ConsoleHelper : IConsoleHelper
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteBox(string text, int padding = 1)
        {
            var lines = (text ?? string.Empty).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int max = lines.Length == 0 ? 0 : lines.Max(l => l.Length);
            int innerWidth = max + padding * 2;
            var border = new string('-', innerWidth + 2);
            Console.WriteLine(border);
            foreach (var line in lines)
            {
                Console.WriteLine("|" + new string(' ', padding) + line.PadRight(max) + new string(' ', padding) + "|");
            }
            Console.WriteLine(border);
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void ClearScreen()
        {
            Console.Clear();
        }

        public string ReadLine()
        {
            // Simple and reliable: just use Console.ReadLine() which respects the Console encoding we set
            var line = Console.ReadLine();
            return line ?? string.Empty;
        }

    }
}
