using System;
using DiscordGameSDKWrapper;
using Verse;
using UnityEngine;

namespace RimRPC
{
    public class RimRPC
    {
        private const long ClientId = 1288106578825969816; // Votre application ID

        internal static Discord discord;
        internal static long Started = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public static void BootMeUp()
        {
            Initialize();
            UpdatePresence("Dans le menu principal", null);
        }

        public static void Initialize()
        {
            try
            {
                discord = new Discord(ClientId, (UInt64)CreateFlags.NoRequireDiscord);
                Log.Message("RimRPC : Discord Game SDK initialisé avec le client ID par défaut.");
            }
            catch (Exception ex)
            {
                Log.Error($"RimRPC : Échec de l'initialisation du Discord Game SDK : {ex.Message}");
            }
        }

        public static void UpdatePresence(string state, string details)
        {
            if (discord == null)
            {
                Log.Error("RimRPC : Discord SDK non initialisé.");
                return;
            }

            var activityManager = discord.GetActivityManager();

            var activity = new Activity
            {
                State = state,
                Details = details,
                Timestamps = new ActivityTimestamps
                {
                    Start = Started
                },
                Assets = new ActivityAssets
                {
                    LargeImage = "logo", // Assurez-vous que "logo" est ajouté dans les assets de votre application Discord
                    LargeText = "RimWorld"
                }
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
            UpdatePresence("Dans le menu principal", null);
        }
    }
}