namespace Outpath_Modding.API.Features
{
    public static class World
    {
        public static Player LocalPlayer
        {
            get
            {
                if (LocalPlayer == null)
                    LocalPlayer = new Player(PlayerHealth.instance, PlayerMovement.instance);
                return LocalPlayer;
            }
            private set { if (LocalPlayer == null) LocalPlayer = value; }
        }

        public static float Minutes
        {
            get
            {
                if (Game.IsGameStarted)
                    return GameManager.instance.seconds;
                else return 0;
            }
            set
            {
                if (Game.IsGameStarted)
                    GameManager.instance.seconds = value;
            }
        }

        public static int Hours
        {
            get
            {
                if (Game.IsGameStarted)
                    return GameManager.instance.hours;
                else return 0;
            }
            set
            {
                if (Game.IsGameStarted)
                    GameManager.instance.hours = value;
            }
        }
    }
}