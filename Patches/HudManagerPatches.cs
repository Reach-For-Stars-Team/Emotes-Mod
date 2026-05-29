using System;
using EmotesMod.Modules.Components;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace EmotesMod.Patches;

[HarmonyPatch]
public class HudManagerPatches
{
    private static GameObject _emoteCanvas;

    public static GameObject EmoteCanvas
    {
        get
        {
            if (_emoteCanvas.IsDestroyedOrNull()) LazyInstantiateHud();

            return _emoteCanvas;
        }
    }

    public static bool CanOpenEmoteWheel()
    {
        return MeetingHud.Instance == null && !PlayerControl.LocalPlayer.Data.IsDead && IntroCutscene.Instance == null;
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    [HarmonyPostfix]
    public static void HudManager_Update_Postfix(HudManager __instance)
    {
        if ( /*ReInput.players.GetPlayer(0).GetButtonDown(InputPatches.OpenEmoteWheelBind.id)*/
            Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftControl) && CanOpenEmoteWheel())
        {
            var wheel = EmoteCanvas.transform.GetChild(0).gameObject;
            wheel.SetActive(!wheel.activeSelf);
        }

        if ( /*ReInput.players.GetPlayer(0).GetButtonDown(InputPatches.StopEmotingBind.id)*/
            Input.GetKeyDown(KeyCode.X) && PlayerControl.LocalPlayer.GetComponent<EmoteBehaviour>().currentEmote)
            Networking.RpcStopEmote();
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    [HarmonyPostfix]
    public static void HudManager_Start_Postfix(HudManager __instance)
    {
        if (OperatingSystem.IsAndroid())
            CreateButtonAndroid();
        else
            __instance.Chat.AddChatWarning(
                "<size=130%>Thanks for downloading Emotes Mod!</size>\nTo open the emote wheel, press Control + E!");
    }

    private static void LazyInstantiateHud()
    {
        if (!_emoteCanvas.IsDestroyedOrNull()) return;

        _emoteCanvas = Object.Instantiate(Assets.EmoteCanvas);
        _emoteCanvas.transform.GetChild(0).gameObject.SetActive(false);
        _emoteCanvas.transform.GetChild(1).gameObject.SetActive(false);
        _emoteCanvas.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(new Action(() =>
        {
            Networking.RpcStopEmote();
            _emoteCanvas.transform.GetChild(1).gameObject.SetActive(false);
        }));
    }

    private static void CreateButtonAndroid()
    {
        var button = Object
            .Instantiate(HudManager.Instance.SettingsButton, HudManager.Instance.SettingsButton.transform.parent)
            .GetComponent<PassiveButton>();
        button.gameObject.name = "EmoteButton";
        button.OnClick = new Button.ButtonClickedEvent();
        button.OnClick.AddListener(new Action(() =>
        {
            var wheel = EmoteCanvas.transform.GetChild(0).gameObject;
            wheel.SetActive(!wheel.activeSelf);
        }));
        button.activeSprites.GetComponent<SpriteRenderer>().sprite = Assets.EmoteButtonHover;
        button.inactiveSprites.GetComponent<SpriteRenderer>().sprite = Assets.EmoteButton;
        button.GetComponent<AspectPosition>().DistanceFromEdge = new Vector3(4.75f, 0.505f, -400f);
    }
}