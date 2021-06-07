using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnhollowerRuntimeLib;

namespace PlaytimeTimer
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class BepInExLoader : BasePlugin
    {
        public const string
         MODNAME = "InMissionTime",
         AUTHOR = "Endskill",
         GUID = AUTHOR + "." + MODNAME,
         VERSION = "1.0.0";

        public static ConfigEntry<bool> EndskillTime;

        public override void Load()
        {
            EndskillTime = Config.Bind("EndskillTime", "IsActive", false, "Counts up to 69 instead of 60");

            //ClassInjector.RegisterTypeInIl2Cpp<PlayTimeShowHandler>();
            ClassInjector.RegisterTypeInIl2Cpp<TimeShowHandler>();

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }
    }
}
