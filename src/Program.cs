namespace ConsoleEmulator
{
    internal static class Program
    {
        private const string vfs_root = "src";
        private static void Main()
        {
            while (true)
            {
                Console.Write($"{vfs_root}>");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }
                string[] splittedInput = input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string command = splittedInput[0];
                string[] args = splittedInput.Skip(1).ToArray();
                if (command == "exit")
                {
                    break;
                }
                ExecuteCommand(command,args);
            }
        }
        private static void ExecuteCommand(string command, string[] args)
        {
            switch (command)
            {
                case "cd":
                    if (args.Length < 1)
                    {
                        Console.WriteLine("Missing argument");
                    }
                    else
                    {
                        Console.WriteLine($"cd zaglushka: {string.Join(", ", args)}");
                    }
                    break;
                case "ls":
                    Console.WriteLine($"ls zaglushka: {string.Join(", ", args)}");
                    break;
                default:
                    Console.WriteLine($"{command} не является внутренней или внешней командой исполняемой программой или пакетным файлом");
                    break;
            }
        }
    }
}