namespace ConsoleEmulator
{
    public static class Parser
    {
        public static bool TryParse(string? input, out ParsedCommand parsedCommand)
        {
            parsedCommand = default;
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            string[] splittedInput = input.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (splittedInput.Length < 1)
            {
                return false;
            }

            string command = splittedInput[0];
            string[] args = splittedInput.Skip(1).ToArray();
            parsedCommand = new ParsedCommand(command, args);
            return true;
        }
    }
}