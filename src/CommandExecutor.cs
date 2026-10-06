using System.Text;

namespace ConsoleEmulator
{
    public static class CommandExecutor
    {
        public static VirtualFileSystem? Vfs; 
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
                case "tail":
                    ExecuteTail(args);
                    break;
                case "rev":
                    ExecuteRev(args);
                    break;
                case "cp":
                    ExecuteCp(args);
                    break;
                default:
                    Console.WriteLine($"{command} не является внутренней или внешней командой исполняемой программой или пакетным файлом");
                    break;
            }
        }
        private static void ExecuteCd(string[] args)
        {
            
            if (Vfs == null)
            {
                Console.WriteLine("cd: файловая система не инициализирована");
                return;
            }

            if (args.Length > 1)
            {
                Console.WriteLine("cd: слишком много аргументов");
                return;
            }

            string? targetPath = args.Length > 0 ? args[0] : null;

            if (!Vfs.ChangeDirectory(targetPath, out string errorMessage))
            {
                Console.WriteLine(errorMessage);
            }
        }
        private static void ExecuteLs(string[] args)
        {
            if (Vfs == null)
            {
                Console.WriteLine("ls: файловая система не инициализирована");
                return;
            }

            string path = args.Length > 0 ? args[0] : string.Empty;
            VfsNode? target = string.IsNullOrWhiteSpace(path) 
                ? Vfs.CurrentDirectory 
                : PathResolver.Resolve(Vfs.Root, Vfs.CurrentDirectory, path);

            if (target == null)
            {
                Console.WriteLine($"ls: не удалось получить доступ к '{path}': Файл или директория отсутствует");
                return;
            }

            if (!target.IsDirectory)
            {
                Console.WriteLine(target.Name);
                return;
            }

            foreach (var child in target.Children.Values)
            {
                Console.WriteLine(child.Name);
            }
        }

        private static void ExecuteTail(string[] args)
        {
            if (Vfs == null)
            {
                Console.WriteLine("tail: файловая система не инициализирована");
                return;
            }

            if (args.Length == 0)
            {
                Console.WriteLine("tail: пропущен операнд, задающий файл");
                return;
            }

            string filePath = args[0];
            VfsNode? target = PathResolver.Resolve(Vfs.Root, Vfs.CurrentDirectory, filePath);

            if (target == null)
            {
                Console.WriteLine($"tail: невозможно открыть '{filePath}': Файл или директория отсутствует");
                return;
            }

            if (target.IsDirectory)
            {
                Console.WriteLine($"tail: ошибка чтения '{filePath}': Это каталог");
                return;
            }

            if (target.Content == null || target.Content.Length == 0)
            {
                return;
            }
            
            string text = Encoding.UTF8.GetString(target.Content);
            string[] lines = text.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);

            foreach (string line in lines.TakeLast(10))
            {
                Console.WriteLine(line);
            }
        }
        private static void ExecuteRev(string[] args)
        {
            if (Vfs == null)
            {
                Console.WriteLine("rev: файловая система не инициализирована");
                return;
            }

            if (args.Length == 0)
            {
                Console.WriteLine("rev: пропущен операнд, задающий файл");
                return;
            }

            string filePath = args[0];
            VfsNode? targetNode = PathResolver.Resolve(Vfs.Root, Vfs.CurrentDirectory, filePath);

            if (targetNode == null)
            {
                Console.WriteLine($"rev: невозможно открыть '{filePath}': Файл или директория отсутствует");
                return;
            }

            if (targetNode.IsDirectory)
            {
                Console.WriteLine($"rev: ошибка чтения '{filePath}': Это каталог");
                return;
            }

            if (targetNode.Content == null || targetNode.Content.Length == 0)
            {
                return;
            }

            string text = Encoding.UTF8.GetString(targetNode.Content);
            string[] lines = text.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);

            foreach (string line in lines)
            {
                char[] charArray = line.ToCharArray();
                Array.Reverse(charArray);
                Console.WriteLine(new string(charArray));
            }
        }
        private static void ExecuteCp(string[] args)
        {
            if (Vfs == null)
            {
                Console.WriteLine("cp: файловая система не инициализирована");
                return;
            }

            if (args.Length == 0)
            {
                Console.WriteLine("cp: пропущен операнд, задающий файл-источник");
                return;
            }

            if (args.Length == 1)
            {
                Console.WriteLine($"cp: после '{args[0]}' пропущен операнд, задающий целевой файл");
                return;
            }

            if (args.Length > 2)
            {
                Console.WriteLine("cp: слишком много аргументов");
                return;
            }

            if (!Vfs.Copy(args[0], args[1], out string errorMessage))
            {
                Console.WriteLine(errorMessage);
            }
        }
    }
}