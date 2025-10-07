using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace QoL.Patches;

[HarmonyPatch(typeof(InventoryPaneInput))]
class InventoryPatch
{

    [HarmonyPatch(typeof(InventoryPaneInput), nameof(InventoryPaneInput.Start))]
    [HarmonyPostfix]
    static void StartPostfix(InventoryPaneInput __instance)
    {
        Fsm fsm = __instance.gameObject.GetFsm("UI Inventory");
        FsmState state = fsm.GetState("Completion Rate");
        if (state == null) return;
        FsmStateAction[] actions = state.Actions;

        if (actions[0] is PlayerDataBoolTest pdbt)
        {
            pdbt.boolName = PlayerDataPatch.boolName;
        }
    }
}