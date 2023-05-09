namespace Outpath_Modding.API.Features
{
    public static class Game
    {
        public static bool IsGameStarted => GameManager.instance != null;
    }
}
