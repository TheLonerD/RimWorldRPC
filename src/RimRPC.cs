using System;
using DiscordGameSDKWrapper;
using RimWorld;
using Verse;
using UnityEngine;


namespace RimRPC
{
    [StaticConstructorOnStartup]
    public class RimRPC
    {
        private const long ClientId = 1288106578825969816; // Votre application ID

        internal static Discord discord;
        internal static long Started = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        internal static ActivityManager activityManager;
        internal static Activity activity;

        static RimRPC()
        {
            BootMeUp();
        }

        public static void BootMeUp()
        {
            Initialize();
            UpdatePresence();
        }

        public static void Initialize()
        {
            try
            {
                discord = new Discord(ClientId, (UInt64)CreateFlags.NoRequireDiscord);
                activityManager = discord.GetActivityManager();
                activity = new Activity();
                Log.Message("RimRPC : Discord Game SDK initialisé avec le client ID par défaut.");
            }
            catch (Exception ex)
            {
                Log.Error($"RimRPC : Échec de l'initialisation du Discord Game SDK : {ex.Message}");
            }
        }

        public static void UpdatePresence()
        {
            if (discord == null)
            {
                Log.Error("RimRPC : Discord SDK non initialisé.");
                return;
            }

            var settings = RWRPCMod.Settings;

            // Construire l'état (State) et les détails (Details) en fonction des paramètres
            string state = GetActivityState(settings);
            string details = GetActivityDetails(settings);

            activity.State = state;
            activity.Details = details;
            activity.Timestamps = new ActivityTimestamps
            {
                Start = Started
            };
            activity.Assets = new ActivityAssets
            {
                LargeImage = "logo", // Assurez-vous que "logo" est ajouté dans les assets de votre application Discord
                LargeText = "RimWorld"
            };

            activityManager.UpdateActivity(activity, (result) =>
            {
                if (result == Result.Ok)
                {
                    Log.Message("RimRPC : Rich Presence mise à jour avec succès.");
                }
                else
                {
                    Log.Error($"RimRPC : Échec de la mise à jour de la Rich Presence : {result}");
                }
            });
        }

        private static string GetActivityState(RwrpcSettings settings)
        {
            // Construire le texte de l'état en fonction des paramètres
            if (settings.RpcCustomBottom)
            {
                return settings.RpcCustomBottomText;
            }
            else
            {
                string state = "";

                if (settings.RpcColonistCount)
                {
                    int colonistCount = GetColonistCount();
                    state += $"Nombre de colons : {colonistCount}";
                }

                if (settings.RpcBiome)
                {
                    string biome = GetCurrentBiome();
                    if (!string.IsNullOrEmpty(state))
                        state += " | ";
                    state += $"Biome : {biome}";
                }

                // Ajoutez d'autres informations en fonction des paramètres

                if (string.IsNullOrEmpty(state))
                {
                    state = "En jeu";
                }

                return state;
            }
        }

        private static string GetActivityDetails(RwrpcSettings settings)
        {
            // Construire le texte des détails en fonction des paramètres
            if (settings.RpcCustomTop)
            {
                return settings.RpcCustomTopText;
            }
            else
            {
                string details = "";

                if (settings.RpcDay || settings.RpcHour || settings.RpcQuadrum || settings.RpcYear || settings.RpcYearShort)
                {
                    var gameTime = GetGameTimeString(settings);
                    details += gameTime;
                }

                if (settings.RpcColony)
                {
                    string colonyName = GetColonyName();
                    if (!string.IsNullOrEmpty(details))
                        details += " | ";
                    details += $"Colonie : {colonyName}";
                }

                // Ajoutez d'autres informations en fonction des paramètres

                if (string.IsNullOrEmpty(details))
                {
                    details = "Joue à RimWorld";
                }

                return details;
            }
        }

        // Méthodes pour récupérer les informations du jeu
        private static int GetColonistCount()
        {
            return PawnsFinder.AllMaps_FreeColonists.Count;
        }

        private static string GetCurrentBiome()
        {
            var map = Find.CurrentMap;
            if (map != null && map.Biome != null)
            {
                return map.Biome.LabelCap;
            }
            return "N/A";
        }

        private static string GetGameTimeString(RwrpcSettings settings)
        {
            var builder = new System.Text.StringBuilder();

            var tickManager = Find.TickManager;
            if (tickManager != null)
            {
                var daysPassed = GenDate.DaysPassed;
                var quadrum = GenDate.Quadrum(Find.TickManager.TicksAbs, Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile).x);
                var year = GenDate.Year(Find.TickManager.TicksAbs, Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile).x);

                if (settings.RpcDay)
                {
                    builder.Append($"Jour {daysPassed}");
                }
                if (settings.RpcQuadrum)
                {
                    if (builder.Length > 0) builder.Append(" ");
                    builder.Append($"{quadrum}");
                }
                if (settings.RpcYear)
                {
                    if (builder.Length > 0) builder.Append(" ");
                    builder.Append($"Année {year}");
                }
                else if (settings.RpcYearShort)
                {
                    if (builder.Length > 0) builder.Append(" ");
                    builder.Append($"An {year}");
                }
                if (settings.RpcHour)
                {
                    int hour = GenLocalDate.HourOfDay(Find.CurrentMap);
                    if (builder.Length > 0) builder.Append(" ");
                    builder.Append($"Heure {hour}");
                }
            }

            return builder.ToString();
        }

        private static string GetColonyName()
        {
            return Find.World.info.name;
        }

        public static void Shutdown()
        {
            if (discord != null)
            {
                discord.Dispose();
                discord = null;
                Log.Message("RimRPC : Discord SDK fermé.");
            }
        }

        public static void GoToMainMenu_Postfix()
        {
            UpdatePresence();
        }

        // Méthode de mise à jour régulière
        public static void Update()
        {
            if (discord != null)
            {
                discord.RunCallbacks();
            }
        }
    }
}
