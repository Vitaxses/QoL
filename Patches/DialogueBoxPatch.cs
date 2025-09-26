using HarmonyLib;

namespace QoL.Patches;

[HarmonyPatch(typeof(DialogueBox))]
class DialogueBoxPatch
{
    [HarmonyPatch(typeof(DialogueBox), nameof(DialogueBox.Start))]
    static void Postfix(DialogueBox __instance)
    {
        if (QoLPlugin.InstantText.Value) __instance.currentRevealSpeed = __instance.regularRevealSpeed = __instance.fastRevealSpeed *= 4;
    }
}