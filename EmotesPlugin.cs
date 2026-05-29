using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using EmotesMod.Modules.Components;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace EmotesMod;

[BepInAutoPlugin("EmotesMod", "Emotes Mod", "1.3.0")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class EmotesPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);
    public override void Load()
    {
        ClassInjector.RegisterTypeInIl2Cpp<Emote>();
        ClassInjector.RegisterTypeInIl2Cpp<EmoteWheel>();
        ClassInjector.RegisterTypeInIl2Cpp<EmoteWheelItem>();
        ClassInjector.RegisterTypeInIl2Cpp<EmoteBehaviour>();
        ClassInjector.RegisterTypeInIl2Cpp<EmoteWheel>();
        Assets.Initialize();
        ReactorCredits.Register<EmotesPlugin>(location => true);
        Log.LogMessage("Emotes Mod Loaded Successfully! ^_____^");
        Harmony.PatchAll();
    }
}
