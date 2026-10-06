namespace ConsoleEmulator;

public sealed class VirtualFileSystem
{
    public VfsNode Root { get; } = VfsNode.CreateDirectory("/");
    public VfsNode CurrentDirectory { get; private set; }

    public VirtualFileSystem()
    {
        CurrentDirectory = Root;
    }

    public string GetCurrentPath()
    {
        if (CurrentDirectory == Root) return "/";

        string result = string.Empty;
        VfsNode? current = CurrentDirectory;

        while (current != null && current != Root)
        {
            result = "/" + current.Name + result;
            current = current.Parent;
        }

        return result;
    }

    public static VirtualFileSystem LoadFromCsv(string csvPath)
    {
        if (!File.Exists(csvPath))
            throw new Exception($"CSV-файл не найден: {csvPath}");

        var vfs = new VirtualFileSystem();
        string[] lines = File.ReadAllLines(csvPath);

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            vfs.ParseAndCreateNode(line);
        }

        return vfs;
    }

    private void ParseAndCreateNode(string csvLine)
    {
        string[] parts = csvLine.Split(',');
        if (parts.Length < 2)
            throw new Exception($"Некорректная строка в CSV: '{csvLine}'");

        string filePath = parts[0].Trim();
        string type = parts[1].Trim();
        string content = parts.Length > 2 ? parts[2].Trim() : string.Empty;

        if (type == "dir")
        {
            CreateDirectory(filePath);
        }
        else if (type == "file")
        {
            byte[] bytes = string.IsNullOrEmpty(content) ? [] : Convert.FromBase64String(content);
            CreateFile(filePath, bytes);
        }
        else
        {
            throw new Exception($"Неизвестный тип узла: {type}");
        }
    }

    private void CreateDirectory(string path)
    {
        if (path == "/") return;

        string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        VfsNode current = Root;

        foreach (string segment in segments)
        {
            if (!current.Children.TryGetValue(segment, out VfsNode? child))
            {
                child = VfsNode.CreateDirectory(segment, current);
                current.Children[segment] = child;
            }
            current = child;
        }
    }

    private void CreateFile(string path, byte[] content)
    {
        string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0) return;

        VfsNode current = Root;
        for (int i = 0; i < segments.Length - 1; i++)
        {
            string dirName = segments[i];
            if (!current.Children.TryGetValue(dirName, out VfsNode? dir))
            {
                dir = VfsNode.CreateDirectory(dirName, current);
                current.Children[dirName] = dir;
            }
            current = dir;
        }

        string fileName = segments[^1];
        current.Children[fileName] = VfsNode.CreateFile(fileName, content, current);
    }

    public bool ChangeDirectory(string? path, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(path))
        {
            CurrentDirectory = Root;
            return true;
        }

        VfsNode? target = PathResolver.Resolve(Root, CurrentDirectory, path);
        if (target == null)
        {
            errorMessage = $"cd: {path}: Файл или директория отсутствует";
            return false;
        }

        if (!target.IsDirectory)
        {
            errorMessage = $"cd: {path}: Не является директорией";
            return false;
        }

        CurrentDirectory = target;
        return true;
    }

    public bool Copy(string sourcePath, string destPath, out string errorMessage)
    {
        VfsNode? sourceNode = PathResolver.Resolve(Root, CurrentDirectory, sourcePath);
        if (sourceNode == null)
        {
            errorMessage = $"cp: невозможно открыть '{sourcePath}': Файл или директория отсутствует";
            return false;
        }

        if (!TryResolveDestination(sourceNode, destPath, 
            out VfsNode parentDir, out string targetName, out errorMessage))
            return false;

        if (sourceNode.IsDirectory && IsDescendantOf(parentDir, sourceNode))
        {
            errorMessage = $"cp: невозможно скопировать '{sourcePath}' внутрь самого себя";
            return false;
        }

        parentDir.Children[targetName] = sourceNode.Clone(targetName, parentDir);
        return true;
    }

    private bool TryResolveDestination(
        VfsNode sourceNode,
        string destPath,
        out VfsNode parentDir,
        out string targetName,
        out string errorMessage)
    {
        errorMessage = string.Empty;
        VfsNode? destNode = PathResolver.Resolve(Root, CurrentDirectory, destPath);

        if (destNode != null)
        {
            parentDir = destNode.IsDirectory ? destNode : (destNode.Parent ?? Root);
            targetName = destNode.IsDirectory ? sourceNode.Name : destNode.Name;
            return true;
        }

        return TryParseNewDestination(destPath, out parentDir, out targetName, out errorMessage);
    }

    private bool TryParseNewDestination(
        string destPath,
        out VfsNode parentDir,
        out string targetName,
        out string errorMessage)
    {
        errorMessage = string.Empty;
        string trimmedPath = destPath.TrimEnd('/');
        int lastSlashIndex = trimmedPath.LastIndexOf('/');

        if (lastSlashIndex == -1)
        {
            parentDir = CurrentDirectory;
            targetName = trimmedPath;
            return true;
        }

        string parentPath = lastSlashIndex == 0 ? "/" : trimmedPath[..lastSlashIndex];
        targetName = trimmedPath[(lastSlashIndex + 1)..];

        VfsNode? resolvedParent = PathResolver.Resolve(Root, CurrentDirectory, parentPath);
        if (resolvedParent == null || !resolvedParent.IsDirectory)
        {
            parentDir = CurrentDirectory;
            errorMessage = $"cp: невозможно создать обычный файл '{destPath}': Файл или директория отсутствует";
            return false;
        }

        parentDir = resolvedParent;
        return true;
    }

    private static bool IsDescendantOf(VfsNode node, VfsNode potentialAncestor)
    {
        VfsNode? current = node;
        while (current != null)
        {
            if (current == potentialAncestor) return true;
            current = current.Parent;
        }
        return false;
    }
}