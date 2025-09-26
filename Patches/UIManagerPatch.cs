using HarmonyLib;

namespace QoL.Patches;

[HarmonyPatch(typeof(UIManager))]
class UIManagerPatch
{
    [HarmonyPatch(typeof(UIManager), nameof(UIManager.Start))]
    [HarmonyPostfix]
    static void Postfix(UIManager __instance)
    {
        if (QoLPlugin.FastMenu.Value) __instance.MENU_FADE_SPEED *= 7;
    }
}