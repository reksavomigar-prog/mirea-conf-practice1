namespace ConsoleEmulator
{
    public static class CommandExecutor
    {
        public static void ExecuteCommand(string command, string[] args)
        {
            switch (command)
            {
                case "cd":
                    ExecuteCd(args);
                    break;
                case "ls":
                    ExecuteLs(args);
                    break;
                default:
                    Console.WriteLine($"{command} не является внутренней или внешней командой исполняемой программой или пакетным файлом");
                    break;
            }
        }
        private static void ExecuteCd(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Missing argument");
            }
            else
            {
                Console.WriteLine($"cd zaglushka: {string.Join(", ", args)}");
            }
        }
        private static void ExecuteLs(string[] args)
        {
            Console.WriteLine($"ls zaglushka: {string.Join(", ", args)}");
        }
    }
}