using System;
using System.Collections;
using System.Collections.Generic;
using EmotesMod;
using EmotesMod.Modules.Components;
using EmotesMod.Patches;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using PowerTools;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace EmotesMod.Modules.Components
{
    public class EmoteWheelItem(IntPtr ptr) : MonoBehaviour(ptr)
    {
        public Il2CppReferenceField<Emote> Emote;
        public Il2CppReferenceField<Image> icon;
        public Il2CppReferenceField<Button> button;

        public void OnClick()
        {
            Networking.RpcPlayEmote(Emote.Value.name);
            HudManagerPatches.EmoteCanvas.transform.GetChild(0).gameObject.SetActive(false);
            SoundManager.Instance.PlaySound(HudManager.Instance.MapButton.ClickSound, false);
        }


        public void Start()
        {
            if (Emote.Value == null)
            {
                button.Value.interactable = false;
                return;
            }

            icon.Value.sprite = Emote.Value.emoteIcon;
            icon.Value.material = new(HatManager.Instance.PlayerMaterial);
            PlayerMaterial.SetColors(new Color(0.7f, 0.7f, 0.8f, 1), icon.Value.material);
        }
    }
}