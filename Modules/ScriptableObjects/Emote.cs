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
        public Il2CppReferenceField<AnimationClip> anim;
        public Il2CppReferenceField<Sprite> emoteIcon;
        //Temp solution, will need to find a better way.
        public bool playLooped => name.Contains("loop");
        public bool canMove => name.Contains("moveable");
    }
}