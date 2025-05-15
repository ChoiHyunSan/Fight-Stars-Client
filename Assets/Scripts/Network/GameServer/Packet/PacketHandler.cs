using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using UnityEngine;
using UnityEngine.UIElements;

class PacketHandler
{
    public static void S_EnterRoomHandler(PacketSession session, IMessage message)
    {
        S_EnterRoom enterRoom = message as S_EnterRoom;
        ServerSession serverSession = session as ServerSession;
        if (enterRoom == null || serverSession == null)
            return;

        if(enterRoom.EnterResult != S_EnterRoom.Types.EnterResult.Success)
        {
#if UNITY_EDITOR
            Debug.LogError("Failed to enter room: " + enterRoom.EnterResult.ToString());
#endif
            return;
        }

        // Handle the S_EnterRoom packet
#if UNITY_EDITOR
        Debug.Log("S_EnterRoomHandler called with message: " + enterRoom.ToString());
#endif

        bool enterSuccess = true;

        PlayerFactory playerFactory = PlayerFactory.Instance;
        if (playerFactory != null)
        {
            foreach (var info in enterRoom.PlayerInfos)
            {
                GameObject player = playerFactory.CreatePlayer(
                    info.UserId,
                    info.CharacterId,
                    info.SkinId,
                    info.Nickname,
                    new Vector2 { x = info.SpawnPos.X, y = info.SpawnPos.Y });
                if (player == null)
                {
                    enterSuccess = false;    
                }
            }
        }

        // 세팅 완료 여부에 따라 다른 패킷 전송
        C_ReadyCompleteGame readyCompletePacket = new C_ReadyCompleteGame()
        {
            UserId = enterSuccess ? UserDataManager.Instance._userInfo.userId : -1
        };
        NetworkManager.Instance.Send(readyCompletePacket);
    }

    public static void S_ReadyCompleteGameHandler(PacketSession session, IMessage message)
    {
        S_ReadyCompleteGame enterRoom = message as S_ReadyCompleteGame;
        ServerSession serverSession = session as ServerSession;
        if (enterRoom == null || serverSession == null)
            return;

#if UNITY_EDITOR
        Debug.Log("S_ReadyCompleteGameHandler called with message: " + enterRoom.ToString());
#endif

        MatchManager.Instance.StartGame();
    }

    public static void S_PositionUpdateHandler(PacketSession session, IMessage message)
    {
        S_PositionUpdate positionUpdate = message as S_PositionUpdate;
        ServerSession serverSession = session as ServerSession;
        if (positionUpdate == null || serverSession == null)
            return;

#if UNITY_EDITOR
        Debug.Log("S_PositionUpdateHandler called with message: " + positionUpdate.ToString());
#endif

        MatchManager.Instance.UpdatePosition(positionUpdate.PlayerPosUpdates);
    }

    public static void S_FireHandler(PacketSession session, IMessage message)
    {
        S_Fire firePacket = message as S_Fire;
        ServerSession serverSession = session as ServerSession;
        if (firePacket == null || serverSession == null)
            return;

#if UNITY_EDITOR
        Debug.Log($"S_FireHandler called with message: {firePacket.ToString()}");
#endif
        MatchManager.Instance.UpdateFire(firePacket);
    }

    public static void S_AttackHandler(PacketSession session, IMessage message)
    {
        S_Attack attackPacket = message as S_Attack;
        ServerSession serverSession = session as ServerSession;
        if (attackPacket == null || serverSession == null)
            return;

#if UNITY_EDITOR
        Debug.Log($"S_AttackHandler called with message: {attackPacket.ToString()}");
#endif
        MatchManager.Instance.UpdateAttack(attackPacket);
    }

    public static void S_DieHandler(PacketSession session, IMessage message)
    {
        S_Die diePacket = message as S_Die;
        ServerSession serverSession = session as ServerSession;
        if (diePacket == null || serverSession == null)
            return;
#if UNITY_EDITOR
        Debug.Log($"S_DieHandler called with message: {diePacket.ToString()}");
#endif
        MatchManager.Instance.UpdateDie(diePacket);
    }

    public static void S_RespawnHandler(PacketSession session, IMessage message)
    {
        S_Respawn respawnPacket = message as S_Respawn;
        ServerSession serverSession = session as ServerSession;
        if (respawnPacket == null || serverSession == null)
            return;
#if UNITY_EDITOR
        Debug.Log($"S_RespawnHandler called with message: {respawnPacket.ToString()}");
#endif
        MatchManager.Instance.UpdateRespawn(respawnPacket);
    }

    public static void S_DestroyProjectileHandler(PacketSession session, IMessage message)
    {
        S_DestroyProjectile destroyProjectilePacket = message as S_DestroyProjectile;
        ServerSession serverSession = session as ServerSession;
        if (destroyProjectilePacket == null || serverSession == null)
            return;
#if UNITY_EDITOR
        Debug.Log($"S_DestroyProjectileHandler called with message: {destroyProjectilePacket.ToString()}");
#endif

        MatchManager.Instance.UpdateDestroyProjectile(destroyProjectilePacket);
    }
}
