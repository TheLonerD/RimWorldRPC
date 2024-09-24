using Verse;
using RimWorld;
using System.Linq;

namespace RimRPC
{
    internal class StateHandler
    {
        public static void MenuState()
        {
            RimRPC.UpdatePresence("Dans le menu principal", null);
        }

        public static void PushState(Map map)
        {
            var world = Current.Game?.World;

            if (world == null)
            {
                RimRPC.UpdatePresence("Dans le menu principal", null);
            }
            else
            {
                // Récupération des informations de jeu
                string colonyName = GetColonyName();
                int colonistCount = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count;

                // Autres informations (à adapter selon vos besoins)
                string state = $"{colonyName} ({colonistCount} colons)";
                string details = "En train de jouer";

                RimRPC.UpdatePresence(state, details);
            }
        }

        private static string GetColonyName()
        {
            return Current.Game?.World?.factionManager?.OfPlayer?.Name ?? "Colonie sans nom";
        }
    }
}