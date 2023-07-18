using Outpath_Modding.GameConsole.Components;

namespace Outpath_Modding.GameConsole.Commands
{
    public class Clear : ICommand
    {
        public string Command { get; set; } = "clear";
        public string[] Abbreviate { get; set; } = new string[] { "cl", "cr" };
        public string Description { get; set; } = "Clear the console";

        public bool Execute(string[] args, out string reply)
        {
            ConsoleManager.Console.ClearConsole();
            reply = "Console cleared!";
            return true;
        }
    }
}
