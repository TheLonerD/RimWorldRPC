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
        public bool RpcShowGameMessages = true;

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
            Scribe_Values.Look(ref RpcShowGameMessages, "RpcShowGameMessages", true);
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

        public override string SettingsCategory() => "RPC_ModSettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();
            listing.Begin(inRect);

            bool settingsChanged = false;

            listing.Gap();
            listing.Label("RPC_CustomTextLabel".Translate());
            listing.GapLine();

            bool previousRpcCustomBottom = Settings.RpcCustomBottom;
            listing.CheckboxLabeled("RPC_EnableCustomBottomText".Translate(), ref Settings.RpcCustomBottom, "RPC_CustomBottomTextDesc".Translate());
            if (Settings.RpcCustomBottom != previousRpcCustomBottom)
            {
                settingsChanged = true;
            }

            if (Settings.RpcCustomBottom)
            {
                string previousRpcCustomBottomText = Settings.RpcCustomBottomText;
                listing.Label("RPC_CustomBottomTextLabel".Translate());
                Settings.RpcCustomBottomText = listing.TextEntry(Settings.RpcCustomBottomText);
                if (Settings.RpcCustomBottomText != previousRpcCustomBottomText)
                {
                    settingsChanged = true;
                }
            }

            bool previousRpcCustomTop = Settings.RpcCustomTop;
            listing.CheckboxLabeled("RPC_EnableCustomTopText".Translate(), ref Settings.RpcCustomTop, "RPC_CustomTopTextDesc".Translate());
            if (Settings.RpcCustomTop != previousRpcCustomTop)
            {
                settingsChanged = true;
            }

            if (Settings.RpcCustomTop)
            {
                string previousRpcCustomTopText = Settings.RpcCustomTopText;
                listing.Label("RPC_CustomTopTextLabel".Translate());
                Settings.RpcCustomTopText = listing.TextEntry(Settings.RpcCustomTopText);
                if (Settings.RpcCustomTopText != previousRpcCustomTopText)
                {
                    settingsChanged = true;
                }
            }

            listing.Gap();
            listing.Label("RPC_TimeInfo".Translate());
            listing.GapLine();

            bool previousRpcDay = Settings.RpcDay;
            listing.CheckboxLabeled("RPC_ShowDay".Translate(), ref Settings.RpcDay, "RPC_ShowDayDesc".Translate());
            if (Settings.RpcDay != previousRpcDay)
            {
                settingsChanged = true;
            }

            bool previousRpcHour = Settings.RpcHour;
            listing.CheckboxLabeled("RPC_ShowHour".Translate(), ref Settings.RpcHour, "RPC_ShowHourDesc".Translate());
            if (Settings.RpcHour != previousRpcHour)
            {
                settingsChanged = true;
            }

            bool previousRpcQuadrum = Settings.RpcQuadrum;
            listing.CheckboxLabeled("RPC_ShowQuadrum".Translate(), ref Settings.RpcQuadrum, "RPC_ShowQuadrumDesc".Translate());
            if (Settings.RpcQuadrum != previousRpcQuadrum)
            {
                settingsChanged = true;
            }

            bool previousRpcYear = Settings.RpcYear;
            listing.CheckboxLabeled("RPC_ShowYear".Translate(), ref Settings.RpcYear, "RPC_ShowYearDesc".Translate());
            if (Settings.RpcYear != previousRpcYear)
            {
                settingsChanged = true;
            }

            bool previousRpcYearShort = Settings.RpcYearShort;
            listing.CheckboxLabeled("RPC_ShowShortYear".Translate(), ref Settings.RpcYearShort, "RPC_ShowShortYearDesc".Translate());
            if (Settings.RpcYearShort != previousRpcYearShort)
            {
                settingsChanged = true;
            }

            listing.Gap();
            listing.Label("RPC_ColonyInfo".Translate());
            listing.GapLine();

            bool previousRpcBiome = Settings.RpcBiome;
            listing.CheckboxLabeled("RPC_ShowBiome".Translate(), ref Settings.RpcBiome, "RPC_ShowBiomeDesc".Translate());
            if (Settings.RpcBiome != previousRpcBiome)
            {
                settingsChanged = true;
            }

            bool previousRpcColony = Settings.RpcColony;
            listing.CheckboxLabeled("RPC_ShowColonyName".Translate(), ref Settings.RpcColony, "RPC_ShowColonyNameDesc".Translate());
            if (Settings.RpcColony != previousRpcColony)
            {
                settingsChanged = true;
            }

            bool previousRpcColonistCount = Settings.RpcColonistCount;
            listing.CheckboxLabeled("RPC_ShowColonistCount".Translate(), ref Settings.RpcColonistCount, "RPC_ShowColonistCountDesc".Translate());
            if (Settings.RpcColonistCount != previousRpcColonistCount)
            {
                settingsChanged = true;
            }

            listing.Gap();
            listing.Label("RPC_EventSettings".Translate());
            listing.GapLine();

            bool previousRpcShowGameMessages = Settings.RpcShowGameMessages;
            listing.CheckboxLabeled("RPC_ShowGameMessages".Translate(), ref Settings.RpcShowGameMessages, "RPC_ShowGameMessagesDesc".Translate());
            if (Settings.RpcShowGameMessages != previousRpcShowGameMessages)
            {
                settingsChanged = true;
            }

            listing.End();

            if (settingsChanged)
            {
                Settings.Write();
                if (Current.ProgramState == ProgramState.Playing)
                {
                    RimRPC.UpdatePresence();
                }
            }
        }
    }
}
