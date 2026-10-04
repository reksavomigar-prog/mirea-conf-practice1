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
                if (Parser.TryParse(input, out ParsedCommand command))
                {
                    if (command.Name == "exit")
                    {
                        break;
                    }
                    CommandExecutor.ExecuteCommand(command.Name, command.Args);
                }
            }
        }

    }
}