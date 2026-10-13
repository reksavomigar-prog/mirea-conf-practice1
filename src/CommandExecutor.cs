using System.Text;

namespace ConsoleEmulator
{
    public static class CommandExecutor
    {
        public static VirtualFileSystem? Vfs; 
        private static ChangeDirectoryHandler? cdHandler = new (Vfs);
        private static ListHandler? listHandler = new(Vfs);
        private static TailHandler? tailHandler = new(Vfs);
        private static ReverseHandler? reverseHandler = new(Vfs);
        private static CopyHandler? copyHandler = new(Vfs);

        public static void Init()
        {
            cdHandler = new (Vfs);
            listHandler = new(Vfs);
            tailHandler = new(Vfs);
            reverseHandler = new(Vfs);
            copyHandler = new(Vfs);
        }
        public static void ExecuteCommand(string command, string[] args)
        {
            switch (command)
            {
                case "cd":
                    cdHandler?.ExecuteCd(args);
                    break;
                case "ls":
                    listHandler?.ExecuteLs(args);
                    break;
                case "tail":
                    tailHandler?.ExecuteTail(args);
                    break;
                case "rev":
                    reverseHandler?.ExecuteRev(args);
                    break;
                case "cp":
                    copyHandler?.ExecuteCp(args);
                    break;
                default:
                    Console.WriteLine($"{command} не является внутренней или внешней командой "
                    + "исполняемой программой или пакетным файлом");
                    break;
            }
        }


    }
}