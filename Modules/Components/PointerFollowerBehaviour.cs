using System;
using EmotesMod.Networking;
using Reactor.Utilities;
using UnityEngine;

namespace EmotesMod.Modules.Components;

public class PointerFollowerBehaviour(IntPtr ptr) : MonoBehaviour(ptr)
{
    public SpriteRenderer spriteRenderer;
    public bool active = false;
    public int lastNetworkedAngle = 0;
    public PlayerControl pc;
    public int networkedThreshold = 10;
    private void FixedUpdate()
    {
        if (!pc.AmOwner) return;
        if (!active) return;
        
        transform.LookAt2d(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        spriteRenderer.flipY = transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270;
        if (Mathf.Abs(Mathf.DeltaAngle(lastNetworkedAngle, transform.eulerAngles.z)) > networkedThreshold && active && !OperatingSystem.IsAndroid())
        {
            //Network??
            lastNetworkedAngle = (int) transform.eulerAngles.z;
            pc.RpcUpdatePointer(lastNetworkedAngle);
            PluginSingleton<EmotesPlugin>.Instance.Log.LogInfo("Difference between last networked val and actual val exceeded, resending value...");
        }
    }

    public void SetActive(bool b, Emote emote = null)
    {
        active = b;
        spriteRenderer.enabled = b;
        if (emote == null) return;
        
        if (emote.PointerSprite.Value) spriteRenderer.sprite = emote.PointerSprite.Value;
        spriteRenderer.material = emote.PointerUsesPlayerColor.Value ? pc.cosmetics.bodySprites[0].BodySprite.material : new Material(Shader.Find("Sprites/Default"));
    }
}