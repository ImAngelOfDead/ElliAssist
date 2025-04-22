using System.Collections.Generic;

public class Command
{//
    public List<string> Keywords { get; set; }
    public string Action { get; set; }
    public string Path { get; set; }
}

public class CommandList
{
    public List<Command> Commands { get; set; }
}