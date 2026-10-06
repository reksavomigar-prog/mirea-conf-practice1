public class VfsNode
{
    public string Name {get;set;} = "";
    public bool IsDirectory{get; init;}
    public byte[]? Content {get; set;}
    public VfsNode? Parent{get;set;}
    public Dictionary<string, VfsNode> Children {get;} = new();

    private VfsNode(string name, bool isDirectory, VfsNode? parent)
    {
        Name = name;
        IsDirectory = isDirectory;
        Parent = parent;
    }
    public static VfsNode CreateDirectory(string name, VfsNode? parent = null)
    {
        return new VfsNode(name, true, parent);
    }
    public static VfsNode CreateFile(string name, byte[]? content, VfsNode? parent = null)
    {
        return new VfsNode(name, false, parent){Content = content};
    }

    public VfsNode Clone(string newName, VfsNode? newParent = null)
    {
        if (IsDirectory)
        {
            var dirClone = CreateDirectory(newName, newParent);
            foreach (var (childName, childNode) in Children)
            {
                dirClone.Children[childName] = childNode.Clone(childName, dirClone);
            }
            return dirClone;
        }

        byte[]? contentCopy = Content != null ? (byte[])Content.Clone() : null;
        return CreateFile(newName, contentCopy, newParent);
    }   
    
}