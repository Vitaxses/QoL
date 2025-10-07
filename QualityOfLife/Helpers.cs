using System;
using System.Collections;
using System.Linq;
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

    public static GameObject SpawnPlatform(Vector3 pos, bool isDusty, GameObject reference, Vector3? size = null, float spriteAlpha = 0.7f)
    {
        GameObject plat = UnityEngine.Object.Instantiate(reference);
        plat.name = "QOL Skip-Helper";
        plat.transform.position = plat.transform.localPosition = pos;
        plat.transform.localScale = size ?? reference.transform.localScale;
        plat.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, spriteAlpha);
        if (isDusty) UnityEngine.Object.DestroyImmediate(plat.GetComponent<DustyPlatform>());
        return plat;
    }
}