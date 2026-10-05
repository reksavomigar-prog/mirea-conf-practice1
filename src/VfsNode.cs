public class VfsNode
{
    public string Name {get;set;} = "";
    public bool IsDirectory{get; private set;}
    public byte[]? Content {get; set;}
    public VfsNode? Parent{get;set;}
    public Dictionary<string, VfsNode> Children {get;} = new();

    public VfsNode(bool isDirectory)
    {
        IsDirectory = isDirectory;
    }
    
}