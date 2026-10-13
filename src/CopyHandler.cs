using System.Text;

namespace ConsoleEmulator
{
    public class CopyHandler
    {
        public VirtualFileSystem? Vfs; 

        public const int NonValidArgumentsCount = 1;
        public const int MaxArgumentsCount = 2;
        public CopyHandler(VirtualFileSystem? vfs)
        {
            Vfs = vfs;
        }

        public void ExecuteCp(string[] args)
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

            if (args.Length == NonValidArgumentsCount)
            {
                Console.WriteLine($"cp: после '{args[0]}' пропущен операнд, задающий целевой файл");
                return;
            }

            if (args.Length > MaxArgumentsCount)
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