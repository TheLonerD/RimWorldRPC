using System;
using System.IO;
using Verse;
using UnityEngine;

namespace RimRPC
{
    [StaticConstructorOnStartup]
    public static class DiscordLibraryCopier
    {
        static DiscordLibraryCopier()
        {
            CopyDiscordLibrary();
        }

        private static void CopyDiscordLibrary()
        {
            string modDirectory = RWRPCMod.ModDirectory;
            string sourcePath = GetSourceLibraryPath(modDirectory);
            string targetPath = GetTargetLibraryPath();

            if (sourcePath == null || targetPath == null)
            {
                Log.Error("RimRPC : Impossible de déterminer les chemins source ou cible pour la bibliothèque Discord.");
                return;
            }

            try
            {
                if (!File.Exists(targetPath))
                {
                    File.Copy(sourcePath, targetPath);
                    Log.Message($"RimRPC : Bibliothèque Discord copiée de '{sourcePath}' vers '{targetPath}'.");
                }
                else
                {
                    Log.Message("RimRPC : La bibliothèque Discord est déjà présente.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"RimRPC : Échec de la copie de la bibliothèque Discord. Exception : {ex.Message}");
            }
        }

        private static string GetSourceLibraryPath(string modDirectory)
        {
            string architecture = "";
            string libName = "";

            if (IntPtr.Size == 8)
            {
                architecture = "x86_64";
            }
            else
            {
                architecture = "x86";
            }

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                libName = "discord_game_sdk.dll";
            }
            else if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                libName = "discord_game_sdk.dylib";
                // For macOS on Apple Silicon, the architecture is ‘aarch64’.
                if (IsRunningOnAppleSilicon())
                {
                    architecture = "aarch64";
                }
            }
            else if (Application.platform == RuntimePlatform.LinuxPlayer)
            {
                libName = "discord_game_sdk.so";
            }
            else
            {
                Log.Error("RimRPC : Plateforme non supportée.");
                return null;
            }

            return Path.Combine(modDirectory, "Lib", architecture, libName);
        }

        private static string GetTargetLibraryPath()
        {
            string dataPath = Application.dataPath; // Path to RimWorldWin64_Data
            string pluginsPath = Path.Combine(dataPath, "Plugins");
            string libName = "";

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                libName = "discord_game_sdk.dll";
            }
            else if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                libName = "discord_game_sdk.dylib";
            }
            else if (Application.platform == RuntimePlatform.LinuxPlayer)
            {
                libName = "discord_game_sdk.so";
            }
            else
            {
                Log.Error("RimRPC : Plateforme non supportée.");
                return null;
            }

            // Make sure the Plugins folder exists
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }

            return Path.Combine(pluginsPath, libName);
        }

        private static bool IsRunningOnAppleSilicon()
        {
            return SystemInfo.processorType.Contains("Apple");
        }

    }
}
