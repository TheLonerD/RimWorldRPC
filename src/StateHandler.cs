using Verse;
using RimWorld;
using System.Linq;

namespace RimRPC
{
    internal class StateHandler
    {
        public static void MenuState()
        {
            RimRPC.UpdatePresence();
        }

        public static void PushState(Map map)
        {
            RimRPC.UpdatePresence();
        }

        private static string GetColonyName()
        {
            return Current.Game?.World?.factionManager?.OfPlayer?.Name ?? "Colony with no name";
        }
    }
}