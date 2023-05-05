using Outpath_Modding.GameConsole;

namespace Outpath_Modding.Loader
{
    public static class Loader
    {
        public static void Load()
        {
            Paths.SetupModPaths();

            ConsoleManager.SetupConsole();
        }
    }
}