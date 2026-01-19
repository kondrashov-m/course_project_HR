using System.Collections.Generic;

namespace HRSystem.UI
{
    public interface IConsoleManager
    {
        void ClearScreen();
        void PrintHeader(string title);
        void PrintMenu(List<MenuItem> items);
        void PrintInfo(string message);
        void PrintSuccess(string message);
        void PrintError(string message);
        void PrintData(string label, string value);
        string GetInput(string prompt);
        void WaitForKey();
    }
    
    public class MenuItem
    {
        public string Key { get; }
        public string Text { get; }
        
        public MenuItem(string key, string text)
        {
            Key = key;
            Text = text;
        }
    }
}