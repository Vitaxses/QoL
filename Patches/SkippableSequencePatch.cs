using GlobalEnums;
using HarmonyLib;

namespace QoL.Patches;

[HarmonyPatch(typeof(SkippableSequence))]
class SkippableSequencePatch
{
    [HarmonyPatch(typeof(SkippableSequence), nameof(SkippableSequence.CanSkip), MethodType.Getter)]
    [HarmonyPrefix]
    static bool Prefix(SkippableSequence __instance, ref bool __result)
    {
        if (!QoLPlugin.NoCutscenes.Value) return true;
        __instance.AllowSkip();
        __instance.canSkip = __result = true;
        return false;
    }
}

[HarmonyPatch(typeof(InputHandler), nameof(InputHandler.SetSkipMode))]
class InputHandlerPatch
{
    [HarmonyPrefix]
    static bool Prefix(InputHandler __instance, ref SkipPromptMode newMode)
    {
        string sceneName = GameManager.instance.sceneName;
        if (!QoLPlugin.NoCutscenes.Value || sceneName.Equals("End_Game_Completion")) return true;
        if (newMode == SkipPromptMode.NOT_SKIPPABLE && !sceneName.Equals("End_Credits") 
        && !sceneName.Equals("Opening_Sequence") && !sceneName.Equals("Bone_East_Umbrella")
        && !sceneName.Equals("Cinematic_Stag_travel") && !sceneName.Equals("Opening_Sequence_Act3")) return true;
        newMode = sceneName.Equals("End_Credits") ? SkipPromptMode.SKIP_INSTANT : SkipPromptMode.SKIP_PROMPT;
        __instance.readyToSkipCutscene = true;
        __instance.skipCooldownTime = 0d;
        __instance.SkipMode = newMode;
        return false;
    }
}