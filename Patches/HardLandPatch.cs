using System;
using HarmonyLib;

namespace QoL.Patches;

[HarmonyPatch(typeof(HeroController))]
class HardLandPatch
{

    [HarmonyPatch(typeof(HeroController), nameof(HeroController.ShouldHardLand))]
    [HarmonyPrefix]
    static bool ShouldHardLandPrefix(ref bool __result)
    {
        if (QoLPlugin.NoHardFalls.Value) return __result = false;
        return true;
    }
}