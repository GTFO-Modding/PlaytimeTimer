using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnhollowerRuntimeLib;

namespace PlaytimeTimer
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInDependency("Endskill.ExternalLogger", BepInDependency.DependencyFlags.SoftDependency)]
    public class BepInExLoader : BasePlugin
    {
        public const string
         MODNAME = "InMissionTime",
         AUTHOR = "Endskill",
         GUID = AUTHOR + "." + MODNAME,
         VERSION = "1.0.0";

        public override void Load()
        {
            //ClassInjector.RegisterTypeInIl2Cpp<PlayTimeShowHandler>();
            ClassInjector.RegisterTypeInIl2Cpp<TimeShowHandler>();

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }
    }
}
