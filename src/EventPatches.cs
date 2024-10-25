using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Reflection;

namespace RimRPC
{
    [StaticConstructorOnStartup]
    public static class EventPatches
    {
        static EventPatches()
        {
            Log.Message("RimRPC: Initializing Harmony patches...");
            var harmony = new Harmony("com.rimworld.mod.rimrpc");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Messages), nameof(Messages.Message), new Type[] { typeof(Message), typeof(bool) })]
        public static class Messages_Message_Patch
        {
            public static void Postfix(Message msg, bool historical = true)
            {
                RimRPC.UpdateLastEvent($"Event: {msg.text}");
            }
        }

        [HarmonyPatch(typeof(Root), nameof(Root.Shutdown))]
        public static class Root_Shutdown_Patch
        {
            public static void Postfix()
            {
                RimRPC.Shutdown();
            }
        }
    }
}
