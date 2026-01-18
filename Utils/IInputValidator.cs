using System;

namespace HRSystem.Utils
{
    /// <summary>
    /// Интерфейс для валидации и чтения пользовательского ввода.
    /// </summary>
    public interface IInputValidator
    {
        string ValidateString(string prompt, string errorMessage = "Введите непустое значение");
        int ValidateInteger(string prompt, string errorMessage = "Введите целое число");
        decimal ValidateDecimal(string prompt, string errorMessage = "Введите число");
    }
}
