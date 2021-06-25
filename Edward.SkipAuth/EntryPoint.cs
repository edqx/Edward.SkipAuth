using System.Collections;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;

namespace Edward.SkipAuth
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class SkipAuthPlugin : BasePlugin
    {
        public const string Id = "dev.weakeyes.skipauth";

        public Harmony Harmony { get; } = new Harmony(Id);

        public ConfigEntry<string> Name { get; private set; }
        
        public static List<string> OfficialServerNames = new List<string>()
        {
            "North America", 
            "Europe",
            "Asia" 
        };

        public override void Load()
        {
            Name = Config.Bind("Fake", "Name", ":>");

            Harmony.PatchAll();
        }

        [HarmonyPatch(typeof(AuthManager._CoConnect_d__4), nameof(AuthManager._CoConnect_d__4.MoveNext))]
        public static class DoNothingInConnect
        {
            public static bool Prefix(AuthManager __instance)
            {
                return SkipAuthPlugin.OfficialServerNames.Contains(ServerManager.Instance.CurrentRegion.Name);
            }
        }

        [HarmonyPatch(typeof(AuthManager._CoWaitForNonce_d__5), nameof(AuthManager._CoWaitForNonce_d__5.MoveNext))]
        public static class DontWaitForNonce
        {
            public static bool Prefix(AuthManager __instance)
            {
                return SkipAuthPlugin.OfficialServerNames.Contains(ServerManager.Instance.CurrentRegion.Name);
            }
        }
    }
}
