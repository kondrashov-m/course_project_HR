using System.Collections.Generic;

namespace HRSystem.UI
{
    public class ConsoleManager : IConsoleManager
    {
        public void ClearScreen()
        {
            Console.Clear();
        }
        
        public void PrintHeader(string title)
        {
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine($"║{"",-40}║");
            Console.WriteLine($"║{CenterText(title, 38),-40}║");
            Console.WriteLine($"║{"",-40}║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine();
        }
        
        public void PrintMenu(List<MenuItem> items)
        {
            Console.WriteLine("┌────────────────────────────────────────┐");
            foreach (var item in items)
            {
                Console.WriteLine($"│ {item.Key,-2} {item.Text,-35} │");
            }
            Console.WriteLine("└────────────────────────────────────────┘");
            Console.WriteLine();
        }
        
        public void PrintInfo(string message)
        {
            Console.WriteLine($"  ℹ {message}");
        }
        
        public void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  ✓ {message}");
            Console.ResetColor();
        }
        
        public void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ✗ {message}");
            Console.ResetColor();
        }
        
        public void PrintData(string label, string value)
        {
            Console.WriteLine($"  ┌─ {label}");
            Console.WriteLine($"  │  {value}");
            Console.WriteLine($"  └─");
        }
        
        public string GetInput(string prompt)
        {
            Console.Write($"╭─ {prompt}\n╰─> ");
            return Console.ReadLine() ?? "";
        }
        
        public void WaitForKey()
        {
            Console.WriteLine("\n╭────────────────────────────────────────╮");
            Console.WriteLine("│ Нажмите любую клавишу для продолжения │");
            Console.WriteLine("╰────────────────────────────────────────╯");
            Console.ReadKey();
        }
        
        private string CenterText(string text, int width)
        {
            if (text.Length >= width) return text;
            int padding = (width - text.Length) / 2;
            return text.PadLeft(padding + text.Length).PadRight(width);
        }
    }
}