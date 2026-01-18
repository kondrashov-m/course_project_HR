using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

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

        private string ReadConsoleLineW()
        {
            const int STD_INPUT_HANDLE = -10;
            var handle = GetStdHandle(STD_INPUT_HANDLE);
            if (handle == IntPtr.Zero || handle == new IntPtr(-1))
                return string.Empty;

            var buf = new char[1024];
            if (!ReadConsoleW(handle, buf, (uint)buf.Length, out uint read, IntPtr.Zero))
                return string.Empty;

            if (read == 0) return string.Empty;
            // Trim trailing CR/LF
            var s = new string(buf, 0, (int)read);
            return s.TrimEnd('\r', '\n');
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ReadConsoleW(IntPtr hConsoleInput, [Out] char[] lpBuffer, uint nNumberOfCharsToRead, out uint lpNumberOfCharsRead, IntPtr pInputControl);

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
            // On Windows try to read Unicode directly from console first.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !Console.IsInputRedirected)
            {
                try
                {
                    var direct = ReadConsoleLineW();
                    if (!string.IsNullOrEmpty(direct))
                    {
                        TryLogReadlineString(direct);
                        return direct;
                    }
                }
                catch { }
            }

            // Normal read
            var line = Console.ReadLine();
            TryLogReadlineString(line);
            if (!string.IsNullOrEmpty(line))
                return line;

            // (removed Console.ReadKey blocking fallback — it caused partial/truncated input in some terminals)

            // Second fallback on Windows: try to read Unicode directly from console (ReadConsoleW)
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !Console.IsInputRedirected)
            {
                try
                {
                    var direct = ReadConsoleLineW();
                    if (!string.IsNullOrEmpty(direct))
                        return direct;
                }
                catch { }
            }

            // Next fallback: read raw bytes from stdin until newline and try decode
            try
            {
                var stream = Console.OpenStandardInput();
                var buffer = new List<byte>();
                while (true)
                {
                    int b = stream.ReadByte();
                    if (b == -1) break;
                    if (b == 10) break; // LF
                    if (b == 13) continue; // skip CR
                    buffer.Add((byte)b);
                }

                if (buffer.Count == 0)
                    return string.Empty;

                var bytes = buffer.ToArray();
                // If decoding fails, log raw bytes to help diagnose encoding/terminal issues
                string debugPath = Path.Combine(AppContext.BaseDirectory, "console_input_debug.log");
                try
                {
                    var hex = BitConverter.ToString(bytes);
                    File.AppendAllText(debugPath, DateTime.UtcNow.ToString("o") + " " + hex + Environment.NewLine);
                }
                catch { }
                var encodingsToTry = new Encoding[] {
                    Encoding.UTF8,
                    Encoding.GetEncoding(1251),
                    Encoding.GetEncoding(866),
                    Encoding.Default
                };

                foreach (var enc in encodingsToTry)
                {
                    try
                    {
                        var candidate = enc.GetString(bytes).TrimEnd('\r', '\n');
                        if (!string.IsNullOrWhiteSpace(candidate))
                            return candidate;
                    }
                    catch { }
                }

                // nothing decoded properly
                try { Console.WriteLine($"[DEBUG] Не удалось декодировать ввод — сырые байты записаны в {debugPath}"); } catch { }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void TryLogReadlineString(string line)
        {
            try
            {
                if (line != null)
                {
                    var debugPath = Path.Combine(AppContext.BaseDirectory, "console_input_debug.log");
                    var codes = new System.Text.StringBuilder();
                    foreach (var ch in line)
                        codes.Append(((int)ch).ToString("X4")).Append(" ");
                    File.AppendAllText(debugPath, DateTime.UtcNow.ToString("o") + " READLINE_STR_LEN=" + line.Length + " CODES=" + codes.ToString() + Environment.NewLine);
                }
            }
            catch { }
        }
    }
}
