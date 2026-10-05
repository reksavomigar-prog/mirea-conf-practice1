namespace ConsoleEmulator;

public static class PathResolver
{
    public static VfsNode? Resolve(VfsNode root, VfsNode current, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return current;
        }

        VfsNode node = path[0] == '/' ? root : current;
        string[] parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            if (part == ".") continue;

            if (part == "..")
            {
                node = node.Parent ?? node;
                continue;
            }

            if (!node.Children.TryGetValue(part, out VfsNode? next))
            {
                return null;
            }

            node = next;
        }

        return node;
    }
}