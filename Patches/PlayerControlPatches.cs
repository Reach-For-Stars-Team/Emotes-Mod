using AmongUs.Data;
using EmotesMod.Modules.Components;
using HarmonyLib;
using Hazel;
using UnityEngine;

namespace EmotesMod.Patches;

[HarmonyPatch]
public class PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Awake))]
    [HarmonyPostfix]
    public static void PlayerControl_Awake_Postfix(PlayerControl __instance)
    {
        var emoteBehaviour = __instance.gameObject.AddComponent<EmoteBehaviour>();
        emoteBehaviour.pc = __instance;
        __instance.MyPhysics.Animations.glowAnimator.gameObject.SetActive(false);

        var pointerFollower = new GameObject("PointerFollower");
        pointerFollower.transform.SetParent(__instance.gameObject.transform);
        pointerFollower.transform.localPosition = Vector3.zero;
        
        var rendObject = new GameObject("Renderer");
        rendObject.transform.SetParent(pointerFollower.transform);
        rendObject.transform.localPosition = new (1, 0, 0);

        var pointerFollowerBehaviour = pointerFollower.AddComponent<PointerFollowerBehaviour>();
        pointerFollowerBehaviour.pc = __instance;
        pointerFollowerBehaviour.spriteRenderer = rendObject.AddComponent<SpriteRenderer>();
        
        emoteBehaviour.pointerFollowerBehaviour = pointerFollowerBehaviour;
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.HandleAnimation))]
    [HarmonyPrefix]
    public static bool PlayerPhysics_Awake_Prefix(PlayerPhysics __instance)
    {
        var emoteBehaviour = __instance.GetComponent<EmoteBehaviour>();
        if (emoteBehaviour == null || emoteBehaviour.currentEmote == null) return true;
        if (emoteBehaviour.currentEmote.CanMove.Value)
        {
            __instance.FlipX = __instance.Velocity.x <= 0;
            return false;
        }

        return true;
    }
    
    //Legacy networking code
    /*
    [HarmonyPatch(typeof(NetworkedPlayerInfo), nameof(NetworkedPlayerInfo.UpdateLevel))]
    [HarmonyPrefix]
    public static bool NetworkedPlayerInfo_UpdateSkin_Prefix(NetworkedPlayerInfo __instance, ref uint level)
    {
        var emoteBehaviour = __instance.Object.GetComponent<EmoteBehaviour>();
        if (emoteBehaviour.pointerFollowerBehaviour.actualLevel == 999) emoteBehaviour.pointerFollowerBehaviour.actualLevel = AmongUs.Data.DataManager.Player.Stats.Level;
        if (level == 0)
        {
            __instance.Object.HandleStopEmote();
            return false;
        }
        if (Assets.EmotesNames.TryGetValue(level, out var emoteName))
        {
            __instance.Object.HandlePlayEmote(emoteName);
            return false;
        }

        if (emoteBehaviour.pointerFollowerBehaviour.active && level != emoteBehaviour.pointerFollowerBehaviour.actualLevel)
        {
            //Fallback to handling cursor pos
            __instance.Object.HandleUpdateCursorRotation(level);
            return false;
        }

        return true;
    }*/
    
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnGameEnd))]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnGameStart))]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ResetForMeeting))]
    [HarmonyPostfix]
    public static void PlayerControl_Die_Postfix(PlayerControl __instance)
    {
        if (HudManagerPatches.EmoteCanvas) HudManagerPatches.EmoteCanvas.transform.GetChild(0).gameObject.SetActive(false);
        __instance.gameObject.GetComponent<EmoteBehaviour>().StopEmote();
    }
}