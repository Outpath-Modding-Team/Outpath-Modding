namespace Outpath_Modding.GameConsole.Components
{
    public interface ICommand
    {
        public string Command { get; set; }

        public string[] Abbreviate { get; set; }

        public string Description { get; set; }

        bool Execute(string[] args, out string reply);
    }
}
