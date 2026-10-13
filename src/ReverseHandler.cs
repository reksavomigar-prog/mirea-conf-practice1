using System.Text;

namespace ConsoleEmulator
{
    public class ReverseHandler
    {
        public VirtualFileSystem? Vfs; 
        public ReverseHandler(VirtualFileSystem? vfs)
        {
            Vfs = vfs;
        }


        public void ExecuteRev(string[] args)
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
  
    }
}