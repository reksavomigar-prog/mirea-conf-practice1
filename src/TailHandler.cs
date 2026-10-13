using System.Text;

namespace ConsoleEmulator
{
    public class TailHandler
    {
        public VirtualFileSystem? Vfs; 
        public const int sectionLength = 10;
        public TailHandler(VirtualFileSystem? vfs)
        {
            Vfs = vfs;
        }

        public void ExecuteTail(string[] args)
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
            foreach (string line in lines.TakeLast(sectionLength))
            {
                Console.WriteLine(line);
            }
        }
  
    }
}