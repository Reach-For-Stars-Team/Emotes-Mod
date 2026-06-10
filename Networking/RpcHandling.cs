using Reactor.Networking.Attributes;

namespace EmotesMod.Networking;

public static class RpcHandling
{
    [MethodRpc(255)]
    public static void RpcEmote(this PlayerControl player, string emoteName)
    {
        player.HandlePlayEmote(emoteName);
    }
    [MethodRpc(254)]
    public static void RpcCancelEmote(this PlayerControl player)
    {
        player.HandleStopEmote();
    }
    [MethodRpc(253)]
    public static void RpcUpdatePointer(this PlayerControl player, int rotation)
    {
        player.HandleUpdateCursorRotation(rotation);
    }
}