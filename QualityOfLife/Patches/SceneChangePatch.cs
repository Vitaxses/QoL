using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
        FastNPC(scene);
        Helper.Delay(() =>
        {
            SkipWeakness(scene);
            FastPickup(scene);
            OldPatch(scene);
            CreateSkipPlatforms(scene);
        }, 0.5f);
    }

    static void OldPatch(Scene to)
    {
        if (!QoLPlugin.OldPatch.Value) return;
        switch (to.name)
        {
            case "Under_17":
                GameObject obj = GameObject.Find("terrain collider (15)");
                if (obj == null) return;
                obj.transform.position = obj.transform.localPosition = new Vector3(12.25f, 7.64f, 0f);
                GameObject.Destroy(obj.GetComponent<NonSlider>());
                // Even if the component is turned off, it is still not possible to wall cling
        break;
        }
    }

    static void FastNPC(Scene to)
    {
        if (!QoLPlugin.FastNPC.Value) return;
        if (to.name == "Bone_04") PlayerData.instance.metMapper = true;
    }

    static void CreateSkipPlatforms(Scene to)
    {
        if (!QoLPlugin.SkipPlatforms.Value) return;
        string name = to.name.ToLower();
        switch (name)
        {
            case "bone_east_08_boss_golem":
                if (!PlayerData.instance.hasDash || PlayerData.instance.defeatedSongGolem || !PlayerData.instance.hasBrolly) return;
                Helper.SpawnPlatform(new Vector3(48.5f, 6.5f, 0f), false, GameObject.Find("bone_plat_02"), new Vector3(1.1f, 0.8f, 1f));
            break;

            case "bone_01":
                if (PlayerData.instance.bone01shortcutPlat) return;
                Helper.SpawnPlatform(new Vector3(104.5f, 69.5f, -0.09f), true, GameObject.Find("bone_plat_03 (2)"));
            break;

            case "crawl_01":
                if (PlayerData.instance.hasDash || PlayerData.instance.hasDoubleJump) return;
                Helper.SpawnPlatform(new Vector3(65f, 47f, -0.09f), true, GameObject.Find("bone_plat_02"), new Vector3(0.8f, 0.7f));
            break;

            case "crawl_03b":
                if (PlayerData.instance.hasDash || PlayerData.instance.hasDoubleJump) return;
                Helper.SpawnPlatform(new Vector3(17f, 10.5f, -0.09f), true, GameObject.Find("bone_plat_03"));
            break;

            case "aspid_01":
                if (PlayerData.instance.hasDash || PlayerData.instance.hasDoubleJump || PlayerData.instance.hasWalljump) return;
                Helper.SpawnPlatform(new Vector3(47f, 200f, -0.09f), true, GameObject.Find("bone_plat_03"));
            break;

            case "bellway_03":
                if (PlayerData.instance.hasBrolly || PlayerData.instance.hasDoubleJump || !PlayerData.instance.hasWalljump || PlayerData.instance.hasSuperJump) return;
                Helper.SpawnPlatform(new Vector3(110f, 6f, -0.09f), true, GameObject.Find("bone_plat_01 (2)"), size: new Vector3(1.0325f, 0.9829f, 1.0346f), spriteAlpha: 1f);
            break;

            case "peak_01":
            case "peak_07":
                if (PlayerData.instance.hasHarpoonDash || !PlayerData.instance.hasDash || PlayerData.instance.hasDoubleJump) return;
                if ((ToolItemManager.IsToolEquipped("Silk Charge") && PlayerData.instance.hasBrolly) || (ToolItemManager.IsToolEquipped("Thread Sphere") && ToolItemManager.IsToolEquipped("Flea Brew") && ToolItemManager.IsToolEquipped("Lifeblood Syringe") && PlayerData.instance.hasBrolly))
                {
                    var prefab = GameObject.Find("crumble_plat_peak_small (2)");
                    if (name.Equals("peak_01"))
                    {
                        Helper.SpawnPlatform(new Vector3(36f, 62f, 0.004f), false, prefab, spriteAlpha: 1f);
                        Helper.SpawnPlatform(new Vector3(20.5602f, 123f, 0.004f), false, prefab, spriteAlpha: 1f);
                        Helper.SpawnPlatform(new Vector3(28.5f, 154f, 0.004f), false, prefab, spriteAlpha: 1f);
                        Helper.SpawnPlatform(new Vector3(27f, 162f, 0.004f), false, prefab, spriteAlpha: 1f);
                        Helper.SpawnPlatform(new Vector3(60.9638f, 283.9561f, 0.004f), false, prefab, spriteAlpha: 1f);
                        Helper.SpawnPlatform(new Vector3(51.9965f, 286.0029f, 0.004f), false, prefab, spriteAlpha: 1f);
                    }
                    else
                    {
                        Helper.SpawnPlatform(new Vector3(32.4671f, 14f, 0.004f), false, prefab, spriteAlpha: 1f);
                        Helper.SpawnPlatform(new Vector3(43.7945f, 40.2256f, 0.004f), false, prefab, spriteAlpha: 1f);
                        Helper.SpawnPlatform(new Vector3(63.7128f, 48f, 0.004f), false, prefab, spriteAlpha: 1f);
                    }
                }
            break;
        }
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
                    if (!QoLPlugin.FastNPC.Value) return;
                    PlayerData.instance.SetBool(nameof(PlayerData.encounteredBellBeast), true);
                    PlayerData.instance.SetBool(nameof(PlayerData.seenBellBeast), true);
                })
                ).ToArray();
            break;

            case "bone_east_05":
                Fsm fsm1 = GameObject.Find("Shrine Weaver Ability").GetFsm();
                if (fsm1 == null) break;;
                FsmTransition transition1 = fsm1.GetState("Idle").GetTransition(0);
                transition1.toState = "Bind Prepare";
                transition1.toFsmState = fsm1.GetState("Bind Prepare");

                fsm1.GetState("End").Actions = fsm1.GetState("End").Actions.AddItem(
                new CallActionFsm(() =>
                {
                    if (QoLPlugin.FastNPC.Value) PlayerData.instance.SetBool(nameof(PlayerData.encounteredLace1), true);
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
                GameObject bossScene1 = GameObject.Find("Boss Scene");
                Fsm fsm2 = bossScene1.GetFsm("Battle End");
                FsmState endPauseState = fsm2.GetState("End Pause");
                FsmTransition transition2 = fsm2.GetState("Idle").GetTransition(0);
                transition2.toState = "End Pause";
                transition2.toFsmState = endPauseState;
                endPauseState.Actions = endPauseState.Actions.AddItem
                (
                    new CallActionFsm(() =>
                    {
                        PlayerData.instance.SetBool(nameof(PlayerData.HasSeenSilkHearts), true);
                        HeroController.instance.AddToMaxSilkRegen(1);
                        PlayerData.instance.SetBool(nameof(PlayerData.UnlockedFastTravel), true);
                        GameManager.instance.CheckAllAchievements();
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

            case "song_tower_01":
                Transform bossScene2 = GameObject.Find("Boss Scene").transform;
                GameObject.Destroy(bossScene2.GetChild(8).gameObject);
                Fsm fsm5 = bossScene2.GetChild(6).gameObject.GetFsm("Control");
                FsmState deathState = fsm5.GetState("Death");
                deathState.Actions = deathState.Actions.AddItem
                (
                    new CallActionFsm
                    (
                        () =>
                        {
                            PlayerData.instance.SetBool(nameof(PlayerData.HasSeenSilkHearts), true);
                            HeroController.instance.AddToMaxSilkRegen(1);
                            PlayerData.instance.defeatedLaceTower = true;
                            GameObject.Find("State Control/song_tower_right_gate").GetFsm().SetState("Init");
                            GameManager.instance.CheckAllAchievements();
                            bossScene2.transform.GetChild(8).gameObject.GetFsm().SetState("Silk Quest?");
                        }
                    )
                ).ToArray();
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
        if (skip)
        {
            GameObject WeaknessManager = GameObject.Find("Weakness Scene");
            if (WeaknessManager == null) return;
            WeaknessManager.SetActive(false);
            PlayerData.instance.churchKeeperIntro = true;
        }

        if (tName.Equals("cog_09_destroyed"))
        {
            GameObject WeaknessManager = GameObject.Find("Weakness Cog Drop Scene");
            if (WeaknessManager == null) return;
            WeaknessManager.SetActive(false);
        }
    }
}