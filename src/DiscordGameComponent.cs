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
        public RimRPCGameComponent(Game game) : base()
        {
            // Votre code d'initialisation si nécessaire
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();

            // Appeler Update() toutes les 60 ticks (1 seconde)
            if (Find.TickManager.TicksGame % 60 == 0)
            {
                RimRPC.Update();
                RimRPC.UpdatePresence();
            }
        }
    }
}