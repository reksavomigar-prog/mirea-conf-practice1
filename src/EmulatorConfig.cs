namespace ConsoleEmulator
{    
    public readonly struct EmulatorConfig
    {
        public EmulatorConfig(string? vfsPath, string? scriptPath)
        {
            VfsPath = vfsPath;
            ScriptPath = scriptPath;
        }
        public readonly string? VfsPath;
        public readonly string? ScriptPath;
    }
}