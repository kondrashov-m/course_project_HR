using System;

namespace HRSystem.Commands
{
    /// <summary>
    /// Интерфейс команды.
    /// </summary>
    public interface ICommand
    {
        string Name { get; }
        void Execute();
    }
}
