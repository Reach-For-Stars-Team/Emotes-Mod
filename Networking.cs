using System.Linq;
using System.Text;
using EmotesMod.Modules.Components;
using Il2CppSystem;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;

namespace EmotesMod;

public static class Networking
{
    public static void RpcPlayEmote(string emoteName)
    {
        var id = Assets.EmotesNames.First(x => x.Value == emoteName).Key;
        var oldLvl = PlayerControl.LocalPlayer.Data.PlayerLevel;
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
        var oldLvl = PlayerControl.LocalPlayer.Data.PlayerLevel;
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
}