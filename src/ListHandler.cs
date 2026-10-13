using System.Text;

namespace ConsoleEmulator
{
    public class ListHandler
    {
        public VirtualFileSystem? Vfs; 
        public ListHandler(VirtualFileSystem? vfs)
        {
            Vfs = vfs;
        }


        public void ExecuteLs(string[] args)
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
  
    }
}