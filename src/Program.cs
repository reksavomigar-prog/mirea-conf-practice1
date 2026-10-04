namespace ConsoleEmulator
{
    internal static class Program
    {
        private const string VfsRoot = "src";
        private static string _vfs = VfsRoot;
        private static void Main(string[] args)
        {
            EmulatorConfig emulatorConfig = GetConfig(args);
            Console.WriteLine($"[Config] VFS: {emulatorConfig.VfsPath ?? "(не задан)"}");
            Console.WriteLine($"[Config] Script: {emulatorConfig.ScriptPath ?? "(не задан)"}");
            SetVfs(emulatorConfig.VfsPath);
            HandleScript(emulatorConfig.ScriptPath);
            RunMainLoop();
        }
        private static void HandleScript(string? scriptPath)
        {
            if (scriptPath == null)
            {
                return;
            }
            if (!File.Exists(scriptPath))
            {
                Console.WriteLine($"Файл стартового скрипта {scriptPath} не найден");
                return;
            }
            string[] lines = File.ReadAllLines(scriptPath);
            foreach(var line in lines)
            {
                Console.WriteLine($"{_vfs}>{line}");
                if (Parser.TryParse(line, out ParsedCommand command))
                {
                    if (command.Name == "exit")
                    {
                        break;
                    }
                    CommandExecutor.ExecuteCommand(command.Name, command.Args);
                }
            }
        }
        private static void SetVfs(string? vfsPath)
        {
            _vfs = vfsPath == null ? VfsRoot : vfsPath;
        }
        
        private static EmulatorConfig GetConfig(string[] args)
        {
            string? vfsPath = null;
            string? scriptPath = null;
            for (int i = 0; i < args.Length; i++)
            {
                bool isNextExists = i+1 < args.Length;
                if (args[i] == "--vfs" && isNextExists)
                {
                    vfsPath = args[i+1];
                    i++;
                }
                else if (args[i] == "--script" && isNextExists)
                {
                    scriptPath = args[i+1];
                    i++;
                }
                
            }
            return new EmulatorConfig(vfsPath, scriptPath);
        }
        private static void RunMainLoop()
        {
            while (true)
            {
                Console.Write($"{_vfs}>");
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