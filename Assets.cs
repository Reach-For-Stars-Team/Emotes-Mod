using System.Collections.Generic;
using System.Linq;
using EmotesMod.Modules.Components;
using Il2CppInterop.Runtime;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace EmotesMod;

public static class Assets
{
    public static AssetBundle Bundle;
    public static GameObject EmoteCanvas;
    public static Sprite EmoteButton;
    public static Sprite EmoteButtonHover;
    public static Dictionary<uint, string> EmotesNames = new Dictionary<uint, string>();
    public static void Initialize()
    {
        Bundle = AssetBundleManager.Load("emotes");
        EmoteCanvas = Bundle.LoadAsset<GameObject>("EmoteCanvas.prefab")?.DontUnload();
        EmoteButton = Bundle.LoadAsset<Sprite>("EmoteButton.png")?.DontUnload();
        EmoteButtonHover = Bundle.LoadAsset<Sprite>("EmoteButtonHover.png")?.DontUnload();

        foreach (var obj in Bundle.LoadAllAssets(Il2CppType.Of<Emote>()))
        {
            var e = obj.TryCast<Emote>();
            if (e != null) EmotesNames.Add(ToHash(e.name), e.name);
        }
    }
    private static uint ToHash(string name)
    {
        unchecked
        {
            uint hash = 2166136261u;
            foreach (char c in name)
            {
                hash ^= (byte)c;
                hash *= 16777619u;
            }
            return hash;
        }
    }
}