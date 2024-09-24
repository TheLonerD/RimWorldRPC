using UnityEngine;
using Verse;

namespace RimRPC
{
    public class RwrpcSettings : ModSettings
    {
        // Ajoutez les propriétés manquantes
        public bool RpcCustomBottom = false;
        public string RpcCustomBottomText = "";
        public bool RpcCustomTop = false;
        public string RpcCustomTopText = "";
        public bool RpcDay = false;
        public bool RpcHour = false;
        public bool RpcQuadrum = false;
        public bool RpcYear = false;
        public bool RpcYearShort = false;
        public bool RpcBiome = false;
        public bool RpcColony = false;
        public bool RpcColonistCount = false;

        // Ajoutez d'autres propriétés si nécessaire

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref RpcCustomBottom, "RpcCustomBottom", false);
            Scribe_Values.Look(ref RpcCustomBottomText, "RpcCustomBottomText", "");
            Scribe_Values.Look(ref RpcCustomTop, "RpcCustomTop", false);
            Scribe_Values.Look(ref RpcCustomTopText, "RpcCustomTopText", "");
            Scribe_Values.Look(ref RpcDay, "RpcDay", false);
            Scribe_Values.Look(ref RpcHour, "RpcHour", false);
            Scribe_Values.Look(ref RpcQuadrum, "RpcQuadrum", false);
            Scribe_Values.Look(ref RpcYear, "RpcYear", false);
            Scribe_Values.Look(ref RpcYearShort, "RpcYearShort", false);
            Scribe_Values.Look(ref RpcBiome, "RpcBiome", false);
            Scribe_Values.Look(ref RpcColony, "RpcColony", false);
            Scribe_Values.Look(ref RpcColonistCount, "RpcColonistCount", false);

            // Sauvegardez d'autres propriétés si nécessaire
        }
    }

    public class RWRPCMod : Mod
    {
        public static RwrpcSettings Settings;

        public static string ModDirectory;

        public RWRPCMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<RwrpcSettings>();
            ModDirectory = content.RootDir;

            // Initialisation du mod
            RimRPC.BootMeUp();
        }

        public override string SettingsCategory() => "RimRPC Mod";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            // Si vous avez des paramètres à afficher, implémentez l'interface ici
        }
    }
}
