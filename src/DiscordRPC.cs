using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RimRPC
{
    internal class DiscordRPC
    {
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

        static DiscordRPC()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string libraryPath = Path.Combine(basePath, "Mods/3291415439/Lib/");
            string targetPath = Path.Combine(basePath, "MonoBleedingEdge/EmbedRuntime/");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                libraryPath = Path.Combine(libraryPath, IntPtr.Size == 8 ? "x64/discord-rpc.dll" : "x86/discord-rpc.dll");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                libraryPath = Path.Combine(libraryPath, "discord-rpc.so");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                libraryPath = Path.Combine(libraryPath, "discord-rpc.dylib");
            }

            string targetLibraryPath = Path.Combine(targetPath, Path.GetFileName(libraryPath));
            try
            {
                if (!File.Exists(targetLibraryPath))
                {
                    File.Copy(libraryPath, targetLibraryPath);
                }

                LoadLibrary(targetLibraryPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void LoadLibrary(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                IntPtr handle = LoadLibraryWindows(path);
                if (handle == IntPtr.Zero)
                {
                    throw new Exception($"Failed to load library {path}, error: {Marshal.GetLastWin32Error()}");
                }
            }
            else
            {
                IntPtr handle = dlopen(path, 2); // RTLD_NOW
                if (handle == IntPtr.Zero)
                {
                    throw new Exception($"Failed to load library {path}");
                }
            }
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibraryWindows(string lpFileName);

        [DllImport("libdl.so", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Initialize")]
        public static extern void Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

        [DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Shutdown")]
        public static extern void Shutdown();

        [DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_RunCallbacks")]
        public static extern void RunCallbacks();

        [DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdatePresence")]
        public static extern void UpdatePresence(ref RichPresence presence);

        [DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Respond")]
        public static extern void Respond(string userId, Reply reply);
    }
}