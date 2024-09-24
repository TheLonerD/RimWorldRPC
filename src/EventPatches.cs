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
            var harmony = new Harmony("com.rimworld.mod.rimrpc");
            harmony.PatchAll();

            // Patch pour le retour au menu principal
            MethodInfo targetMethod = AccessTools.Method(typeof(GenScene), "GoToMainMenu");
            HarmonyMethod postfixMethod = new HarmonyMethod(typeof(RimRPC).GetMethod("GoToMainMenu_Postfix"));

            try
            {
                harmony.Patch(targetMethod, null, postfixMethod);
                Log.Message("RimRPC : Patch pour le menu principal appliqué avec succès.");
            }
            catch (Exception ex)
            {
                Log.Error("RimRPC : Échec de l'application du patch pour le menu principal. Exception : " + ex);
            }
        }

        [HarmonyPatch(typeof(Messages))]
        [HarmonyPatch("Message")]
        [HarmonyPatch(new Type[] { typeof(string), typeof(MessageTypeDef), typeof(bool) })]
        public static class Messages_Message_Patch
        {
            public static void Postfix(TaggedString text, MessageTypeDef def)
            {
                // Mettez à jour la Rich Presence ou enregistrez l'événement si nécessaire
            }
        }
    }
}
