using System;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.Utils
{
    /// <summary>
    ///  ласс дл€ валидации ввода пользовател€.
    /// </summary>
    public static class InputValidator
    {
        public static string ValidateString(string prompt, string errorMessage = "¬ведите корректное значение")
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
                else
                    Console.WriteLine(errorMessage);
            }
        }

        public static int ValidateInteger(string prompt, string errorMessage = "¬ведите корректное число")
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;
                else
                    Console.WriteLine(errorMessage);
            }
        }

        public static decimal ValidateDecimal(string prompt, string errorMessage = "¬ведите корректную сумму")
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal result))
                    return result;
                else
                    Console.WriteLine(errorMessage);
            }
        }
    }
}
