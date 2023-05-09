namespace Outpath_Modding.API.Features
{
    public class Player
    {
        public Player(PlayerHealth playerHealth, PlayerMovement playerMovement)
        {
            PlayerHealth = playerHealth;
            PlayerMovement = playerMovement;
        }

        public float MaxHealth
        {
            get => PlayerHealth.maxHealth;
            set => PlayerHealth.maxHealth = value;
        }

        public float CurrHealth
        {
            get => PlayerHealth.currHealth;
            set => PlayerHealth.currHealth = value;
        }

        public float Money
        {
            get => PlayerHealth.money;
            set => PlayerHealth.money = value;
        }

        public float WalkingSpeed
        {
            get => PlayerMovement.playerWalkingSpeed;
            set => PlayerMovement.playerWalkingSpeed = value;
        }

        public float RunningSpeed
        {
            get => PlayerMovement.playerRunningSpeed;
            set => PlayerMovement.playerRunningSpeed = value;
        }

        public float JumpStrength
        {
            get => PlayerMovement.jumpStrength;
            set => PlayerMovement.jumpStrength = value;
        }

        public PlayerHealth PlayerHealth;

        public PlayerMovement PlayerMovement;
    }
}
