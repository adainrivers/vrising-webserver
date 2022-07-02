using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using UnhollowerRuntimeLib;
using VRising.WebServer.Hosting;
using VRising.WebServer.Utils;
using Patches = VRising.WebServer.Patch.Patches;

namespace VRising.WebServer
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        public static WebHostServer WebServer;
        public static ManualLogSource Logger { get; private set; }
        public static Harmony HarmonyInstance { get; private set; }
        public static TaskRunner TaskRunner { get; set; }

        public override void Load()
        {
            Logger = Log;

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            HarmonyInstance = new Harmony(PluginInfo.PLUGIN_GUID);
            HarmonyInstance.PatchAll(typeof(Patches));

            if (!ClassInjector.IsTypeRegisteredInIl2Cpp<TaskRunner>())
            {
                ClassInjector.RegisterTypeInIl2Cpp<TaskRunner>();
            }

            TaskRunner = AddComponent<TaskRunner>();

            WebServer = new WebHostServer(80);
            WebServer.Start();

            WebServer.Router.MapGet("/users", DataFactory.GetUsers);

        }

        public override bool Unload()
        {
            WebServer.Stop();
            HarmonyInstance.UnpatchSelf();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is unloaded!");
            return true;
        }
    }
}
