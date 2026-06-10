using System;
using System.Runtime.InteropServices;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using JetBrains.Annotations;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace EmotesMod.Modules.Components
{
    public class Emote(IntPtr ptr) : ScriptableObject(ptr)
    {
        public Il2CppReferenceField<AnimationClip> Animation;
        public Il2CppReferenceField<Sprite> EmoteIcon;
        public Il2CppValueField<bool> PlayLooped;
        public Il2CppValueField<bool> CanMove;
        public Il2CppReferenceField<Sprite> PointerSprite;
        public Il2CppValueField<bool> PointerUsesPlayerColor;
    }
}