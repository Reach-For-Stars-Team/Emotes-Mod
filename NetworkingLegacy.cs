using System.Linq;
using System.Text;
using AmongUs.Data.Player;
using EmotesMod.Modules.Components;
using Il2CppSystem;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace EmotesMod;

public static class NetworkingLegacy
{
    public static void RpcPlayEmote(string emoteName)
    {
        var id = Assets.EmotesNames.First(x => x.Value == emoteName).Key;
        var oldLvl = AmongUs.Data.DataManager.Player.Stats.Level;
        PlayerControl.LocalPlayer.RpcSetLevel(id);
        PlayerControl.LocalPlayer.StartCoroutine(Effects.ActionAfterDelay(0.05f,
            new System.Action(() => PlayerControl.LocalPlayer.RpcSetLevel(oldLvl))));
        HandlePlayEmote(PlayerControl.LocalPlayer, emoteName);
    }

    public static void HandlePlayEmote(this PlayerControl p, string emoteName)
    {
        var emote = Assets.Bundle.LoadAsset<Emote>(emoteName);
        if (emote == null) return;
        var emoteBehaviour = p.GetComponent<EmoteBehaviour>();
        emoteBehaviour.StopEmote();
        emoteBehaviour.currentEmote = emote;
        emoteBehaviour.PlayEmote();
    }
    
    public static void RpcStopEmote()
    {
        var oldLvl = AmongUs.Data.DataManager.Player.Stats.Level;
        PlayerControl.LocalPlayer.RpcSetLevel(0);
        PlayerControl.LocalPlayer.StartCoroutine(Effects.ActionAfterDelay(0.05f,
            new System.Action(() => PlayerControl.LocalPlayer.RpcSetLevel(oldLvl))));
        HandleStopEmote(PlayerControl.LocalPlayer);
    }

    public static void HandleStopEmote(this PlayerControl p)
    {
        var emoteBehaviour = p.GetComponent<EmoteBehaviour>();
        emoteBehaviour.StopEmote();
    }
    
    public static void RpcUpdateCursorRotation(this PlayerControl p, uint value)
    {
        var oldLvl = AmongUs.Data.DataManager.Player.Stats.Level;
        p.RpcSetLevel(value);
        p.StartCoroutine(Effects.ActionAfterDelay(0.05f,
            new System.Action(() => p.RpcSetLevel(oldLvl))));
    }
    
    public static void HandleUpdateCursorRotation(this PlayerControl p, int value)
    {
        var cursor = p.GetComponentInChildren<PointerFollowerBehaviour>();
        if (cursor.active)
        {
            cursor.transform.eulerAngles = new Vector3(0, 0, value);
            cursor.spriteRenderer.flipY = cursor.transform.eulerAngles.z > 90 && cursor.transform.eulerAngles.z < 270;
        }
    }
}