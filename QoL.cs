using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using QoL.Patches;

namespace QoL;

[BepInAutoPlugin(id: ID, name: "QoL")]
public partial class QoLPlugin : BaseUnityPlugin
{
    const string ID = "vitaxses.qol";
    Harmony harmony = new(ID);
    public static ManualLogSource Logger { get; private set; }

    public static ConfigEntry<bool> SkipWeakness { get; private set; }
    public static ConfigEntry<bool> FastMenu { get; private set; }
    public static ConfigEntry<bool> FasterLevers { get; private set; }
    public static ConfigEntry<bool> InstantText { get; private set; }
    public static ConfigEntry<bool> FastPickup { get; private set; }
    public static ConfigEntry<bool> SeePercentage { get; private set; }
    public static ConfigEntry<bool> NoCutscenes { get; private set; }

    private void Awake()
    {
        harmony.PatchAll(typeof(UIManagerPatch));
        harmony.PatchAll(typeof(PlayerDataPatch));
        harmony.PatchAll(typeof(SkippableSequencePatch));
        harmony.PatchAll(typeof(InputHandlerPatch));
        harmony.PatchAll(typeof(DialogueBoxPatch));
        harmony.PatchAll(typeof(LeverPatch));
        harmony.PatchAll(typeof(Lever_tk2dPatch));
        SceneChangePatch.Setup();
        Logger = base.Logger;
        SkipWeakness = Config.Bind("Settings", "SkipWeakness", true);
        FastMenu = Config.Bind("Settings", "FastMenu", true);
        FasterLevers = Config.Bind("Settings", "FasterLevers", true);
        InstantText = Config.Bind("Settings", "InstantText", true);
        FastPickup = Config.Bind("Settings", "FastPickup", true);
        NoCutscenes = Config.Bind("Settings", "NoCutscenes", true);
        SeePercentage = Config.Bind("Settings", "SeePercentage", false);
        Logger.LogInfo($"{ID} loaded!");
    }
}