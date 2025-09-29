using HarmonyLib;

namespace QoL.Patches;

[HarmonyPatch(typeof(PlayerData))]
class PlayerDataPatch
{
    public static string boolName = "Vitax's QOL CanSeePercentage";

    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.GetBool))]
    [HarmonyPrefix]
    static bool StartPrefix(PlayerData __instance, ref string boolName, ref bool __result)
    {
        if (boolName == PlayerDataPatch.boolName && (__instance.ConstructedFarsight || QoLPlugin.SeePercentage.Value))
        {
            __result = true;
            return false;
        }
        return true;
    }
}