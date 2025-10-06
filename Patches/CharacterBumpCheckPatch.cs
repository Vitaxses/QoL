using HarmonyLib;

namespace QoL.Patches;

[HarmonyPatch(typeof(CharacterBumpCheck), nameof(CharacterBumpCheck.CheckForBump))]
class CharacterBumpCheckPatch
{

    [HarmonyPrefix]
    static bool CheckForBumpPrefix()
    {
        return !QoLPlugin.NoBump.Value;
    }
    
}