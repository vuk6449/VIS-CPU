namespace VukCPU
{
    internal class ViscpuMain
    {
        // Registers
        public static string Ax = "0000"; // Classic register
        public static string Bx = "0000"; // Classic register
        public static string Dp = "0000"; // Fallback for when PT takes an invalid input (e.g. pt 46)

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
                    if (op == "a2") Console.WriteLine(Ax);
                    else if (op == "b2") Console.WriteLine(Bx);
                    else Console.WriteLine(Dp);
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
    }
}
