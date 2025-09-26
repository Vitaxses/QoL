using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace QoL.Patches;

public class SceneChangePatch
{
    public static void Setup()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode lsm)
    {
        if (HeroController.instance == null) return;
        if (scene.name == "Bone_04") PlayerData.instance.metMapper = true;
        Helper.Delay(() =>
        {
            SkipWeakness(scene);
            FastPickup(scene);
        }, 0.5f);
    }

    static void FastPickup(Scene to)
    {
        if (!QoLPlugin.FastPickup.Value) return;
        switch (to.name.ToLower())
        {
            case "mosstown_02":
                Fsm fsm = GameObject.Find("Shrine Weaver Ability").GetFsm();
                if (fsm == null) break;
                FsmTransition transition = fsm.GetState("Idle").GetTransition(0);
                transition.toState = "Bind Prepare";
                transition.toFsmState = fsm.GetState("Bind Prepare");

                fsm.GetState("End").Actions = fsm.GetState("End").Actions.AddItem(
                new CallActionFsm(() =>
                {
                    PlayerData.instance.SetBool(nameof(PlayerData.encounteredBellBeast), true);
                    PlayerData.instance.SetBool(nameof(PlayerData.seenBellBeast), true);
                })
                ).ToArray();
                break;

            case "bone_east_05":
                Fsm fsm1 = GameObject.Find("Shrine Weaver Ability").GetFsm();
                if (fsm1 == null) break;
                FsmTransition transition1 = fsm1.GetState("Idle").GetTransition(0);
                transition1.toState = "Bind Prepare";
                transition1.toFsmState = fsm1.GetState("Bind Prepare");

                fsm1.GetState("End").Actions = fsm1.GetState("End").Actions.AddItem(
                new CallActionFsm(() =>
                {
                    PlayerData.instance.SetBool(nameof(PlayerData.encounteredLace1), true);
                })
                ).ToArray();
                break;

            case "shellwood_10":
                Fsm fsm3 = GameObject.Find("Shrine Weaver Ability").GetFsm();
                if (fsm3 == null) break;
                FsmTransition transition3 = fsm3.GetState("Idle").GetTransition(0);
                transition3.toState = "Bind Prepare";
                transition3.toFsmState = fsm3.GetState("Bind Prepare");
                break;

            case "bone_05_boss":
                GameObject bossScene = GameObject.Find("Boss Scene");
                Fsm fsm2 = bossScene.GetFsm("Battle End");
                FsmState endPauseState = fsm2.GetState("End Pause");
                FsmTransition transition2 = fsm2.GetState("Idle").GetTransition(0);
                transition2.toState = "End Pause";
                transition2.toFsmState = endPauseState;
                endPauseState.Actions = endPauseState.Actions.AddItem
                (
                    new CallActionFsm(() =>
                    {
                        HeroController.instance.AddToMaxSilk(1);
                        PlayerData.instance.SetBool(nameof(PlayerData.HasSeenSilkHearts), true);
                        PlayerData.instance.SetInt(nameof(PlayerData.silkRegenMax), 1);
                        PlayerData.instance.SetBool(nameof(PlayerData.UnlockedFastTravel), true);
                    })
                ).ToArray();
                /* This breaks the game (as in it loads it correctly but when leaving the scene and then leaving the next scene the game breaks)
                endPauseState.Actions = endPauseState.Actions.AddItem
                (
                    new CallActionFsm(() =>
                    {
                        foreach (Scene scene in SceneManager.GetAllScenes())
                        {
                            if (scene.name == "Bone_05_bellway") return;
                        }
                        Addressables.LoadSceneAsync("Scenes/Bone_05_bellway", LoadSceneMode.Additive, true, 100, SceneReleaseMode.ReleaseSceneWhenSceneUnloaded);
                    })
                ).ToArray();
                */ 
                break;

            case "under_18":
                Fsm fsm4 = GameObject.Find("Ability Scene/Shrine Weaver Ability").GetFsm();
                if (fsm4 == null) break;
                FsmTransition transition4 = fsm4.GetState("Idle").GetTransition(0);
                transition4.toState = "Bind Prepare";
                transition4.toFsmState = fsm4.GetState("Bind Prepare");
                break;
                
            default:
                break;
        }
    }

    static void SkipWeakness(Scene to)
    {
        if (!QoLPlugin.SkipWeakness.Value) return;
        string tName = to.name.ToLower();
        bool skip = false;
        if (tName.Equals("bonetown"))
        {
            skip = true;
            PlayerData.instance.completedTutorial = true;
            GameObject.Find("Churchkeeper Intro Scene").GetFsm().SetState("Set End"); // To get the First Quest (Citadel Seeker)
        }
        if (tName.Equals("tut_03") || tName.Equals("tut_01"))
        {
            skip = true;
        }
        if (skip) {
            GameObject WeaknessManager = GameObject.Find("Weakness Scene");
            if (WeaknessManager == null) return;
            WeaknessManager.SetActive(false);
            PlayerData.instance.churchKeeperIntro = true;
        }
    }
}