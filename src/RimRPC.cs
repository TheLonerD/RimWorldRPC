using HarmonyLib;
using System;
using System.Reflection;
using Verse;

namespace RimRPC
{
    public class Mod : Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            Log.Message("RimRPC: Initializing mod...");
            var harmony = new Harmony("weilbyte.rimworld.rimrpc");

            MethodInfo targetmethod = AccessTools.Method(typeof(GenScene), "GoToMainMenu");
            HarmonyMethod postfixmethod = new HarmonyMethod(typeof(RimRPC).GetMethod("GoToMainMenu_Postfix"));

            try
            {
                harmony.Patch(targetmethod, null, postfixmethod);
                Log.Message("RimRPC: Main menu patch applied successfully.");
            }
            catch (Exception ex)
            {
                Log.Error("RimRPC: Failed to apply main menu patch. Exception: " + ex);
            }

            RimRPC.BootMeUp(content);
        }
    }

    public class RimRPC
    {
        internal static DiscordRPC.RichPresence Presence;
        internal static string Colony;
        internal static int onDay;
        internal static long Started = (DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks) / TimeSpan.TicksPerSecond;
        internal static string LastEvent; // Nouvelle variable

        public static void BootMeUp(ModContentPack content)
        {
            Log.Message("RimRPC: Initializing Discord RPC...");
            DiscordRPC.InitLib(content);
            DiscordRPC.EventHandlers eventHandlers = default;
            eventHandlers.ReadyCallback = (DiscordRPC.ReadyCallback)Delegate.Combine(eventHandlers.ReadyCallback, new DiscordRPC.ReadyCallback(ReadyCallback));
            eventHandlers.DisconnectedCallback = (DiscordRPC.DisconnectedCallback)Delegate.Combine(eventHandlers.DisconnectedCallback, new DiscordRPC.DisconnectedCallback(DisconnectedCallback));
            eventHandlers.ErrorCallback = (DiscordRPC.ErrorCallback)Delegate.Combine(eventHandlers.ErrorCallback, new DiscordRPC.ErrorCallback(ErrorCallback));
            eventHandlers.JoinCallback = (DiscordRPC.JoinCallback)Delegate.Combine(eventHandlers.JoinCallback, new DiscordRPC.JoinCallback(JoinCallback));
            eventHandlers.SpectateCallback = (DiscordRPC.SpectateCallback)Delegate.Combine(eventHandlers.SpectateCallback, new DiscordRPC.SpectateCallback(SpectateCallback));
            eventHandlers.RequestCallback = (DiscordRPC.RequestCallback)Delegate.Combine(eventHandlers.RequestCallback, new DiscordRPC.RequestCallback(RequestCallback));

            DiscordRPC.Initialize("428272711702282252", ref eventHandlers, true, "0612");

            Presence = new DiscordRPC.RichPresence
            {
                LargeImageKey = "logo",
                State = "RPC_MainMenu".Translate()
            };

            DiscordRPC.UpdatePresence(ref Presence);
            ReadyCallback();
            Log.Message("RimRPC: Discord RPC initialized.");
        }

        public static void UpdateLastEvent(string eventDescription)
        {
            Log.Message($"RimRPC: Attempting to update last event with: {eventDescription}");
            if (RWRPCMod.Settings.ShowLastEvent)
            {
                LastEvent = eventDescription;
                Presence.Details = LastEvent;
                Log.Message($"RimRPC: Updating last event: {LastEvent}");
                DiscordRPC.UpdatePresence(ref Presence);
            }
            else
            {
                Log.Message("RimRPC: ShowLastEvent is disabled.");
            }
        }

        private static void RequestCallback(DiscordRPC.JoinRequest request)
        {
        }

        private static void SpectateCallback(string secret)
        {
        }

        private static void JoinCallback(string secret)
        {
        }

        private static void ErrorCallback(int errorCode, string message)
        {
            Log.Message($"RichPresence :: ErrorCallback: {errorCode} {message}");
        }

        private static void DisconnectedCallback(int errorCode, string message)
        {
            Log.Message($"RichPresence :: DisconnectedCallback: {errorCode} {message}");
        }

        private static void ReadyCallback()
        {
            Log.Message("RichPresence :: Running");
        }

        public static void GoToMainMenu_Postfix()
        {
            StateHandler.MenuState();
        }

        public static void Shutdown()
        {
            DiscordRPC.Shutdown();
            DiscordRPC.Cleanup();
        }
    }
}
