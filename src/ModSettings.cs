using UnityEngine;
using Verse;

namespace RimRPC
{
    public class RwrpcSettings : ModSettings
    {
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
        }

        public override string SettingsCategory() => "RimRPC Mod";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();
            listing.Begin(inRect);

            // Variable pour détecter les changements
            bool settingsChanged = false;

            listing.Gap();
            listing.Label("Texte personnalisé :");
            listing.GapLine();

            // Pour RpcCustomBottom
            bool previousRpcCustomBottom = Settings.RpcCustomBottom;
            listing.CheckboxLabeled("Activer le texte personnalisé en bas", ref Settings.RpcCustomBottom, "Affiche un texte personnalisé en bas de la Rich Presence.");
            if (Settings.RpcCustomBottom != previousRpcCustomBottom)
            {
                settingsChanged = true;
            }

            if (Settings.RpcCustomBottom)
            {
                string previousRpcCustomBottomText = Settings.RpcCustomBottomText;
                listing.Label("Texte personnalisé en bas :");
                Settings.RpcCustomBottomText = listing.TextEntry(Settings.RpcCustomBottomText);
                if (Settings.RpcCustomBottomText != previousRpcCustomBottomText)
                {
                    settingsChanged = true;
                }
            }

            // Pour RpcCustomTop
            bool previousRpcCustomTop = Settings.RpcCustomTop;
            listing.CheckboxLabeled("Activer le texte personnalisé en haut", ref Settings.RpcCustomTop, "Affiche un texte personnalisé en haut de la Rich Presence.");
            if (Settings.RpcCustomTop != previousRpcCustomTop)
            {
                settingsChanged = true;
            }

            if (Settings.RpcCustomTop)
            {
                string previousRpcCustomTopText = Settings.RpcCustomTopText;
                listing.Label("Texte personnalisé en haut :");
                Settings.RpcCustomTopText = listing.TextEntry(Settings.RpcCustomTopText);
                if (Settings.RpcCustomTopText != previousRpcCustomTopText)
                {
                    settingsChanged = true;
                }
            }

            // Groupe pour les informations du temps en fessant pareil que RpcCustomBottom mais en les groupant
            listing.Gap();
            listing.Label("Informations du temps :");
            listing.GapLine();

            bool previousRpcDay = Settings.RpcDay;
            listing.CheckboxLabeled("Jour", ref Settings.RpcDay, "Affiche le jour en jeu.");
            if (Settings.RpcDay != previousRpcDay)
            {
                settingsChanged = true;
            }
            
            bool previousRpcHour = Settings.RpcHour;
            listing.CheckboxLabeled("Heure", ref Settings.RpcHour, "Affiche l'heure en jeu.");
            if (Settings.RpcHour != previousRpcHour)
            {
                settingsChanged = true;
            }

            bool previousRpcQuadrum = Settings.RpcQuadrum;
            listing.CheckboxLabeled("Quadrimestre", ref Settings.RpcQuadrum, "Affiche le quadrimestre en jeu.");
            if (Settings.RpcQuadrum != previousRpcQuadrum)
            {
                settingsChanged = true;
            }

            bool previousRpcYear = Settings.RpcYear;
            listing.CheckboxLabeled("Année", ref Settings.RpcYear, "Affiche l'année en jeu.");
            if (Settings.RpcYear != previousRpcYear)
            {
                settingsChanged = true;
            }

            bool previousRpcYearShort = Settings.RpcYearShort;
            listing.CheckboxLabeled("Année courte", ref Settings.RpcYearShort, "Affiche l'année en jeu sous forme courte.");
            if (Settings.RpcYearShort != previousRpcYearShort)
            {
                settingsChanged = true;
            }

            // Groupe pour les informations de la colonie en fessant pareil que RpcCustomBottom mais en les groupant
            listing.Gap();
            listing.Label("Informations de la colonie :");
            listing.GapLine();

            bool previousRpcBiome = Settings.RpcBiome;
            listing.CheckboxLabeled("Biome", ref Settings.RpcBiome, "Affiche le biome actuel de la colonie.");
            if (Settings.RpcBiome != previousRpcBiome)
            {
                settingsChanged = true;
            }

            bool previousRpcColony = Settings.RpcColony;
            listing.CheckboxLabeled("Nom de la colonie", ref Settings.RpcColony, "Affiche le nom de la colonie actuelle.");
            if (Settings.RpcColony != previousRpcColony)
            {
                settingsChanged = true;
            }

            bool previousRpcColonistCount = Settings.RpcColonistCount;
            listing.CheckboxLabeled("Nombre de colons", ref Settings.RpcColonistCount, "Affiche le nombre de colons dans la colonie.");
            if (Settings.RpcColonistCount != previousRpcColonistCount)
            {
                settingsChanged = true;
            }

            listing.End();

            if (settingsChanged)
            {
                // Sauvegarder les paramètres
                Settings.Write();

                // Mettre à jour la Rich Presence
                RimRPC.UpdatePresence();
            }
        }
    }
}
