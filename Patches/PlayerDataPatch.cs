using HarmonyLib;

namespace QoL.Patches;

[HarmonyPatch(typeof(PlayerData))]
class PlayerDataPatch
{
    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.SetupExistingPlayerData))]
    [HarmonyPostfix]
    static void ExistingDataPostfix(PlayerData __instance)
    {
        if (QoLPlugin.SeePercentage.Value) __instance.ConstructedFarsight = true;
    }

    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.SetupNewPlayerData))]
    [HarmonyPostfix]
    static void NewDataPostfix(PlayerData __instance)
    {
        if (QoLPlugin.SeePercentage.Value) __instance.ConstructedFarsight = true;
    }
}