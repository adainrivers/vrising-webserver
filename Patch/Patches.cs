using System;
using HarmonyLib;
using ProjectM;
using VRising.WebServer.Utils;

namespace VRising.WebServer.Patch
{
    public class Patches
    {
        [HarmonyPatch(typeof(LoadPersistenceSystemV2), "SetLoadState")]
        [HarmonyPostfix]
        private static void LoadPersistenceSystemV2SetLoadStatePostfix(ServerStartupState.State loadState, LoadPersistenceSystemV2 __instance)
        {
            try
            {
                Plugin.Logger.LogInfo($"{nameof(LoadPersistenceSystemV2)}.{nameof(LoadPersistenceSystemV2.SetLoadState)}: {loadState.ToString()}");
                if (loadState == ServerStartupState.State.SuccessfulStartup)
                {
                    WorldFactory.AssetsLoaded = true;
                }
            }
            catch (Exception e)
            {
                Plugin.Logger.LogError(e);
            }
        }
    }
}
