namespace ConsoleEmulator
{    
    public readonly struct ParsedCommand
    {
        public ParsedCommand(string command, string[] args)
        {
            Name = command;
            Args = args;
        }
        public readonly string Name;
        public readonly string[] Args;
    }
}