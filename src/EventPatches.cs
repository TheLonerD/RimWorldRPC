using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Linq;
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
                var methods = AccessTools.GetDeclaredMethods(typeof(LetterStack))
                    .Where(m => m.Name == "ReceiveLetter")
                    .ToList();

                foreach (var method in methods)
                {
                    Log.Message($"RimRPC : Méthode ReceiveLetter trouvée : {method.ToString()}");
                }

                // Appliquer les patches
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

    // Patch pour Messages.Message(string, MessageTypeDef, bool)
    [HarmonyPatch(typeof(Messages))]
    [HarmonyPatch("Message", new Type[] { typeof(string), typeof(MessageTypeDef), typeof(bool) })]
    public static class Messages_StringMessage_Patch
    {
        public static void Postfix(string text, MessageTypeDef def, bool historical)
        {
            if (!RWRPCMod.Settings.RpcShowGameMessages) return; 

            Log.Message($"RimRPC : Message reçu (string) - {text}");
            RimRPC.lastEventText = text;
            RimRPC.lastEventTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            RimRPC.UpdatePresence();
        }
    }

    // Patch pour Messages.Message(Message, bool)
    [HarmonyPatch(typeof(Messages))]
    [HarmonyPatch("Message", new Type[] { typeof(Message), typeof(bool) })]
    public static class Messages_Message_Patch
    {
        public static void Postfix(Message msg, bool historical)
        {
            if (!RWRPCMod.Settings.RpcShowGameMessages) return;

            string text = msg.text;
            Log.Message($"RimRPC : Message reçu (Message) - {text}");
            RimRPC.lastEventText = text;
            RimRPC.lastEventTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            RimRPC.UpdatePresence();
        }
    }

    [HarmonyPatch]
    public static class LetterStack_ReceiveLetter_Patch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.DeclaredMethod(
                typeof(LetterStack),
                "ReceiveLetter",
                new Type[] { typeof(Letter), typeof(string), typeof(int), typeof(bool) }
            );
        }

        public static void Postfix(Letter let, string debugInfo, int delayTicks, bool playSound)
        {
            if (!RWRPCMod.Settings.RpcShowGameMessages) return;

            string text = $"Lettre : {let.Label}";
            Log.Message($"RimRPC : Lettre reçue - {text}");
            RimRPC.lastEventText = text;
            RimRPC.lastEventTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            RimRPC.UpdatePresence();
        }
    }
}