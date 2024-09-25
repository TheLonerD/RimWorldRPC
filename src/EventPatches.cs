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
            
            try
            {
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                Log.Message("RimRPC : Tous les patches ont été appliqués avec succès.");
            }
            catch (Exception ex)
            {
                Log.Error($"RimRPC : Échec de l'application des patches. Exception : {ex}");
            }

            // Patch pour le retour au menu principal
            try
            {
                MethodInfo targetMethod = AccessTools.Method(typeof(GenScene), "GoToMainMenu");
                MethodInfo postfixMethod = AccessTools.Method(typeof(RimRPC), "GoToMainMenu_Postfix");

                if (targetMethod != null && postfixMethod != null)
                {
                    harmony.Patch(targetMethod, postfix: new HarmonyMethod(postfixMethod));
                    Log.Message("RimRPC : Patch pour le menu principal appliqué avec succès.");
                }
                else
                {
                    Log.Error("RimRPC : Impossible de trouver les méthodes nécessaires pour le patch du menu principal.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"RimRPC : Échec de l'application du patch pour le menu principal. Exception : {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(Messages))]
    [HarmonyPatch("Message", new Type[] { typeof(string), typeof(MessageTypeDef), typeof(bool) })]
    public static class Messages_Message_Patch
    {
        public static void Postfix(string text, MessageTypeDef def, bool historical)
        {
            // Mettez à jour la Rich Presence ou enregistrez l'événement si nécessaire
            Log.Message($"RimRPC : Message reçu - {text}");
        }
    }
}