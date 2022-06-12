using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using PlaytimeDisplay.Manager;
using PlaytimeDisplay.Scripts;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace PlaytimeDisplay
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInDependency(EndskApi.BepInExLoader.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
    public class BepinexLoader : BasePlugin
    {
        public const string
         MODNAME = "Playtime",
         AUTHOR = "Endskill",
         GUID = "dev.gtfomodding." + MODNAME,
         VERSION = "1.0.0";

        public override void Load()
        {
            LogManager._debugMessagesActive = Config.Bind("Dev Settings", "DebugMessages", false, "This settings activates/deactivates debug messages in the console for this specific plugin.").Value;
            ClassInjector.RegisterTypeInIl2Cpp<Playtime>();

            LogManager.SetLogger(Log);

            EndskApi.Api.InitApi.AddInitCallback(Init);
        }

        private void Init()
        {
            var gameObj = new GameObject("PlaytimeDisplay_Endskill");
            var playTime = gameObj.AddComponent<Playtime>();
            EndskApi.Api.CacheApi.SaveInstance(playTime);

            EndskApi.Api.LevelApi.AddBeginLevelCallback(playTime.StartPlaytime);
            EndskApi.Api.LevelApi.AddEndLevelCallback(playTime.SavePlaytime);
        }
    }
}
