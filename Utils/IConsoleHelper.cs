using System;

namespace HRSystem.Utils
{
    /// <summary>
    /// Интерфейс для работы с консолью.
    /// </summary>
    public interface IConsoleHelper
    {
        void WriteLine(string message);
        void Write(string message);
        string ReadLine();
        void ClearScreen();
    }
}
