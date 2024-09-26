using Verse;

namespace RimRPC
{
    public class DiscordGameComponent : GameComponent
    {
        public DiscordGameComponent(Game game) : base()
        {
            
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();

            if (RimRPC.discord != null)
            {
                RimRPC.discord.RunCallbacks();
            }
        }
    }

    public class RimRPCGameComponent : GameComponent
    {
        private bool initialized = false;
        public RimRPCGameComponent(Game game) : base()
        {
            
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();

            // Initialisation delayed until the game is ready
            if (!initialized && Current.Game != null && Find.TickManager != null)
            {
                RimRPC.BootMeUp();
                initialized = true;
            }

            // Call Update() every 60 ticks (1 second)
            if (initialized && Find.TickManager.TicksGame % 60 == 0)
            {
                RimRPC.Update();
                RimRPC.UpdatePresence();
            }
        }
    }
}