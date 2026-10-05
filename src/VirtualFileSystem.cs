namespace ConsoleEmulator;

public sealed class VirtualFileSystem
{
    public VfsNode Root {get;} = new(true) {Name = "/"};
    public VfsNode CurrentDirectory{get;private set;}

    public VirtualFileSystem()
    {
        CurrentDirectory = Root;
    }
    
    public static VirtualFileSystem LoadFromCsv(string csvPath)
    {
        if (!File.Exists(csvPath))
        {
            throw new Exception($"CSV-файл не найден: {csvPath}");
        }

        var vfs = new VirtualFileSystem();
        string[] lines = File.ReadAllLines(csvPath);

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] parts = line.Split(',');
            if (parts.Length < 2)
            {
                throw new Exception($"Некорректная строка в CSV: '{line}'");
            }
            string filePath = parts[0].Trim();
            string type = parts[1].Trim();
            string content = parts.Length > 2 ? parts[2].Trim() : string.Empty;

            if (type == "dir")
            {
                vfs.CreateDirectory(filePath);
            }
            else if (type == "file")
            {
                byte[] content64 = string.IsNullOrEmpty(content) ? Array.Empty<byte>() : Convert.FromBase64String(content);
                vfs.CreateFile(filePath,content64);
            }
            else
            {
                throw new Exception($"Неизвестный тип узла: {type}");
            }
        }
        return vfs;
    }
    private void CreateDirectory(string path)
    {
        if (path == "/")
        {
            return;
        }
        string[] segments = path.Split('/',StringSplitOptions.RemoveEmptyEntries);
        VfsNode current = Root;

        foreach (string segment in segments)
        {
            if (!current.Children.TryGetValue(segment,out VfsNode? child))
            {
                child = new VfsNode(true)
                {
                    Name = segment,
                    Parent = current
                };
                current.Children[segment] = child;
            }
            current = child;
        }
    }
    private void CreateFile(string path, byte[] content)
    {
        string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
        {
            return;
        }

        VfsNode current = Root;
        for (int i = 0; i < segments.Length - 1; i++)
        {
            string dirName = segments[i];
            if (!current.Children.TryGetValue(dirName, out VfsNode? dir))
            {
                dir = new VfsNode(true)
                {
                    Name = dirName,
                    Parent = current
                };
                current.Children[dirName] = dir;
            }

            current = dir;
        }

        string fileName = segments[^1];
        var fileNode = new VfsNode(false)
        {
            Name = fileName,
            Content = content,
            Parent = current
        };
        current.Children[fileName] = fileNode;
    }
}