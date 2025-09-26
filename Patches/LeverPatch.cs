using HarmonyLib;

namespace QoL.Patches;


/* The 'Lever' class doesn't seem to be used but not fully sure (its not like im going to test every lever in the game) */
[HarmonyPatch(typeof(Lever))]
class LeverPatch
{
    [HarmonyPatch(typeof(Lever), nameof(Lever.Start))]
    [HarmonyPostfix]
    static void LeverStartPostfix(Lever __instance)
    {
        if (QoLPlugin.FasterLevers.Value)
        {
            __instance.openGateDelay = 0f;
        }
    }
}

[HarmonyPatch(typeof(Lever_tk2d))]
class Lever_tk2dPatch {

    [HarmonyPatch(typeof(Lever_tk2d), nameof(Lever_tk2d.Start))]
    [HarmonyPostfix]
    static void TK2DLeverStartPostfix(Lever_tk2d __instance)
    {
        if (QoLPlugin.FasterLevers.Value)
        {
            __instance.openGateDelay = 0f;
        }
    }

    /*
    [HarmonyPatch(typeof(Lever_tk2d), nameof(Lever_tk2d.fsmActivateEvent), MethodType.Getter)]
    [HarmonyPrefix]
    static bool TK2DLeverEventPrefix(ref string __result)
    {
        if (!QoLPlugin.FasterLevers.Value) return true;
        __result = "ACTIVATE";
        return false;
    }*/
}