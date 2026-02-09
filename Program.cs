// TODO: Use Eax, Ebx, Ewax, Ewbx
// TODO: Make Eax and Ebx actually change Ax and Bx
// TODO: Add Al and Ah and configure them
// TODO: Add Bl and Bh and configure them

using System.Globalization;
using System.Text;

namespace VukCPU
{
    internal class ViscpuMain
    {
        // Registers
        public static string Ax = "0000"; // Classic register
        public static string Bx = "0000"; // Classic register
        public static string Dp = "0000"; // Fallback for when PT takes an invalid input (e.g. pt 46)
        public static string Ewax = "00000000"; // 32-bit Seperated AX
        public static string Ewbx = "00000000"; // 32-bit Seperated BX
        public static string Eax = "00000000"; // Classic register
        public static string Ebx = "00000000"; // Classic register

        private static void Main(string[] args)
        {
            string inputPath = args.Length > 0
                ? args[0]
                : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "main.bin"));

            if (args.Length < 1)
            {
                Warn("WARN: No input file specified. Defaulting to main.bin");
                Console.WriteLine("Usage: VukCPU.exe <code>");
            }

            if (!File.Exists(inputPath))
            {
                Error($"ERR: {inputPath} not found.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(1);
            }

            string code = File.ReadAllText(inputPath);
            Exec(code);
        }

        private static void Exec(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                Error("Invalid code: Empty string");
                return;
            }

            Console.Clear();

            char sc = ' ';
            
            string[] tokens = code.Split(sc, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == "ax")
                {
                    string b1 = tokens[i + 1];
                    string b2 = tokens[i + 2];

                    string word = $"{b1}{b2}".ToLower();
                    Ax = word;
                    i += 2;
                }
                else if (tokens[i] == "bx")
                {
                    string b1 = tokens[i + 1];
                    string b2 = tokens[i + 2];

                    string word = $"{b1}{b2}".ToLower();
                    Bx = word;
                    i += 2;
                }
                else if (tokens[i] == "dp")
                {
                    string b1 = tokens[i + 1];
                    string b2 = tokens[i + 2];

                    string word = $"{b1}{b2}".ToLower();
                    Dp = word;
                    i += 2;
                }
                else if (tokens[i] == "pt" && i + 1 < tokens.Length)
                {
                    string op = tokens[i + 1].ToLower();
                    if (op == "a2") Console.WriteLine(Ascii(Ax));
                    else if (op == "b2") Console.WriteLine(Ascii(Bx));
                    else if (op == "dp") Console.WriteLine(Ascii(Dp));
                    else Console.WriteLine(Ascii(Dp));
                    i++;
                }
            }
        }

        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        private static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static string ToHex(string input)
        {
            if (input == string.Empty)
            {
                return string.Empty;
            }

            byte[] bytes = Encoding.ASCII.GetBytes(input);
            var sb = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }

        private static string Ascii(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                return string.Empty;
            }

            string cleaned = hex.Trim();
            if (cleaned.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                cleaned = cleaned.Substring(2);
            }

            cleaned = cleaned.Replace(" ", string.Empty);

            if (cleaned.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string must have even length.", nameof(hex));
            }

            var bytes = new byte[cleaned.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string pair = cleaned.Substring(i * 2, 2);
                bytes[i] = byte.Parse(pair, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return Encoding.ASCII.GetString(bytes);
        }
    }
}
