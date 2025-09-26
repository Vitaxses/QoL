using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace QoL;

public static class Helper
{
    public static Fsm GetFsm(this GameObject obj, string wantedFSM = "")
    {
        PlayMakerFSM[] pfsm = obj.GetComponents<PlayMakerFSM>();
        if (pfsm.IsNullOrEmpty())
        {
            Debug.LogWarning($"Getting FSM on Object (obj: {obj.name} has no PlayMakerFSM component)");
            return null;
        }
        foreach (var fsm in pfsm)
        {
            if (fsm.FsmName.Equals(wantedFSM, StringComparison.OrdinalIgnoreCase)) return fsm.Fsm;
        }
        return pfsm[0].Fsm;
    }

    public static void Delay(Action action, float time, bool realtime = false)
    {
        if (GameManager.instance == null) return;
        GameManager.instance.StartCoroutine(delay(action, time, realtime));
    }

    private static IEnumerator delay(Action action, float time, bool realtime)
    {
        if (realtime) yield return new WaitForSeconds(time);
        else yield return new WaitForSecondsRealtime(time);
        action.Invoke();
    }
}