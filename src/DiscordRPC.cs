using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Verse;

namespace RimRPC
{
    internal class DiscordRPC
    {
        public static class DiscordFunctions
        {
            public static class Const
            {
                public const string Init = "Discord_Initialize";
                public const string Shutdown = "Discord_Shutdown";
                public const string RunCallbacks = "Discord_RunCallbacks";
                public const string UpdatePresence = "Discord_UpdatePresence";
                public const string Respond = "Discord_Respond";
            }

            public static DiscordInit Init { get; set; }
            public static DiscordShutdown Shutdown { get; set; }
            public static DiscordRunCallbacks RunCallbacks { get; set; }
            public static DiscordUpdatePresence UpdatePresence { get; set; }
            public static DiscordRespond Respond { get; set; }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DiscordInit(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DiscordShutdown();
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DiscordRunCallbacks();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DiscordUpdatePresence(ref RichPresence presence);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DiscordRespond(string userId, Reply reply);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ReadyCallback();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DisconnectedCallback(int errorCode, string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErrorCallback(int errorCode, string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void JoinCallback(string secret);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SpectateCallback(string secret);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void RequestCallback(JoinRequest request);

        public struct EventHandlers
        {
            public ReadyCallback ReadyCallback;
            public DisconnectedCallback DisconnectedCallback;
            public ErrorCallback ErrorCallback;
            public JoinCallback JoinCallback;
            public SpectateCallback SpectateCallback;
            public RequestCallback RequestCallback;
        }

        [Serializable]
        public struct RichPresence
        {
            public string State;
            public string Details;
            public long StartTimestamp;
            public long EndTimestamp;
            public string LargeImageKey;
            public string LargeImageText;
            public string SmallImageKey;
            public string SmallImageText;
            public string PartyId;
            public int PartySize;
            public int PartyMax;
            public string MatchSecret;
            public string JoinSecret;
            public string SpectateSecret;
            public bool Instance;
        }

        [Serializable]
        public struct JoinRequest
        {
            public string UserId;
            public string Username;
            public string Avatar;
        }

        public enum Reply
        {
            No,
            Yes,
            Ignore
        }

        private static IntPtr _handle = IntPtr.Zero;
        private static Dictionary<string, IntPtr> _functions = new Dictionary<string, IntPtr>();
        private static bool _init = false;

        public static void InitLib(ModContentPack content)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory; // Game Path
            string libraryPath = Path.Combine(content.RootDir, "Lib");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                libraryPath = Path.Combine(libraryPath, IntPtr.Size == 8 ? Path.Combine("x64", "discord-rpc.dll") : Path.Combine("x86", "discord-rpc.dll"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                libraryPath = Path.Combine(libraryPath, "discord-rpc.so");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                libraryPath = Path.Combine(libraryPath, "discord-rpc.dylib");
            }

            try
            {
                _handle = LoadLibrary(libraryPath);
                
                _functions[DiscordFunctions.Const.Init] = LoadFunction(DiscordFunctions.Const.Init);
                DiscordFunctions.Init = Marshal.GetDelegateForFunctionPointer<DiscordInit>(_functions[DiscordFunctions.Const.Init]);
                _functions[DiscordFunctions.Const.Shutdown] = LoadFunction(DiscordFunctions.Const.Shutdown);
                DiscordFunctions.Shutdown = Marshal.GetDelegateForFunctionPointer<DiscordShutdown>(_functions[DiscordFunctions.Const.Shutdown]);
                _functions[DiscordFunctions.Const.RunCallbacks] = LoadFunction(DiscordFunctions.Const.RunCallbacks);
                DiscordFunctions.RunCallbacks = Marshal.GetDelegateForFunctionPointer<DiscordRunCallbacks>(_functions[DiscordFunctions.Const.RunCallbacks]);
                _functions[DiscordFunctions.Const.UpdatePresence] = LoadFunction(DiscordFunctions.Const.UpdatePresence);
                DiscordFunctions.UpdatePresence = Marshal.GetDelegateForFunctionPointer<DiscordUpdatePresence>(_functions[DiscordFunctions.Const.UpdatePresence]);
                _functions[DiscordFunctions.Const.Respond] = LoadFunction(DiscordFunctions.Const.Respond);
                DiscordFunctions.Respond = Marshal.GetDelegateForFunctionPointer<DiscordRespond>(_functions[DiscordFunctions.Const.Respond]);

                _init = true;
            }
            catch (Exception e)
            {
                Log.Error($"RimRPC: {e.Message}");
            }
        }

        private static IntPtr LoadLibrary(string path)
        {
            IntPtr handle = IntPtr.Zero;
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                handle = LoadLibraryWindows(path);
            }
            else
            {
                handle = dlopen(path, 2); // RTLD_NOW
            }

            if (handle == IntPtr.Zero)
            {
                throw new Exception($"Failed to load library {path}, error: {GetLastError()}");
            }

            return handle;
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "LoadLibrary")]
        private static extern IntPtr LoadLibraryWindows(string lpFileName);
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "FreeLibrary")]
        private static extern IntPtr FreeLibraryWindows(IntPtr hModule);
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("libdl.so", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr dlopen(string fileName, int flags);
        [DllImport("libdl.so", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);
        [DllImport("libdl.so", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern string dlerror();
        [DllImport("libdl.so", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern void dlclose(IntPtr handle);

        private static IntPtr LoadFunction(string funcName)
        {
            var result = IntPtr.Zero;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                result = GetProcAddress(_handle, funcName);
            }
            else
            {
                result = dlsym(_handle, funcName);
            }
            
            if (result == IntPtr.Zero)
                throw new Exception($"Cannot find a function named {funcName}, error: {GetLastError()}");

            return result;
        }

        private static string GetLastError()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return $"0x{Marshal.GetLastWin32Error():x}";
            }
            else
            {
                return dlerror();
            }
        }

        public static void Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId)
        {
            if (!_init)
                return;
            
            DiscordFunctions.Init(applicationId, ref handlers, autoRegister, optionalSteamId);
        }

        public static void Shutdown()
        {
            if (!_init)
                return;

            DiscordFunctions.Shutdown();
        }

        public static void RunCallbacks()
        {
            if (!_init)
                return;

            DiscordFunctions.RunCallbacks();
        }

        public static void UpdatePresence(ref RichPresence presence)
        {
            if (!_init)
                return;

            DiscordFunctions.UpdatePresence(ref presence);
        }

        public static void Respond(string userId, Reply reply)
        {
            if (!_init)
                return;

            DiscordFunctions.Respond(userId, reply);
        }

        public static void Cleanup()
        {
            if (!_init)
                return;

            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    FreeLibraryWindows(_handle);
                }
                else
                {
                    dlclose(_handle);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"RimRPC: Error while cleanup: {ex.Message}");
            }
        }
    }
}
