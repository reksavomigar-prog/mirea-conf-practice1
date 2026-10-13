using System.Text;

namespace ConsoleEmulator
{
    public class ChangeDirectoryHandler
    {
        public VirtualFileSystem? Vfs; 
        public const int ChangeDirectoryMaxArg = 1;
        public ChangeDirectoryHandler(VirtualFileSystem? vfs)
        {
            Vfs = vfs;
        }

        public void ExecuteCd(string[] args)
        {
            if (Vfs == null)
            {
                Console.WriteLine("cd: файловая система не инициализирована");
                return;
            }
            if (args.Length > ChangeDirectoryMaxArg)
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
  
    }
}