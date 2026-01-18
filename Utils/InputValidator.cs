using System;

namespace HRSystem.Utils
{
    /// <summary>
    /// Экземплярная реализация валидатора ввода, использующая абстракцию консоли.
    /// </summary>
    public class InputValidator : IInputValidator
    {
        private readonly IConsoleHelper _console;

        public InputValidator(IConsoleHelper console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public string ValidateString(string prompt, string errorMessage = "Введите непустое значение")
        {
            while (true)
            {
                _console.Write($"  ➤ {prompt}: ");
                var input = _console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
                _console.WriteLine(errorMessage);
            }
        }

        public int ValidateInteger(string prompt, string errorMessage = "Введите целое число")
        {
            while (true)
            {
                _console.Write($"  ➤ {prompt}: ");
                if (int.TryParse(_console.ReadLine(), out var result))
                    return result;
                _console.WriteLine(errorMessage);
            }
        }

        public decimal ValidateDecimal(string prompt, string errorMessage = "Введите число")
        {
            while (true)
            {
                _console.Write($"  ➤ {prompt}: ");
                if (decimal.TryParse(_console.ReadLine(), out var result))
                    return result;
                _console.WriteLine(errorMessage);
            }
        }
    }
}
