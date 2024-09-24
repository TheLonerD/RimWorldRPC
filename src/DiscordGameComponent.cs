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
}
