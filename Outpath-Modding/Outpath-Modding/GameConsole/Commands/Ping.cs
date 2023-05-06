using Outpath_Modding.GameConsole.Components;

namespace Outpath_Modding.GameConsole.Commands
{
    public class Ping : ICommand
    {
        public string Command { get; set; } = "ping";
        public string[] Abbreviate { get; set; } = new string[] { };
        public string Description { get; set; } = "Ping! Pong!";

        public bool Execute(string[] args, out string reply)
        {
            if (args == null || args.Length <= 0)
            {
                reply = "Pong";
                return true;
            }

            if (bool.TryParse(args[0], out bool returnBool))
            {
                reply = "Pong";
                return returnBool;
            }
            else
            {
                reply = "Pong";
                return true;
            }
        }
    }
}
