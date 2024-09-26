using Verse;

namespace RimRPC
{
    public class DiscordGameComponent : GameComponent
    {
        public DiscordGameComponent(Game game) : base()
        {
            // Votre code d'initialisation si nécessaire
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
            // Votre code d'initialisation si nécessaire
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();

            // Initialisation retardée jusqu'à ce que le jeu soit prêt
            if (!initialized && Current.Game != null && Find.TickManager != null)
            {
                RimRPC.BootMeUp();
                initialized = true;
            }

            // Appeler Update() toutes les 60 ticks (1 seconde)
            if (initialized && Find.TickManager.TicksGame % 60 == 0)
            {
                RimRPC.Update();
                RimRPC.UpdatePresence();
            }
        }
    }
}